using UnityEngine;
using System.Collections;

public class MapEditor : MonoBehaviour 
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 mouseStartPos = Input.mousePosition;
            DragMap(mouseStartPos);
        }
    }

    private void DragMap(Vector3 mouseStartPos)
    {
        if (Input.mousePosition.x > mouseStartPos.x)
        {
            
        }
    }
}
