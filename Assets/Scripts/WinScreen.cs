using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class WinScreen : MonoBehaviour {           
    public RawImage backgroundImage;   

    // load and set background image
    void Start() {
        Texture2D loadedTex = ImageLoader.LoadImageFromFile(
            ImageLoader.WinScreenPath);
        backgroundImage.texture = loadedTex;
    }
}
