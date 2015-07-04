using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexGrid : MonoBehaviour {

    //this got really gross really fast

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

    //so it only tries to place one at a time...?
    bool placing = false;

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
        GameObject newHex; //for set parent and getting the hex component
        Hex newHexComp;
        grid = new GameObject("grid").transform;        
        for (int i = 0; i < _x; i++)
        {
            for (int j = 0; j < _y; j++)
            {
                //spawn hex with name containing x and y
                spawnThis.name = "Hex " + i + " " + j;
                Vector2 hexpos = HexOffset(i, j);
                Vector3 pos = new Vector3(hexpos.x, hexpos.y, 0);
                hexInstance = Instantiate(spawnThis, pos, Quaternion.identity) as GameObject;
                //set parent
                newHex = GameObject.Find("Hex " + i + " " + j + "(Clone)");
                newHex.transform.SetParent(grid);

                //set x and y coordinance
                newHexComp = newHex.GetComponent<Hex>();
                newHexComp.hexX = i;
                newHexComp.hexY = j;
                
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
        if (MapHud.mouseFollower != null && Input.GetKey(KeyCode.Mouse0) && !placing)
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
        placing = true;
        string hexName = MapHud.mouseFollower.name;//"forrestHex(Clone) etc.
        GameObject newHex;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, hexMask))
        {
            Hex hitHex = hit.collider.gameObject.GetComponent<Hex>();
            string hitName = hit.collider.name;//Hex 0 0(Clone) etc. So when I destroy the hit.collider.object I can still use the name 
            if (hit.collider.tag + "(Clone)" != MapHud.mouseFollower.name)
            {
               //Debug.Log("hit collider tag doesn't equal mousefollower name");
                switch (hexName) //switch statement figures out which hex to instantiate based off of name of the mouse follower
                {
                    case "forrestHex(Clone)":

                        if (Camera.main.orthographicSize >= 19)
                        {
                            MassInstantiate(hit.collider.gameObject, forrestHex);
                        }
                        forrestHex.name = hitName;
                        if(forrestHex.name.Contains("(Clone)"))
                        {
                            //Debug.Log(forrestHex.name);
                            forrestHex.name = forrestHex.name.Replace("(Clone)", "");
                            //Debug.Log(forrestHex.name);
                        }
                        //else
                        //{
                            newHex = Instantiate(forrestHex, hit.collider.transform.position, Quaternion.identity) as GameObject;
                        //}
                        forrestHex.name = "forrestHex";
                        break;
                    case "grassHex(Clone)":

                        grassHex.name = hitName;
                        Debug.Log("hit name" + hitName);
                        if (grassHex.name.Contains("(Clone)"))
                        {
                            grassHex.name = grassHex.name.Replace("(Clone)", "");
                        }
                        Debug.Log("grass name  " + grassHex.name);
                        newHex = Instantiate(grassHex, hit.collider.transform.position, Quaternion.identity) as GameObject;
                        grassHex.name = "grassHex";
                        break;
                    case "waterHex(Clone)":
                        waterHex.name = hitName;
                        if (waterHex.name.Contains("(Clone)"))
                        {
                            waterHex.name = waterHex.name.Replace("(Clone)", "");
                        }
                        newHex = Instantiate(waterHex, hit.collider.transform.position, Quaternion.identity) as GameObject;
                        waterHex.name = "waterHex";
                        break;
                    case "defaultHex(Clone)":
                        defaultHex.name = hitName;
                        if (defaultHex.name.Contains("(Clone)"))
                        {
                            defaultHex.name = defaultHex.name.Replace("(Clone)", "");
                        }
                        newHex = Instantiate(defaultHex, hit.collider.transform.position, Quaternion.identity) as GameObject;
                        defaultHex.name = "defaultHex";
                        break;
                    default:
                        Debug.Log("Using Default case");
                        forrestHex.name = "forrestHex";
                        break;
                }
                //need to transfer hexX and Y from hit.collider to new Hex
                Destroy(hit.collider.gameObject);
                GameObject.Find(hitName).transform.SetParent(grid);
                GameObject.Find(hitName).AddComponent<SphereCollider>();
            }            
        }
        placing = false;
    }

    private void MassInstantiate(GameObject replace, GameObject toInstantiate)
    {
        Hex hex = replace.GetComponent<Hex>();
        //Debug.Log(replace.GetComponent<Hex>().hexX + " " + replace.GetComponent<Hex>().hexY);
        GameObject replace0, replace1, replace2;
        GameObject newHex;

        replace0 = GameObject.Find("Hex " + hex.hexX + " " + (hex.hexY +1) + "(Clone)");
        replace1 = GameObject.Find("Hex " + (hex.hexX - 1) + " " + hex.hexY + "(Clone)");
        replace2 = GameObject.Find("Hex " + (hex.hexX + 1) + " " + (hex.hexY + 1) + "(Clone)");

        if (replace0 != null)
        {
            Debug.Log("replace0 not null " + "Hex " + hex.hexX + " " + (hex.hexY + 1));
            toInstantiate.name = "Hex " + hex.hexX + " " + (hex.hexY + 1);
            newHex = Instantiate(toInstantiate, replace0.transform.position, Quaternion.identity) as GameObject;
            newHex.transform.SetParent(grid);
            newHex.AddComponent<SphereCollider>();
        }

        if (replace1 != null)
        {
            Debug.Log("replace1 not null" + "Hex " + (hex.hexX - 1) + " " + hex.hexY);
            toInstantiate.name = "Hex " + (hex.hexX - 1) + " " + hex.hexY;
            newHex = Instantiate(toInstantiate, replace1.transform.position,Quaternion.identity) as GameObject;
            newHex.transform.SetParent(grid);
            newHex.AddComponent<SphereCollider>();
        }

        if (replace2 != null)
        {
            Debug.Log("replace2  not null " + "Hex " + (hex.hexX + 1) + " " + (hex.hexY + 1));
            toInstantiate.name = "Hex " + (hex.hexX + 1) + " " + (hex.hexY + 1);
            newHex = Instantiate(toInstantiate, replace2.transform.position, Quaternion.identity) as GameObject;
            newHex.transform.SetParent(grid);
            newHex.AddComponent<SphereCollider>();
        }

        Destroy(replace0);
        Destroy(replace1);
        Destroy(replace2);
        
    }
}
