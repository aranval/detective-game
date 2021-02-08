using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectZoomPanel : MonoBehaviour
{
    private Image bgImage;
    private GameObject zoomedObject;
    private Camera playerCamera;
    public Camera zoomCamera;
    private Vector3 positionInFrontOfTheCamera = Vector3.zero;
    private Canvas zoomCanvas;
    private PlayerMovement playerMovement;
    private bool allowRotating = false;
    public float rotateSensitivity = 1f;
    private bool useExistingObject;

    private void Awake() {
        bgImage = GetComponentInChildren<Image>();
        bgImage.gameObject.SetActive(false);
        zoomCanvas = GetComponent<Canvas>();
        zoomCanvas.planeDistance = 2f;
        zoomCamera.gameObject.SetActive(false);
    }

    // todo: add mechanics for object zooming and rotating
    public void ZoomOnObject(GameObject _zoomedObject, float _cameraZoom, bool _useExistingObject) {
        useExistingObject = _useExistingObject;
        // get the player movement script
        if (playerMovement == null) playerMovement = FindObjectOfType<PlayerMovement>();
        // disable player movement input
        playerMovement.SetPlayerMovementInputEnabled(false);
        // get the position of the Camera
        if (playerCamera == null) playerCamera = Camera.main;
        if (playerCamera == null) Debug.LogError("Player camera not found!");
        playerCamera.gameObject.SetActive(false);
        zoomCamera.gameObject.SetActive(true);
        // show the wanted object in the center of the panel
        zoomedObject = useExistingObject ? _zoomedObject : Instantiate(_zoomedObject);
        zoomedObject.transform.SetParent(zoomCamera.transform);
        // get the position in front of the camera
        positionInFrontOfTheCamera = zoomCamera.transform.forward;
        zoomedObject.transform.localPosition = positionInFrontOfTheCamera;
        // set the camera size accordingly
        zoomCamera.orthographicSize = 1f / _cameraZoom;
        // show background to block RayCast
        bgImage.gameObject.SetActive(true);
        // allow rotation of the object
        allowRotating = true;
    }

    public void StopZoom() {
        zoomCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
        // destroy the object's clone
        if (!useExistingObject) Destroy(zoomedObject);
        // disallow rotation of the object
        allowRotating = false;
        // hide the background 
        bgImage.gameObject.SetActive(false);
        // enable player movement input
        playerMovement.SetPlayerMovementInputEnabled();
    }

    public void Update() {
        if (allowRotating) {
            // if the player holds the button
            if (Input.GetButton("Fire1")) {
                // rotate object
                Vector2 mouseDelta = Vector2.zero;
                mouseDelta.x = Input.GetAxis("Mouse X");
                mouseDelta.y = Input.GetAxis("Mouse Y");
                if (mouseDelta != Vector2.zero) {
                    //
                    // the following section is a modified code from:
                    // https://answers.unity.com/questions/299126/how-to-rotate-relative-to-camera-angleposition.html
                    // courtesy of Raigex
                    //
                    // get the world vector space for cameras up vector 
                    Vector3 relativeUp = playerCamera.transform.TransformDirection(Vector3.up);
                    // get world vector for space cameras right vector
                    Vector3 relativeRight = playerCamera.transform.TransformDirection(Vector3.right);
                    // turn relativeUp vector from world to objects local space
                    Vector3 objectRelativeUp = zoomedObject.transform.InverseTransformDirection(relativeUp);
                    // turn relativeRight vector from world to object local space
                    Vector3 objectRelaviveRight = zoomedObject.transform.InverseTransformDirection(relativeRight);
                    // calculate desired rotation
                    Quaternion desiredRotation = Quaternion.AngleAxis(mouseDelta.x / zoomedObject.gameObject.transform.localScale.x * rotateSensitivity, objectRelativeUp)
                        * Quaternion.AngleAxis(-mouseDelta.y / zoomedObject.gameObject.transform.localScale.x  * rotateSensitivity, objectRelaviveRight);
                    // rotate
                    zoomedObject.transform.Rotate(desiredRotation.eulerAngles);
                }
            }
        }
    }
}
