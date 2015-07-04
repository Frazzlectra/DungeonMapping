using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapHud : MonoBehaviour 
{

    Text mapDimentions;

    //hex buttons for selecting...
    public Button forrestHex;
    public Button grassHex;
    public Button waterHex;
    public Button defaultHex;

    //sprites to follow mouse so you know which hex you have selected
    public GameObject forrestSpr;
    public GameObject grassSpr;
    public GameObject waterSpr;
    public GameObject defaultSpr;

    public static GameObject mouseFollower; 

    void Start()
    {        
        //shows the size of the map in hexes
        mapDimentions = GetComponentInChildren<Text>();        
        mapDimentions.text = "width: " + MainMenu.mapWidth + " height: " + MainMenu.mapWidth;
        
        //set up buttons
        forrestHex.onClick.AddListener(() => { ButtonClicked("forrestHex"); });
        grassHex.onClick.AddListener(() => { ButtonClicked("grassHex"); });
        waterHex.onClick.AddListener(() => { ButtonClicked("waterHex"); });
        defaultHex.onClick.AddListener(() => { ButtonClicked("defaultHex"); });
    }

    private void ButtonClicked(string btn)
    {
        if (mouseFollower != null)//destroy sprite attacked to mouse
        {
            Destroy(mouseFollower);
        }
        switch (btn)//create new sprite to follow mouse based on the hex clicked
        {
            case "forrestHex":
                mouseFollower = Instantiate(forrestSpr, Input.mousePosition, Quaternion.identity) as GameObject;
                break;
            case "grassHex":
                mouseFollower = Instantiate(grassSpr, Input.mousePosition, Quaternion.identity) as GameObject;
                break;
            case "waterHex":
                mouseFollower = Instantiate(waterSpr, Input.mousePosition, Quaternion.identity) as GameObject;
                break;
            case"defaultHex":
                mouseFollower = Instantiate(defaultSpr, Input.mousePosition, Quaternion.identity) as GameObject;
                break;
            default:
                Debug.Log("error in map hud");
                break;
        }
    }

}
