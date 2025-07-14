using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class BackgroundImage : MonoBehaviour {
    public RawImage backgroundImage;

    // load AI-generated background image
    void Start() {
        if (File.Exists(ImageLoader.GameBgPath)) {
            backgroundImage.color = Color.white;
            Texture2D texture = ImageLoader.LoadImageFromFile(
                ImageLoader.GameBgPath);
            backgroundImage.texture = texture;
        }
    }
}
