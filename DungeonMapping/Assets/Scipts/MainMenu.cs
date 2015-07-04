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


    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            if (mapHeightTxt.text != "" && mapWidthTxt.text != "")
            {
                string sTemp = mapWidthTxt.text;
                mapWidth = int.Parse(sTemp);
                sTemp = mapHeightTxt.text;
                mapHeight = int.Parse(sTemp);
                Application.LoadLevel(1);
            }
        }
    }


}
