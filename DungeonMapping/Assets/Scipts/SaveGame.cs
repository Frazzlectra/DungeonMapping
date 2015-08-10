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
    public static bool saving;

    public static string mapName;


    void Start()
    {
        Canvas MenuCanvas = FindObjectOfType<Canvas>();
        if (Application.loadedLevel == 1)
        {
            if (saving == false)
            {
                saveBtn.onClick.AddListener(() => { SaveNow(); });
            }
        }
        if (Application.loadedLevel == 0)
        {
            loadBtn.onClick.AddListener(() => { Load(); });

            if (File.Exists(Application.persistentDataPath + "/" + "SavedMaps"))//if there are saved maps give them to the main menu to see.
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fileSaves = File.Open(Application.persistentDataPath + "/" + "SavedMaps", FileMode.Open);
                AllSavedMaps maps = (AllSavedMaps)bf.Deserialize(fileSaves);
                foreach (string map in maps.savedMaps)
                {
                    if (!MainMenu.loadableMaps.Contains(map))
                    {
                        MainMenu.loadableMaps.Add(map);
                    }
                }
                bf.Serialize(fileSaves, maps);
                fileSaves.Close();

                //set up in main menu
                MenuCanvas.SendMessage("ShowSavedMaps");
            }
        }
    }

    public void SaveNow()
    {
        saving = true;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + mapName);

        MapData data = new MapData();

        HexGrid.hexList.Sort();

        Hex last = HexGrid.hexList[HexGrid.hexList.Count - 1];
        data.mapHeight = MainMenu.mapHeight;
        data.mapWidth = MainMenu.mapWidth;
        foreach (Hex hex in HexGrid.hexList)
        {
            data.types.Add(hex.typeSpawn);
        }
        bf.Serialize(file, data);
        file.Close();

        //make a list of saved files so they can be written on the start menu...
        if (!File.Exists(Application.persistentDataPath + "/" + "SavedMaps"))
        {
            FileStream fileSaves = File.Create(Application.persistentDataPath + "/" + "SavedMaps");
            AllSavedMaps maps = new AllSavedMaps();
            maps.savedMaps.Add(mapName);
            bf.Serialize(fileSaves, maps);
            fileSaves.Close();
        }
        else
        {
            List<string> tempList = new List<string>();
            FileStream fileSaves = File.Open(Application.persistentDataPath + "/" + "SavedMaps", FileMode.Open);
            AllSavedMaps maps = (AllSavedMaps)bf.Deserialize(fileSaves);
            //AllSavedMaps maps = (AllSavedMaps)bf.Deserialize(fileSaves);
            if (!maps.savedMaps.Contains(mapName))
            {

                foreach (string map in maps.savedMaps)
                {
                    tempList.Add(map);
                }
                tempList.Add(mapName);
                //foreach (string map in tempList)
                //{
                    //maps.savedMaps.Add(mapName);
                //}
                bf.Serialize(fileSaves, maps);
                fileSaves.Close();
                File.Delete(Application.persistentDataPath + "/" + "SavedMaps");
                FileStream newFileSaves = File.Create(Application.persistentDataPath + "/" + "SavedMaps");
                AllSavedMaps newMaps = new AllSavedMaps();
                foreach (string map in tempList)
                {
                    newMaps.savedMaps.Add(map);
                }
                bf.Serialize(newFileSaves, newMaps);
                newFileSaves.Close();
            } 
            else
            {
                Debug.Log("that map already exists overwriting");
                bf.Serialize(fileSaves, maps);
                fileSaves.Close();
            }
            saving = false;
        }
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
[Serializable]
class AllSavedMaps
{
    public List<string> savedMaps = new List<string>();
}