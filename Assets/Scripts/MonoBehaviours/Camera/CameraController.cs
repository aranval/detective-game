using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * TODO: Create ENUM cameraType, switchloop for types both on start n update
 */
public class CameraController : MonoBehaviour {

    public float smoothing = 9f;
    public Vector3 offset = new Vector3(0f, 1.5f, 0f);
    public Transform playerPosition;
    private float currentDistance;
    void Start() {
        if (playerPosition == null) return;
        CheckCameraPosition();
        transform.rotation = Quaternion.LookRotation(playerPosition.position - transform.position + offset);
    }

    private void CheckCameraPosition(){ 
        currentDistance = Vector3.Distance(transform.position, playerPosition.position); 
        GameObject[] cameraRigs = GameObject.FindGameObjectsWithTag("CameraControl");

        if (cameraRigs != null && cameraRigs.Length > 0) {
            for (int i = 0; i < cameraRigs.Length; i++) {
                Transform tmpTransform = cameraRigs[i].transform;
                float cameraRigDistance = Vector3.Distance(cameraRigs[i].transform.position, playerPosition.position);
                if (cameraRigDistance < currentDistance) MoveCamera(tmpTransform.position);
            }
        }
    }

    private void MoveCamera(Vector3 position) => transform.position = position;

    void Update() {
        if (playerPosition == null) return;
        CheckCameraPosition();
        Quaternion newRotation = Quaternion.LookRotation(playerPosition.position - transform.position + offset);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * smoothing);
    }

}