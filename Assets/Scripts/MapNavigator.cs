using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNavigator : MonoBehaviour
{
    public Transform cam;
    public float cameraSpeed;
    public float edgeTolerance = 0.1f;
    public float rotationAngle = 45.0f;
    public float horizontalLimit = 25.0f;
    public float verticalLimit = 25.0f;

    private Quaternion fromAngle;
    private Quaternion toAngle;
    
    private float mouseAngle;
    private Vector2 mousePosition;

    public void RotateLeft(){
        cam.transform.Rotate(Vector3.up,rotationAngle,Space.World);
    }
    public void RotateRight(){
        cam.transform.Rotate(Vector3.up,-rotationAngle,Space.World);
    }
    
    public void Update() {
        mousePosition.x = Input.mousePosition.x - Screen.width*0.5f;
        mousePosition.y = Input.mousePosition.y - Screen.height*0.5f;
        mouseAngle = Mathf.Rad2Deg * Mathf.Atan2(mousePosition.y,mousePosition.x);
        if(mouseAngle < 0) {
            mouseAngle += 360;
        }
        
        if ( cam.transform.position.x <= horizontalLimit && cam.transform.position.x >= -horizontalLimit && cam.transform.position.z <= verticalLimit && cam.transform.position.z >= -verticalLimit) {
            if ( ( (Input.mousePosition.y > Screen.height * (1 - edgeTolerance) || Input.mousePosition.y < Screen.height * edgeTolerance) || 
                ( Input.mousePosition.x > Screen.width * (1 - edgeTolerance) || Input.mousePosition.x < Screen.width * edgeTolerance) ) ) {
                cam.transform.Translate(Vector3.right * Time.deltaTime * cameraSpeed * Mathf.Cos(Mathf.Deg2Rad * mouseAngle));
                cam.transform.Translate(Vector3.forward * Time.deltaTime * cameraSpeed * Mathf.Sin(Mathf.Deg2Rad * mouseAngle));
            }     
        }
        if(cam.transform.position.x > horizontalLimit) {
                cam.transform.position = new Vector3(horizontalLimit, cam.transform.position.y, cam.transform.position.z);
            }
            if(cam.transform.position.x < -horizontalLimit) {
                cam.transform.position = new Vector3(-horizontalLimit, cam.transform.position.y, cam.transform.position.z);
            }
            if(cam.transform.position.z > verticalLimit) {
                cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y,verticalLimit);
            }
            if(cam.transform.position.z < -verticalLimit) {
                cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y,-verticalLimit);
        }
    }
}
