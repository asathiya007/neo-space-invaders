using System;
using System.IO;
using UnityEngine;


// utility class for loading images from filepath
public static class ImageLoader {
    public const string StartScreenPath = "./Assets/Images/StartScreen.png";
    public const string LoadScreenPath = "./Assets/Images/LoadingScreen.png";
    public const string WinScreenPath = "./Assets/Images/WinScreen.png";
    public const string GameBgPath =
        "./Assets/Images/GenAI_Background.png";
    public const string InvaderPath1 =
        "./Assets/Images/GenAI_Invader1.png";
    public const string InvaderPath2 =
        "./Assets/Images/GenAI_Invader2.png";
    public const string InvaderPath3 =
        "./Assets/Images/GenAI_Invader3.png";

    // function to get randomly selected filepath for game over screen
    public static string GetRandomGameOverScreenPath() {
        System.Random random = new System.Random();
        string[] gameOverScreenPaths = { 
            "./Assets/Images/GameOverScreen1.png",
            "./Assets/Images/GameOverScreen2.png",
            "./Assets/Images/GameOverScreen3.png",
        };
        return gameOverScreenPaths[random.Next(gameOverScreenPaths.Length)];
    }

    // utility function for loading images from filepath
    public static Texture2D LoadImageFromFile(string path) {
        // check if image exists at filepath
        if (!File.Exists(path)) {
            Debug.LogError("Image file not found at path: " + path);
            return null;
        }

        // load and return image bytes
        byte[] fileData = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(10, 10); // placeholder, will get
        // replaced by image
        tex.LoadImage(fileData);
        return tex;
    }
}
