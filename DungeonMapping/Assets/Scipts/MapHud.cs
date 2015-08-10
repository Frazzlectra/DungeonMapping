using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapHud : MonoBehaviour 
{

    Text mapDimentions;
    //quit button
    public Button quitToMenu;
    //hex buttons for selecting...
    public Button forrestHex;
    public Button grassHex;
    public Button waterHex;
    public Button iceHex;
    public Button desertHex;
    public Button mountainHex;

    //sprites to follow mouse so you know which hex you have selected
    public GameObject forrestSpr;
    public GameObject grassSpr;
    public GameObject waterSpr;
    public GameObject iceSpr;
    public GameObject desertSpr;
    public GameObject mountainSpr;

    public static GameObject mouseFollower; 

    void Start()
    {        
        //shows the size of the map in hexes
        mapDimentions = GetComponentInChildren<Text>();        
        mapDimentions.text = "width: " + MainMenu.mapWidth + " height: " + MainMenu.mapHeight;
        
        //set up buttons
        forrestHex.onClick.AddListener(() => { ButtonClicked("forrestHex"); });
        grassHex.onClick.AddListener(() => { ButtonClicked("grassHex"); });
        waterHex.onClick.AddListener(() => { ButtonClicked("waterHex"); });
        iceHex.onClick.AddListener(() => { ButtonClicked("iceHex"); });
        desertHex.onClick.AddListener(() => { ButtonClicked("desertHex"); });
        mountainHex.onClick.AddListener(() => { ButtonClicked("mountainHex"); });

        //quit button
        quitToMenu.onClick.AddListener(() => { ButtonClicked("quit"); });
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
            case"iceHex":
                mouseFollower = Instantiate(iceSpr, Input.mousePosition, Quaternion.identity) as GameObject;
                break;
            case "desertHex":
                mouseFollower = Instantiate(desertSpr, Input.mousePosition, Quaternion.identity) as GameObject;
                break;
            case "mountainHex":
                mouseFollower = Instantiate(mountainSpr, Input.mousePosition, Quaternion.identity) as GameObject;
                break;
            case "quit":
                Application.LoadLevel(0);
                break;
            default:
                Debug.Log("error in map hud");
                break;
        }
    }

}
