using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    //for dragging camera 
    public static bool drag = true;//disable when clicking on hud items etc
    Vector3 resetCamera;//try and set this to the center of the map

    Camera _camera;//for dragging map
    Vector3 cameraPos;//how far to drag the map

    float mouseX;//drag map on x
    float mouseY;//drag map on y

    void Start()
    {
        _camera = Camera.main;
        resetCamera = Camera.main.transform.position;
    }
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            DragMouse();//drag camera
        }
        //zoom in and out with mouse wheel
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && Camera.main.orthographicSize < 30)//back
        {
            Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize + 1, 10);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize > 1) //forward
        {
            Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize - 1, 10);
        }
    }
    private void DragMouse()
    {
        if (drag == true)
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
            //how far to drag camera 
            cameraPos = new Vector3(-mouseX, -mouseY, 0);
            //move camera
            _camera.transform.position += cameraPos;
        }
    }
}
