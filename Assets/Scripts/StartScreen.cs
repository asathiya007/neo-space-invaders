using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class StartScreen : MonoBehaviour {           
    public RawImage backgroundImage;   

    // load and set background image
    void Start() {
        Texture2D loadedTex = ImageLoader.LoadImageFromFile(
            ImageLoader.StartScreenPath);
        backgroundImage.texture = loadedTex;
    }
}
