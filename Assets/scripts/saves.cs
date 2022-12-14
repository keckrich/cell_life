using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class saves : MonoBehaviour
{

    public Settings settingsValues;
    public string fileName = "save";

    Dictionary<int, string> saveFiles = new Dictionary<int, string>();

    public TMP_Dropdown dropdown;
    public TMP_InputField inputField;
  

   





    // Start is called before the first frame update
    void Start()
    {
        LoadSaveName();
        AddSaveFilesToDropdown();

        dropdown.onValueChanged.AddListener(o => this.fileName = dropdown.captionText.text);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // save settings as json to file
    public void SaveSettings(string name)
    {
        string json = JsonUtility.ToJson(settingsValues);
        System.IO.File.WriteAllText(Application.dataPath + "/saves/" + name + ".json", json);

        // // update the count of filename
        // string[] split = this.fileName.Split('_');
        // int count = 0;

        // if (int.TryParse(split[split.Length - 1], out count))
        // {
        //     split[split.Length - 1] = (count + 1).ToString();
        //     this.fileName = string.Join("_", split);
        // }
        // else
        // {
        //     this.fileName += "_1";
        // }

        LoadSaveName();
        AddSaveFilesToDropdown();
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
            JsonUtility.FromJsonOverwrite(json, settingsValues);

            settingsValues.Reset();

            //TODO: change the name field to the name of the file
            // update the file name and imput field to the new file name
             this.fileName = name;
             this.inputField.text = name;

        } catch (System.Exception e) {
            Debug.Log(e);
        }
    }

    public void LoadSettings()
    {
        saveFiles.Clear();
        LoadSettings(this.fileName);
    }
    // load all the save files
    void LoadSaveName(){
        saveFiles.Clear();

        // load all save files
        // string[] files = System.IO.Directory.GetFiles(Application.dataPath + "/saves/");
        string[] files = Directory.GetFiles(Application.dataPath + "/saves/", "*.json");
        saveFiles.Add(0, "");

        // add all save files to dictionary
        foreach (string file in files)
        {
            saveFiles.Add(saveFiles.Count, file);
        }
    }

    // add the save files to the dropdown
    public void AddSaveFilesToDropdown(){
        // clear the dropdown
        dropdown.ClearOptions();
        // saveFiles.Clear();

        // add all save files to dropdown
        foreach (KeyValuePair<int, string> file in saveFiles)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(file.Value.Split('/')[file.Value.Split('/').Length - 1].Split('.')[0]));
        }
        
        dropdown.RefreshShownValue();

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
