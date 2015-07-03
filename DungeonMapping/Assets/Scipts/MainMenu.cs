using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public static int mapWidth = 0;
    public static int mapHeight = 0;

    InputField mapWidthInp;
    InputField mapHeightInp;

    public Text mapWidthTxt;
    public Text mapHeightTxt;

    void Start()
    {
        //Getting Input Field and Text
        GameObject mapWidthGO = GameObject.Find("mapWidth");
        GameObject mapHeightGO = GameObject.Find("mapHeight");

        mapHeightInp = mapHeightGO.GetComponent<InputField>();
        mapWidthInp = mapWidthGO.GetComponent<InputField>();

        //mapWidthTxt = mapWidthInp.GetComponent<Text>();
        //mapHeightTxt = mapHeightInp.GetComponent<Text>();

        //AddListener To Input
        mapWidthInp.onValueChange.AddListener( delegate { setUpMap("width"); });
        mapHeightInp.onValueChange.AddListener(delegate { setUpMap("height"); });

        //Validating input
        mapWidthInp.onValidateInput += delegate(string input, int charIndex, char addedChar) { return MyValidation(input, addedChar, "width"); };
        mapHeightInp.onValidateInput += delegate(string input, int charIndex, char addedChar) { return MyValidation(input, addedChar, "height"); };
    }

    private char MyValidation(string input, char charToValidate, string dimension)
    {        
        if (char.IsDigit(charToValidate))
        {            
            Debug.Log("Is Digit wooh");
            //this section is setting the map with to a public variable I can access while generating the grid...
            switch (dimension)
            {
                case "width":
                    mapWidth = int.Parse(mapWidthTxt.text);
                    break;
                case "height":
                    mapHeight = int.Parse(mapHeightTxt.text);
                    break;
                default:
                    Debug.Log("neither height nore width were changed...");
                    break;
            }
        }
        else
        {
            charToValidate = '\0';
        }
        return charToValidate;
    }

    private void setUpMap(string inp)
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            if (mapHeightTxt.text != "" && mapWidthTxt.text != "")
            {
                Debug.Log("Map Width : " + mapWidth + "Map Height " + mapHeight +"Can SUBMIT");
            }            
        }
    }


}
