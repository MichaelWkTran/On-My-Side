using System;
using System.IO;
using UnityEngine;


//Note that this system is only loading and saving data to m_data and does not interact with external information
public static class SaveSystem
{
    public const string m_fileName = "/Save.json";//"Save.bb";
    public const uint m_dataSize = 3U; public enum SaveIDs
    {
        GameManager     = 0,
        Settings        = 1,
        Levels          = 2,
    };
    [Serializable] public class SaveData
    {
        public string[] m_data;
        public SaveData(string[] _data) { m_data = _data; }   
    }

    static SaveData m_data; public static SaveData m_Data
    {
        get
        {
            //Initilize and load data if none currently exists
            if (m_data == null) { m_data = new SaveData(new string[m_dataSize]); Load(); }

            return m_data;
        }
    }


    //Saves the data from m_data to the file, Application.persistentDataPath + m_fileName
    public static void Save()
    {
        if (m_data == null) return;
        
        //Get the file directory of the save data
        string path = Application.persistentDataPath + m_fileName;
        
        //Load from Json
        string saveDataJson = JsonUtility.ToJson(m_data);
        File.WriteAllText(path, saveDataJson);
    }

    //Loads the data stored in Application.persistentDataPath + m_fileName to m_data
    public static void Load()
    {
        //Ensure that the data is initialized before loading
        if (m_data == null) { m_data = new SaveData(new string[m_dataSize]); }

        //Get the file directory of the save data
        string path = Application.persistentDataPath + m_fileName;
        
        //Check whether the file in the searched path exists, if not then exit the function
        if (!File.Exists(path)) { Debug.LogError("Save file not found in " + path); return; }
        
        //Load to Json
        string saveDataJson = File.ReadAllText(path);
        m_data = JsonUtility.FromJson<SaveData>(saveDataJson);
    }
}