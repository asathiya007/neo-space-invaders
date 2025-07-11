using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;


public class BackgroundImage : MonoBehaviour
{
    public string genaiApiUrl;
    public RawImage backgroundImage;
    private string[] bgPrompts = {
        "An image of a planet in outer space",
        "An image of a galaxy in outer space",
        "An image of a supernova in outer space",
        "An image of a black hole in outer space",
        "An image of a wormhole in outer space"
    };
    private System.Random random = new System.Random();
    private string bgPrompt;

    // load AI-generated background image
    void Start()
    {
        StartCoroutine(LoadImage());
    }

    // loads AI-generated background image from API
    IEnumerator LoadImage()
    {
        bgPrompt = bgPrompts[random.Next(bgPrompts.Length)];
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(
            genaiApiUrl + "/ai_generated_image?text="
            + $"{bgPrompt.Replace(" ", "%20")}");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Image loaded successfully");
            backgroundImage.color = Color.white;
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            backgroundImage.texture = texture;
        }
        else
        {
            Debug.LogError("Image load failed: " + request.error);
        }
    }
}
