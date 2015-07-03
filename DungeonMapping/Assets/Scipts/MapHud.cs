using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapHud : MonoBehaviour 
{

    Text mapDimentions;

    void Start()
    {
        mapDimentions = GetComponentInChildren<Text>();
        
        mapDimentions.text = "width: " + MainMenu.mapWidth + " height: " + MainMenu.mapWidth;
    }
}
