using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;


public class Invaders : MonoBehaviour
{
    public Invader[] prefabs;
    public int rows = 5;
    public int columns = 11;
    public AnimationCurve speed;
    public int amountKilled { get; private set; }
    public int totalInvaders => this.rows * this.columns;
    public float percentKilled => (float) this.amountKilled / (float) this.totalInvaders; 
    public int amountAlive => this.totalInvaders - this.amountKilled;
    public Projectile missilePrefab; 
    public float missileAttackRate = 1.0f;
    private Vector3 direction = Vector2.right;
    private string prompt;
    public string genaiApiUrl;
    private Invader[] invaders;
    private string[] invaderPrompts = {
        "an animated alien head icon",
        "an animated flying saucer icon"        
    };
    private System.Random random = new System.Random();
    private string invaderPrompt;

    // create grid of invaders
    private void Awake()
    {
        invaders = new Invader[this.rows * this.columns];
        int instantiatedSprites = 0;
        for (int row = 0; row < this.rows; row++)
        {
            float width = 2.0f * (this.columns - 1);
            float height = 2.0f * (this.rows - 1);
            Vector2 centering = new Vector2(-width / 2, -height / 2);
            Vector3 rowPosition = new Vector3(
                centering.x, centering.y + (row * 2.0f), 0.0f);

            for (int col = 0; col < this.columns; col++)
            {
                Invader invader = Instantiate(
                    this.prefabs[row], this.transform);
                invaders[instantiatedSprites] = invader;
                invader.killed += InvaderKilled;
                Vector3 position = rowPosition;
                position.x += col * 2.0f;
                invader.transform.localPosition = position;
                instantiatedSprites++;
            }
        }
    }

    // invaders launch missiles, AI-generated images loaded for invaders
    private void Start()
    {
        InvokeRepeating(
            nameof(MissileAttack), this.missileAttackRate,
            this.missileAttackRate);

        StartCoroutine(LoadImages());
    }

    // loads AI-generated images from API, for invaders
    IEnumerator LoadImages()
    {
        invaderPrompt = invaderPrompts[random.Next(invaderPrompts.Length)];
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(
            genaiApiUrl + "/ai_generated_image?text="
            + $"{invaderPrompt.Replace(" ", "%20")}&crop=circle");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Image loaded successfully");
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite newSprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f), (float) texture.width / (float) 1.7f 
            );
            Sprite[] genAISprites = {newSprite};
            GameObject gameObj;
            foreach (Invader invader in this.invaders) {
                invader.animationSprites = genAISprites;
                invader.spriteRenderer.sprite = newSprite;

                // remove box collider if it exists
                gameObj = invader.gameObject;
                BoxCollider2D boxCollider = gameObj.GetComponent<BoxCollider2D>();
                if (boxCollider != null)
                {
                    Destroy(boxCollider);
                }

                // add circle collider, adjust so it covers sprite fully
                CircleCollider2D circleCollider = gameObj.AddComponent<CircleCollider2D>();
                Vector2 spriteSize = invader.spriteRenderer.bounds.size;
                circleCollider.radius = Mathf.Max(spriteSize.x, spriteSize.y) * 0.5f;
            }
            Debug.Log("Updated animation sprites");
        }
        else
        {
            Debug.LogError("Image load failed: " + request.error);
        }
    }

    // moves invaders horizontally, and then downward once reaching edge
    // of view
    private void Update()
    {
        this.transform.position += direction * this.speed.Evaluate(
            this.percentKilled) * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }

            if (direction == Vector3.right &&
            invader.position.x >= (rightEdge.x - 1.0f)) {
                AdvanceRow();
            } else if (direction == Vector3.left
                    && invader.position.x <= (leftEdge.x + 1.0f)) {
                AdvanceRow(); 
            }
        }
    }

    private void AdvanceRow()
    {
        direction.x *= -1.0f;

        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;
    }

    private void InvaderKilled()
    {
        this.amountKilled++;

        if (this.amountKilled >= this.totalInvaders) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void MissileAttack()
    {
        foreach (Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }

            if (UnityEngine.Random.value < (1.0f / (float) this.amountAlive)) {
                Instantiate(
                    this.missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }
}
