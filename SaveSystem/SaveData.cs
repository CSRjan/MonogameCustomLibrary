using System;
using System.Text.Json;
[System.Serializable]
public class SaveData
{
    //Put variables here, make sure your variables are public and with { get; set;} otherwise it won't save
    public int highScore;
    /// <summary>
    /// Put the "New Game" variable values here
    /// </summary>
    public SaveData()
    {
       highScore = 0;
    }
}
