using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexGrid : MonoBehaviour {

    //this got really gross really fast

    public Transform spawnThis;
    public GameObject hexInstance;
    Transform grid;
    //for savegames
    public static List<Hex> hexList;
    public static List<string> hexTypes;
    public static bool loadMap;

    //for generating
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
        _x = MainMenu.mapWidth;
        _y = MainMenu.mapHeight;
        if (_x > 140)
        {
            _x = 140;
        }
        if (_y > 140)
        {
            _y = 140;
        }

        //this is for creating the grid
        float unitLength = ( useAsInnerCircleRadius )? (radius / (Mathf.Sqrt(3)/2)) : radius;
        offsetX = unitLength * Mathf.Sqrt(3);
        offsetY = unitLength * 1.5f;

        //set up raycast for hover effect
        hexMask = LayerMask.GetMask("HexLayer");
        hexList = new List<Hex>();

        //load or start new map
        if (loadMap == false)
        {
            SetGrid();
        }
        else
        {
            LoadGrid();
        }
    }

    void SetGrid()//Create New Map
    {
        grid = new GameObject("grid").transform;        
        for (int i = 0; i < _x; i++)
        {
            for (int j = 0; j < _y; j++)
            {
                SpawnHexes(i, j, "waterHex");
            }
        }
        spawnThis.name = "waterHex";
    }

    private void LoadGrid()//load grid from saved file
    {
        for (int i = 0; i < _x; i++)
        {
            for (int j = 0; j < _y; j++)
            {
                if (hexTypes[0] == "waterHex")
                {
                    spawnThis = waterHex.transform;
                    spawnThis.name = "waterHex";
                }
                else if (hexTypes[0] == "forrestHex")
                {
                    spawnThis = forrestHex.transform;
                    spawnThis.name = "forrestHex";
                }
                else if (hexTypes[0] == "grassHex")
                {
                    spawnThis = grassHex.transform;
                    spawnThis.name = "grassHex";
                }
                else if (hexTypes[0]  == "defaultHex")
                {
                    spawnThis = defaultHex.transform;
                    spawnThis.name = "defaultHex";
                }
                SpawnHexes(i, j, spawnThis.name);
                hexTypes.RemoveAt(0);
            }            
        }
        waterHex.name = "waterHex";
        forrestHex.name = "forrestHex";
        grassHex.name = "grassHex";
        defaultHex.name = "defaultHex";
    }

    private void SpawnHexes(int i, int j, string spawnType)
    {
        GameObject newHex; //for set parent and getting the hex component
        Hex newHexComp;

        //spawn hex with name containing x and y
        spawnThis.name = "Hex " + i + " " + j;
        Vector2 hexpos = HexOffset(i, j);
        Vector3 pos = new Vector3(hexpos.x, hexpos.y, 0);
        hexInstance = Instantiate(spawnThis, pos, Quaternion.identity) as GameObject;
        //set parent
        newHex = GameObject.Find("Hex " + i + " " + j + "(Clone)");
        newHex.transform.SetParent(grid);
        newHex.AddComponent<SphereCollider>();

        //set x and y coordinance
        newHexComp = newHex.GetComponent<Hex>();
        newHexComp.hexX = i;
        newHexComp.hexY = j;
        newHexComp.typeSpawn = spawnType;
        hexList.Add(newHexComp);
    }
    Vector2 HexOffset(int x, int y)//set hex location based on x and y grid coordinates
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

        //quit button is esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    void MouseHover()//if the mouse is over the grid make the hex it's over animate to a different color
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, hexMask))
        {
            //Debug.Log("hit hex mask?" + hit.collider.name);
            hit.collider.gameObject.SendMessage("HoverAnimation");
            hoverHex = hit.collider.name;

            if (MapHud.mouseFollower != null)//set the mousefollower (the hex you have selected from the hud) to the mouse location
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
            //Hex hitHex = hit.collider.gameObject.GetComponent<Hex>();
            string hitName = hit.collider.name;//Hex 0 0(Clone) etc. So when I destroy the hit.collider.object I can still use the name 
            if (hit.collider.tag + "(Clone)" != MapHud.mouseFollower.name)
            {
               //Debug.Log("hit collider tag doesn't equal mousefollower name");
                switch (hexName) //switch statement figures out which hex to instantiate based off of name of the mouse follower
                {
                    case "forrestHex(Clone)":

                        if (Camera.main.orthographicSize >= 20)//if zoomed out far then fill in multiple hexes
                        {
                            MassInstantiate(hit.collider.gameObject, forrestHex);
                        }
                        //replace the hex that the raycast hit and name it properly
                        forrestHex.name = hitName;
                        if (forrestHex.name.Contains("(Clone)"))//remove clone from the name so it doesn't turn in to (Clone)(Clone)(Clone) etc.
                        {
                            forrestHex.name = forrestHex.name.Replace("(Clone)", "");
                        }
                        newHex = Instantiate(forrestHex, hit.collider.transform.position, Quaternion.identity) as GameObject;                        
                        forrestHex.name = "forrestHex";
                        SetUpHex(newHex, hit.collider.gameObject, forrestHex.name);
                        break;
                    case "grassHex(Clone)":
                        if (Camera.main.orthographicSize >= 17)//if zoomed out
                        {
                            MassInstantiate(hit.collider.gameObject, grassHex);
                        }
                        //replace the hex that the raycast hit and name it properly
                        grassHex.name = hitName;
                        if (grassHex.name.Contains("(Clone)"))//remove clone from the name so it doesn't turn in to (Clone)(Clone)(Clone) etc.
                        {
                            grassHex.name = grassHex.name.Replace("(Clone)", "");
                        }
                        newHex = Instantiate(grassHex, hit.collider.transform.position, Quaternion.identity) as GameObject;                        
                        grassHex.name = "grassHex";
                        SetUpHex(newHex, hit.collider.gameObject, grassHex.name);
                        break;
                    case "waterHex(Clone)":
                        if (Camera.main.orthographicSize >= 17)//if zoomed out
                        {
                            MassInstantiate(hit.collider.gameObject, waterHex);
                        }
                        //replace the hex that the raycast hit and name it properly
                        waterHex.name = hitName;
                        if (waterHex.name.Contains("(Clone)"))//remove clone from the name so it doesn't turn in to (Clone)(Clone)(Clone) etc.
                        {
                            waterHex.name = waterHex.name.Replace("(Clone)", "");
                        }
                        newHex = Instantiate(waterHex, hit.collider.transform.position, Quaternion.identity) as GameObject;                        
                        waterHex.name = "waterHex";
                        SetUpHex(newHex, hit.collider.gameObject, waterHex.name);
                        break;
                    case "defaultHex(Clone)":
                        if (Camera.main.orthographicSize >= 17)//if zoomed out
                        {
                            MassInstantiate(hit.collider.gameObject, defaultHex);
                        }
                        //replace the hex that the raycast hit and name it properly
                        defaultHex.name = hitName;
                        if (defaultHex.name.Contains("(Clone)"))//remove clone from the name so it doesn't turn in to (Clone)(Clone)(Clone) etc.
                        {
                            defaultHex.name = defaultHex.name.Replace("(Clone)", "");
                        }
                        //replace the hex that the raycast hit and name it properly
                        newHex = Instantiate(defaultHex, hit.collider.transform.position, Quaternion.identity) as GameObject;                        
                        defaultHex.name = "defaultHex";
                        SetUpHex(newHex, hit.collider.gameObject, defaultHex.name);
                        break;
                    default:
                        Debug.Log("Using Default case");
                        forrestHex.name = "forrestHex";
                        grassHex.name = "grassHex";
                        waterHex.name = "waterHex";
                        defaultHex.name = "defaultHex";
                        break;
                }
                //need to transfer hexX and Y from hit.collider to new Hex
                Destroy(hit.collider.gameObject);
            }            
        }
        placing = false;
    }

    private void MassInstantiate(GameObject replace, GameObject toInstantiate)
    {
        Hex hex = replace.GetComponent<Hex>();
        //Debug.Log(replace.GetComponent<Hex>().hexX + " " + replace.GetComponent<Hex>().hexY);
        GameObject replace0, replace1, replace2, replace3, replace4;
        GameObject newHex;
        string hexType = toInstantiate.name;
        // set hexes near the hit hex to be replaced
        replace0 = GameObject.Find("Hex " + hex.hexX + " " + (hex.hexY +1) + "(Clone)");
        replace1 = GameObject.Find("Hex " + (hex.hexX - 1) + " " + hex.hexY + "(Clone)");
        replace2 = GameObject.Find("Hex " + (hex.hexX + 1) + " " + (hex.hexY + 1) + "(Clone)");
        replace3 = GameObject.Find("Hex " + (hex.hexX - 1) + " " + (hex.hexY + 1) + "(Clone)");
        replace4 = GameObject.Find("Hex " + hex.hexX + " " + (hex.hexY - 1) + "(Clone)");

        //if the hexes set to be replaced are not null replace them and call hex set up to give them a sphere collider etc.
        if (replace0 != null)
        {
            toInstantiate.name = "Hex " + hex.hexX + " " + (hex.hexY + 1);
            newHex = Instantiate(toInstantiate, replace0.transform.position, Quaternion.identity) as GameObject;
            SetUpHex(newHex, replace0, hexType);
        }

        if (replace1 != null)
        {
            toInstantiate.name = "Hex " + (hex.hexX - 1) + " " + hex.hexY;
            newHex = Instantiate(toInstantiate, replace1.transform.position,Quaternion.identity) as GameObject;
            SetUpHex(newHex, replace1, hexType);
        }

        if (replace2 != null)
        {
            toInstantiate.name = "Hex " + (hex.hexX + 1) + " " + (hex.hexY + 1);
            newHex = Instantiate(toInstantiate, replace2.transform.position, Quaternion.identity) as GameObject;
            SetUpHex(newHex, replace2, hexType);
        }
        if (replace3 != null)
        {
            toInstantiate.name = "Hex " + (hex.hexX - 1) + " " + (hex.hexY + 1);
            newHex = Instantiate(toInstantiate, replace3.transform.position, Quaternion.identity) as GameObject;
            SetUpHex(newHex, replace3, hexType);
        }
        if (replace4 != null)
        {
            toInstantiate.name = "Hex " + (hex.hexX - 1) + " " + (hex.hexY - 1);
            newHex = Instantiate(toInstantiate, replace4.transform.position, Quaternion.identity) as GameObject;
            SetUpHex(newHex, replace4, hexType);
        }

        //Destroy original hex for the location
        Destroy(replace0);
        Destroy(replace1);
        Destroy(replace2);
        Destroy(replace3);
        Destroy(replace4);
        
    }

    void SetUpHex(GameObject newHex, GameObject hit, string type)
    {
        //gives new hexes a sphere collider and the original hexes hex coordinates and parents it to the gird
        Hex hex = newHex.GetComponent<Hex>();
        Hex hexHit = hit.GetComponent<Hex>();
        //for saving etc.
        hex.hexX = hexHit.hexX;
        hex.hexY = hexHit.hexY;
        hex.typeSpawn = type;
        //gameobject settings
        newHex.transform.SetParent(grid);
        newHex.AddComponent<SphereCollider>();
        //place in list
        int index = hexList.IndexOf(hexHit);
        hexList.Remove(hexHit);
        Destroy(hexHit);
        hexList.Insert(index, hex);

    }
}
