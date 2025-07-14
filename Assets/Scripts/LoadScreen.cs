using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class LoadScreen : MonoBehaviour {           
    public RawImage backgroundImage;
    public string genaiApiUrl;
    private string[] bgPrompts = {
        "An image of a planet in outer space",
        "An image of a galaxy in outer space",
        "An image of a supernova in outer space",
        "An image of a black hole in outer space",
        "An image of a wormhole in outer space"
    };
    private string bgPrompt;
    private string[] invaderPrompts = {
        "An animated alien head icon",
        "An animated flying saucer icon"        
    };
    private string invaderPrompt;
    private System.Random random = new System.Random();
    private bool bgImgReqCompleted = false;
    private bool invImgReqCompleted = false;

    // gets AI-generated background image from API
    IEnumerator GetBackgroundImage() {
        bgPrompt = bgPrompts[random.Next(bgPrompts.Length)];
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(
            genaiApiUrl + "/ai_generated_image?text="
            + $"{bgPrompt.Replace(" ", "%20")}");
        request.SetRequestHeader("ngrok-skip-browser-warning", "");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Request for AI-generated background image succeeded");
            this.SaveImg(request, ImageLoader.GameBgPath);
            bgImgReqCompleted = true;
        }
        else
        {
            Debug.LogError(
                "Request for AI-generated background image failed: "
                + request.error);
            bgImgReqCompleted = true;
        }
    }

    // get AI-generated image from API, for invaders
    IEnumerator GetInvaderImage() {
        invaderPrompt = invaderPrompts[random.Next(invaderPrompts.Length)];
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(
            genaiApiUrl + "/ai_generated_image?text="
            + $"{invaderPrompt.Replace(" ", "%20")}&crop=circle");
        request.SetRequestHeader("ngrok-skip-browser-warning", "");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(
                "Request for AI-generated invader image succeeded");            
            this.SaveImg(request, ImageLoader.InvaderPath);
            invImgReqCompleted = true;
        }
        else
        {
            Debug.LogError(
                "Request for AI-generated invader image failed: "
                + request.error);
            invImgReqCompleted = true;
        }
    }


    // load and set background image, get AI-generated content
    void Start() {
        // load background image
        Texture2D loadedTex = ImageLoader.LoadImageFromFile(
            ImageLoader.LoadScreenPath);
        backgroundImage.texture = loadedTex;

        // get AI-generated background image
        StartCoroutine(GetBackgroundImage());

        // get AI-generated invader image
        StartCoroutine(GetInvaderImage());

        // start game after AI-generated images have been loaded
        InvokeRepeating("StartGame", 1.0f, 1.0f);
    }

    // function to check if web requests have completed
    private void StartGame() {
        if (this.bgImgReqCompleted && this.invImgReqCompleted)
        {
            SceneLoader.LoadGameScene();
        }
    }
    
    // function to save image obtained via web request
    private void SaveImg(UnityWebRequest request, string savePath) {
        Texture2D texture = DownloadHandlerTexture.GetContent(request);
        byte[] imageBytes = texture.EncodeToPNG();
        File.WriteAllBytes(savePath, imageBytes);
    }
}
