using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexGrid : MonoBehaviour {

    public Transform spawnThis;
    public GameObject hexInstance;
    Transform grid;
    List<GameObject> hexList;
    
    public int _x = 60;
    public int _y = 60;

    public float radius = 0.5f;
    public bool useAsInnerCircleRadius = true;

    private float offsetX, offsetY;

    //hover effect
    int hexMask;
    public static string hoverHex;
    //place Hex
    public GameObject forrestHex;
    public GameObject grassHex;
    public GameObject waterHex;
    public GameObject defaultHex;

    void Start()
    {
        //when I get the main menu working this is how the user will set the size
        //_x = MainMenu.mapWidth;
        //_y = MainMenu.mapHeight;

        //this is for creating the grid
        float unitLength = ( useAsInnerCircleRadius )? (radius / (Mathf.Sqrt(3)/2)) : radius;
        offsetX = unitLength * Mathf.Sqrt(3);
        offsetY = unitLength * 1.5f;

        //set up raycast for hover effect
        hexMask = LayerMask.GetMask("HexLayer");

        SetGrid();
    }

    void SetGrid()
    {
        grid = new GameObject("grid").transform;        
        for (int i = 0; i < _x; i++)
        {
            for (int j = 0; j < _y; j++)
            {
                spawnThis.name = "Hex " + i + " " + j;
                Vector2 hexpos = HexOffset(i, j);
                Vector3 pos = new Vector3(hexpos.x, hexpos.y, 0);
                hexInstance = Instantiate(spawnThis, pos, Quaternion.identity) as GameObject;
                GameObject.Find("Hex " + i + " " + j + "(Clone)").transform.SetParent(grid);
                
            }
        }
    }

    Vector2 HexOffset(int x, int y)
    {
        Vector2 position = Vector2.zero;

        if (y % 2 == 0)
        {
            position.x = x * offsetX;
            position.y = y * offsetY;
        }
        else
        {
            position.x = (x + 0.5f) * offsetX;
            position.y = y * offsetY;
        }
        return position;
    }

    void FixedUpdate()
    {
        MouseHover();
        if (MapHud.mouseFollower != null && Input.GetKey(KeyCode.Mouse0))
        {
            PlaceHex();
        }
    }
    void MouseHover()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, hexMask))
        {
            //Debug.Log("hit hex mask?" + hit.collider.name);
            hit.collider.gameObject.SendMessage("HoverAnimation");
            hoverHex = hit.collider.name;

            if (MapHud.mouseFollower != null)
            {  
                MapHud.mouseFollower.transform.position = hit.point;
            }
        }
    }
    private void PlaceHex()
    {
        string hexName = MapHud.mouseFollower.name;//"forrestHex(Clone) etc.
        GameObject newHex;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, hexMask))
        {
            string hitName = hit.collider.name;//Hex 0 0(Clone) etc. So when I destroy the hit.collider.object I can still use the name 
            switch (hexName) //switch statement figures out which hex to instantiate based off of name of the mouse follower
            {
                case "forrestHex(Clone)":
                    forrestHex.name = hit.collider.name;
                    newHex = Instantiate(forrestHex, hit.collider.transform.position, hit.collider.transform.rotation) as GameObject;
                    forrestHex.name = "forrestHex";                    
                    break;
                case "grassHex(Clone)":
                    grassHex.name = hit.collider.name;
                    newHex = Instantiate(grassHex, hit.collider.transform.position, hit.collider.transform.rotation)as GameObject;
                    grassHex.name = "grassHex";
                    break;
                case "waterHex(Clone)":
                    waterHex.name = hit.collider.name;
                    newHex = Instantiate(waterHex, hit.collider.transform.position, hit.collider.transform.rotation)as GameObject;
                    waterHex.name = "waterHex";
                    break;
                case "defaultHex(Clone)":
                    defaultHex.name = hit.collider.name;
                    newHex = Instantiate(defaultHex, hit.collider.transform.position, hit.collider.transform.rotation) as GameObject;
                    defaultHex.name = "defaultHex";
                    break;
                default:
                    Debug.Log("Using Default case");
                    break;
            }
            Destroy(hit.collider.gameObject);
            GameObject.Find(hitName + "(Clone)").transform.SetParent(grid);
            GameObject.Find(hitName + "(Clone)").SendMessage("NewHex");
        }


    }
}
