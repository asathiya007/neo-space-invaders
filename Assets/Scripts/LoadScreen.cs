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
        "An animated alien head icon",
        "An animated flying saucer icon"
    };
    private string invaderPrompt;
    private System.Random random = new System.Random();
    private bool bgImgReqCompleted = false;
    private bool[] invImgReqCompleted = {false, false, false};

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
    IEnumerator GetInvaderImage(int invaderNum) {
        // get save path and prompt to generate image
        string savePath;
        if (invaderNum == 1) {
            savePath = ImageLoader.InvaderPath1;
        } else if (invaderNum == 2) {
            savePath = ImageLoader.InvaderPath2;
        } else {
            savePath = ImageLoader.InvaderPath3;
        } 
        string invaderPrompt = invaderPrompts[invaderNum - 1];

        // make web request, process result
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(
            genaiApiUrl + "/ai_generated_image?text="
            + $"{invaderPrompt.Replace(" ", "%20")}&crop=circle");
        request.SetRequestHeader("ngrok-skip-browser-warning", "");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(
                $"Request for AI-generated invader image {invaderNum} "
                + "succeeded");            
            this.SaveImg(request, savePath);
            invImgReqCompleted[invaderNum - 1] = true;
        }
        else
        {
            Debug.LogError(
                $"Request for AI-generated invader image {invaderNum} "
                + $"failed: {request.error}");
            invImgReqCompleted[invaderNum - 1] = true;
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

        // get AI-generated invader images
        StartCoroutine(GetInvaderImage(1));
        StartCoroutine(GetInvaderImage(2));
        StartCoroutine(GetInvaderImage(3));

        // start game after AI-generated images have been loaded
        InvokeRepeating("StartGame", 1.0f, 1.0f);
    }

    // function to check if web requests have completed
    private void StartGame() {
        bool requestsCompleted = this.bgImgReqCompleted;
        for (int i = 0; i < this.invImgReqCompleted.Length; i++) {
            requestsCompleted = requestsCompleted
                && this.invImgReqCompleted[i];
        }
        if (requestsCompleted) {
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
