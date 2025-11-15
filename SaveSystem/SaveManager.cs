using System;
using System.Text.Json;
using System.IO;
public class SaveManager
{
    public SaveData sd = new SaveData();
    public FileSettings fs = new FileSettings();
    string savePath = "C:\\Users\\" + Environment.UserName + "\\Documents\\Save Data\\SaveFile.json";
    string settingPath = "C:\\Users\\" + Environment.UserName + "\\Documents\\Save Data\\GameSettings.json";
    public bool filesFreshlyCreated = false;
    public static SaveManager instance { get; private set; }
    /// <summary>
    /// Checks if a save manager is already instantiated
    /// </summary>
    public SaveManager()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            throw new Exception("Instance already exists");
        }
    }

    /// <summary>
    /// Creates a "New Game" Save
    /// </summary>
    public void NewGame()
    {
        sd = new SaveData();
        Save();
    }

    /// <summary>
    /// If there's already a JSON Save, extract the data, or else create one and give it "Clear Save" data 
    /// </summary>
    public void LoadData()
    {
        if (File.Exists(savePath))
        {
            sd = JsonSerializer.Deserialize<SaveData>(File.ReadAllText(savePath))!;
            fs = JsonSerializer.Deserialize<FileSettings>(File.ReadAllText(settingPath))!;
        }
        else
        {
            var direct = Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            var myFile = File.Create(savePath);
            var myFile2 = File.Create(settingPath);
            myFile.Close();
            myFile2.Close();
            sd = new SaveData();
            fs = new FileSettings();
            filesFreshlyCreated = true;
            Save();
        }
    }

    /// <summary>
    /// Saves the data to the JSON file
    /// </summary>
    public void Save()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        File.WriteAllText(savePath, JsonSerializer.Serialize(sd, options)!);
        File.WriteAllText(settingPath, JsonSerializer.Serialize(fs, options)!);
    }

    /// <summary>
    /// Sets and saves a "Fresh Save File"
    /// </summary>
    /// </summary>
    public void Clear()
    {
        File.Delete(savePath);
        LoadData();
    }

    public bool AchievementCheck(bool temp)
    {
        if (!temp)
        {
            temp = true;
        }
        else
        {

        }
        return temp;
    }
}

