using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class GameOverScreen : MonoBehaviour {           
    public RawImage backgroundImage;   

    // load and set background image
    void Start() {
        Texture2D loadedTex = ImageLoader.LoadImageFromFile(
            ImageLoader.GetRandomGameOverScreenPath());
        backgroundImage.texture = loadedTex;
    }
}