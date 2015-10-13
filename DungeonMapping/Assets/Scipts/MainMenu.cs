using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public static int mapWidth = 0;
    public static int mapHeight = 0;

    public Canvas menuGO;

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

    //for gennerating list of buttons of saved maps...
    public Button instantiateButton;
    public GameObject buttonPanel;
    

    void Start()
    {
        menuGO = GameObject.FindObjectOfType<Canvas>();
        //Getting Input Field and Text
        GameObject mapWidthGO = GameObject.Find("mapWidth");
        GameObject mapHeightGO = GameObject.Find("mapHeight");

        mapHeightInp = mapHeightGO.GetComponent<InputField>();
        mapWidthInp = mapWidthGO.GetComponent<InputField>();

        generate.onClick.AddListener(() => { ButtonClicked("generate", ""); });
        loadMap.onClick.AddListener(() => { ButtonClicked("load", ""); });
        exit.onClick.AddListener(() => { ButtonClicked("exit", ""); });        
        
        //mapWidthTxt = mapWidthInp.GetComponent<Text>();
        //mapHeightTxt = mapHeightInp.GetComponent<Text>();
    }

    private void ButtonClicked(string btn, string name)
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
            case"savedMap":
                mapNameInp.text = name;
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

    public void ShowSavedMaps()//Sets up buttons with previous maps to load
    {
        int newButHeight = 147;
        SavedMapsTxt.text = "Saved Maps";
        if (loadableMaps.Count > 0)//I set this up so it gets the list of saved maps from SaveGame
        {
            foreach (string map in loadableMaps)
            {
                Vector3 newPosition = new Vector3(100, newButHeight, 0);
                Button newButton = Instantiate(instantiateButton) as Button;
                newButton.transform.SetParent(buttonPanel.transform, false);
                newButton.transform.localPosition = newPosition;                
                newButton.name = map;
                Text buttonText = newButton.GetComponentInChildren<Text>();
                buttonText.text = newButton.name;
                newButHeight -= 30;// possition of button in list 

                newButton.onClick.AddListener(() => {ButtonClicked("savedMap", newButton.name);});
            }
        }
    }

}
