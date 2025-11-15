using System;
using System.Text.Json;

[System.Serializable]
public class FileSettings
{
    //Put variables here, make sure your variables are public and with { get; set;} otherwise it won't save
    //This file is used for important, non save file uses such as achievements and settings
    /// <summary>
    /// The Width the window will display out at
    /// </summary>
    public int windowResolutionWidth { get; set; }
    /// <summary>
    /// The Length the window will display out at
    /// </summary>
    public int windowResolutionHeight { get; set; }
    /// <summary>
    /// The bool determining if the game will pause when there is a lost of focus
    /// </summary>
    public bool pauseOnUF { get; set; }
    /// <summary>
    /// The bool determining if the game will start up fullscreen
    /// </summary>
    public bool isFullscreen { get; set; }
    /// <summary>
    /// Put the "New Game" variable values here
    /// </summary>
    public FileSettings()
    {
        pauseOnUF = true;
        isFullscreen = true;
    }

}
