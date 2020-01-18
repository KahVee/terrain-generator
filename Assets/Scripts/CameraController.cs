using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Super simple and barebones class for moving the camera
public class CameraController : MonoBehaviour
{
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public float speed = 5;

    private Vector3 rot;

    //Called on start
    void Start() {
        rot = transform.eulerAngles;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Called once a frame
    void Update() {
        UpdateRotation();
        UpdateLocation();
    }

    void UpdateRotation() {
        rot = transform.eulerAngles;
        float yRot = Input.GetAxisRaw("Mouse X") * XSensitivity;
        float xRot = Input.GetAxisRaw("Mouse Y") * YSensitivity;

        float x = rot.x - xRot;
        Debug.Log(x);
        if(0 < x && x < 100 ) {
            x = Mathf.Clamp(x, 0, 90);
        } else if(x < 0) {
            x = 359.9f;
        } else {
            x = Mathf.Clamp(x, 270, 360);
        }

        transform.eulerAngles = new Vector3(x, rot.y + yRot, 0);        

        if(Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void UpdateLocation() {
        Vector3 pos = transform.localPosition;
        if (Input.GetKey(KeyCode.W)) {
            pos += transform.forward * speed;
        }
        if(Input.GetKey(KeyCode.S)) {
            pos -= transform.forward * speed;
        }
        if(Input.GetKey(KeyCode.D)) {
            pos += transform.right * speed;
        }
        if(Input.GetKey(KeyCode.A)) {
            pos -= transform.right * speed;
        }
        transform.localPosition = pos;
    }
}
