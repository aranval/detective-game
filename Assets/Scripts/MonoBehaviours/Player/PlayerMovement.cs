using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour {
    public Animator animator;
    public NavMeshAgent agent;
    public float inputHoldDelay = 0.6f;
    public float turnSpeedTreshold = 0.6f;
    public float speedDampTime = 0.1f; //speed change delay
    public float slowingSpeed = 0.175f;
    public float turnSmoothing = 8f; //higher quicker turn less smooth
    private WaitForSeconds inputHoldWait;
    private Vector3 destinationPosition;
    private bool inputEnabled = true;
    private const float stopDistanceProportion = 0.1f;
    private const float navMeshSampleDistance = 4f; //distance how far from the click navmesh can be for player to react to it
    public float lightDistanceConstraint = 8f;

    private readonly int hashSpeedParameter = Animator.StringToHash("Speed");
    private readonly int hashLocomotionTag = Animator.StringToHash("Locomotion");
    private Interactable tmpInteractable;

    void Start() {
        agent.updateRotation = false;
        inputHoldWait = new WaitForSeconds(inputHoldDelay);
        destinationPosition = transform.position;
    }

    private void Stopping(out float speed) {
        agent.isStopped = true;
        transform.position = destinationPosition;
        speed = 0f;

        if (tmpInteractable) {
            transform.rotation = tmpInteractable.interactionLocation.rotation;
            tmpInteractable.Interact();
            tmpInteractable = null;
            StartCoroutine(WaitForInteraction());
        }
    }

    private IEnumerator WaitForInteraction() {
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).tagHash);
        inputEnabled = false;
        yield return inputHoldWait;

        while (animator.GetCurrentAnimatorStateInfo(0).tagHash != hashLocomotionTag) {
            yield return null;
        }
        inputEnabled = true;
    }

    private void Slowing(out float speed, float distanceToDestination) {
        agent.isStopped = true;
        transform.position = Vector3.MoveTowards(transform.position, destinationPosition, slowingSpeed * Time.deltaTime);
        float proportionalDistance = 1f - distanceToDestination / agent.stoppingDistance;
        speed = Mathf.Lerp(slowingSpeed, 0f, proportionalDistance);
    }

    /**
     * if gameObject with tag "LightControl" is in proximity, turn it on
     */

    private void ToggleLights() {
        GameObject test = GameObject.Find("Point Light_White (1)");
        GameObject[] lights = GameObject.FindGameObjectsWithTag("LightControl");
        if(lights != null && lights.Length > 0) {
            for (int i = 0; i < lights.Length; i++)
            {
                float lightDistance = Vector3.Distance(lights[i].transform.position, transform.position);
                Light tmpLight = lights[i].GetComponent<Light>();
                if(lightDistance < lightDistanceConstraint) { 
                    tmpLight.enabled = true;
                } else {
                    tmpLight.enabled = false;
                }
            }
        }
    }

    void Update() {
        ToggleLights();
        if (agent.pathPending) {
            return;
        }

        float speed = agent.desiredVelocity.magnitude; //legacy code, fajnie by bylo znalezc jakis reference pisania czegos takiego up2date

        if (agent.remainingDistance <= agent.stoppingDistance * stopDistanceProportion) {
            Stopping(out speed);
        } else if (agent.remainingDistance <= agent.stoppingDistance) {
            Slowing(out speed, agent.remainingDistance);
        } else if (speed > turnSpeedTreshold) {
            Rotate();
        }

        animator.SetFloat(hashSpeedParameter, speed, speedDampTime, Time.deltaTime); //set animator move speed, animator controls navmeshagent move speed
    }

    private void Rotate() {
        Quaternion targetRotation = Quaternion.LookRotation(agent.desiredVelocity);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);

    }

    /**
     * OnGroundClick:
     * https://docs.unity3d.com/Manual/nav-MoveToClickPoint.html
     */
    public void OnGroundClick(BaseEventData data) {
        if (!inputEnabled) {
            Debug.Log("Input disabled! Cannot move the character!");
            return;
        }
        tmpInteractable = null;
        PointerEventData pData = (PointerEventData) data;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(pData.pointerCurrentRaycast.worldPosition, out hit, navMeshSampleDistance, NavMesh.AllAreas)) {
            destinationPosition = hit.position;
        } else {
            destinationPosition = pData.pointerCurrentRaycast.worldPosition;
        }

        agent.SetDestination(destinationPosition);
        agent.isStopped = false;
    }

    public void OnInteractableClick(Interactable interactable) {
        if (!inputEnabled) {
            Debug.Log("Input disabled! Cannot interact with anything!");
            return;
        }

        tmpInteractable = interactable;
        destinationPosition = tmpInteractable.interactionLocation.position;
        agent.SetDestination(destinationPosition);
        agent.isStopped = false;
    }

    public void SetPlayerMovementInputEnabled(bool flag = true) {
        inputEnabled = flag;
    }
}