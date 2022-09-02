using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class saves : MonoBehaviour
{

    public Settings settingsValues;
    public string fileName = "save";

    Dictionary<int, string> saveFiles = new Dictionary<int, string>();




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // save settings as json to file
    public void SaveSettings(string name)
    {
        string json = JsonUtility.ToJson(this);
        System.IO.File.WriteAllText(Application.dataPath + "/saves/" + name + ".json", json);

        // update the count of filename
        string[] split = this.fileName.Split('_');
        int count = 0;

        if (int.TryParse(split[split.Length - 1], out count))
        {
            split[split.Length - 1] = (count + 1).ToString();
            this.fileName = string.Join("_", split);
        }
        else
        {
            this.fileName += "_1";
        }
    }

    public void SaveSettings()
    {
        SaveSettings(this.fileName);
    }

    // load settings from json file
    public void LoadSettings(string name)
    {
        try {
            string json = System.IO.File.ReadAllText(Application.dataPath + "/saves/" + name + ".json");
            JsonUtility.FromJsonOverwrite(json, this);
        } catch (System.Exception e) {
            Debug.Log(e);
        }
    }

    void LoadSaveName(){
        // load all save files
        // string[] files = System.IO.Directory.GetFiles(Application.dataPath + "/saves/");
        string[] files = Directory.GetFiles(Application.dataPath + "/saves/", "*.json");

        // add all save files to dictionary
        foreach (string file in files)
        {
            string[] split = file.Split('/');
            string[] split2 = split[split.Length - 1].Split('.');
            int count = 0;

            if (int.TryParse(split2[0], out count))
            {
                saveFiles.Add(count, split2[0]);
            }
        }
    }

    #region Getters and Setters
    public string getFileName()
    {
        return this.fileName;
    }

    public void setFileName(string fileName)
    {
        this.fileName = fileName;
    }
    #endregion
}
