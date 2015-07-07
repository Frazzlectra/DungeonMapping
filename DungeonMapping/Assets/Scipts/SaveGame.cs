using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class SaveGame : MonoBehaviour {

    public Button saveBtn;
    public Button loadBtn;

    public static string mapName;


    void Start()
    {
        if (Application.loadedLevel == 1)
        {
            saveBtn.onClick.AddListener(() => { SaveNow(); });
        }
        if (Application.loadedLevel == 0)
        {
            loadBtn.onClick.AddListener(() => { Load(); });
            //Debug.Log("level loaded");
        }
    }

    public void SaveNow()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + mapName);

        MapData data = new MapData();

        HexGrid.hexList.Sort();

        //foreach (Hex _hex in HexGrid.hexList)
        //{
        //    Debug.Log(_hex.name);
        //}
        Hex last = HexGrid.hexList[HexGrid.hexList.Count - 1];

        data.mapHeight = MainMenu.mapHeight;
        data.mapWidth = MainMenu.mapWidth;
        foreach (Hex hex in HexGrid.hexList)
        {
            data.types.Add(hex.typeSpawn);
        }
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {        
        if (File.Exists(Application.persistentDataPath + "/" + mapName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + mapName, FileMode.Open);
            MapData data = (MapData)bf.Deserialize(file);
            file.Close();

            MainMenu.mapWidth = data.mapWidth;
            MainMenu.mapHeight = data.mapHeight;
            HexGrid.hexTypes = data.types;
            Debug.Log("Load Map");
            HexGrid.loadMap = true;
            Application.LoadLevel(1);
        }
        else
        {
            Debug.Log("failed to find file");
        }
    }
}
[Serializable]
class MapData
{
    public int mapHeight;
    public int mapWidth;
    public List<string> types = new List<string>();
}