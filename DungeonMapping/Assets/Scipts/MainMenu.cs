using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public static int mapWidth = 0;
    public static int mapHeight = 0;

    public InputField mapWidthInp;
    public InputField mapHeightInp;
    public InputField mapNameInp;

    public Button generate;
    public Button loadMap;
    public Button exit;

    public Text mapWidthTxt;
    public Text mapHeightTxt;
    public Text SavedMapsTxt;

    public static List<string> loadableMaps = new List<string>();

    bool generateMap;

    void Start()
    {
        //Getting Input Field and Text
        GameObject mapWidthGO = GameObject.Find("mapWidth");
        GameObject mapHeightGO = GameObject.Find("mapHeight");

        mapHeightInp = mapHeightGO.GetComponent<InputField>();
        mapWidthInp = mapWidthGO.GetComponent<InputField>();

        generate.onClick.AddListener(() => { ButtonClicked("generate"); });
        loadMap.onClick.AddListener(() => { ButtonClicked("load"); });
        exit.onClick.AddListener(() => { ButtonClicked("exit"); });        
        
        //mapWidthTxt = mapWidthInp.GetComponent<Text>();
        //mapHeightTxt = mapHeightInp.GetComponent<Text>();
    }

    private void ButtonClicked(string btn)
    {
        switch (btn)
        {
            case "generate":
                generateMap = true;
                break;
            case"load":
                if (mapNameInp.text != "")
                {
                    SaveGame.mapName = mapNameInp.text;
                }
                else
                {
                    Debug.Log("Pls enter map name");
                }
                //create buttons for all the saved games
                break;
            case "exit":
                Application.Quit();
                break;
            default:
                break;
        }
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter) || generateMap)
        {
            if (mapHeightTxt.text != "" && mapWidthTxt.text != "" && mapNameInp.text != "")
            {
                string sTemp = mapWidthTxt.text;
                mapWidth = int.Parse(sTemp);
                sTemp = mapHeightTxt.text;
                mapHeight = int.Parse(sTemp);
                SaveGame.mapName = mapNameInp.text;
                Application.LoadLevel(1);
            }
            else
            {
                generateMap = false;
            }
        }
    }

    public void ShowSavedMaps()
    {
        SavedMapsTxt.text = "Saved Maps";
        if (loadableMaps.Count > 0)//need to set this up so it gets the list of saved maps from SaveGame
        {
            foreach (string map in loadableMaps)
            {
                SavedMapsTxt.text += "\n" + map;
                Debug.Log("map set " + map );
            }
        }
    }

}
