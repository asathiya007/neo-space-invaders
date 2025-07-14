using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class Invaders : MonoBehaviour {
    public Invader[] prefabs;
    public int rows = 5;
    public int columns = 11;
    public AnimationCurve speed;
    public int amountKilled { get; private set; }
    public int totalInvaders => this.rows * this.columns;
    public float percentKilled => (float) this.amountKilled
        / (float) this.totalInvaders;
    public int amountAlive => this.totalInvaders - this.amountKilled;
    public Projectile missilePrefab; 
    public float missileAttackRate = 1.0f;
    private Vector3 direction = Vector2.right;
    private Invader[] invaders;
    private float elapsedTime = 0f;
    private float invaderKillRate => (float) this.amountKilled
        / this.elapsedTime;
    private float normalizedKillRate => Mathf.InverseLerp(
        0f, 1.25f, invaderKillRate);
    // increase to give more influence to kill rate when determining
    // invader speed, decrease to give more influence to percent killed
    // when determining invader speed
    private float killRateWeight = 0.8f;
    private float adaptDiff => (this.killRateWeight * this.normalizedKillRate)
        + ((1.0f - this.killRateWeight) * this.percentKilled);

    // create grid of invaders
    private void Awake() {
        invaders = new Invader[this.rows * this.columns];
        int instantiatedSprites = 0;
        for (int row = 0; row < this.rows; row++) {
            float width = 2.0f * (this.columns - 1);
            float height = 2.0f * (this.rows - 1);
            Vector2 centering = new Vector2(-width / 2, -height / 2);
            Vector3 rowPosition = new Vector3(
                centering.x, centering.y + (row * 2.0f), 0.0f);

            for (int col = 0; col < this.columns; col++) {
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
    private void Start() {
        if (File.Exists(ImageLoader.InvaderPath)) {
            Texture2D texture = ImageLoader.LoadImageFromFile(
            ImageLoader.InvaderPath);
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
                if (boxCollider != null) {
                    Destroy(boxCollider);
                }

                // add circle collider, adjust so it covers sprite fully
                CircleCollider2D circleCollider = gameObj.AddComponent<CircleCollider2D>();
                Vector2 spriteSize = invader.spriteRenderer.bounds.size;
                circleCollider.radius = Mathf.Max(spriteSize.x, spriteSize.y) * 0.5f;
            }
        }

        InvokeRepeating(
            nameof(MissileAttack), this.missileAttackRate,
            this.missileAttackRate);
    }

    // moves invaders horizontally, and then downward once reaching edge
    // of view
    private void Update() {
        this.elapsedTime += Time.deltaTime;

        // speed of invaders changes based on player performance
        // and game progress (adaptive difficulty)
        this.transform.position += direction * this.speed.Evaluate(
            adaptDiff) * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        foreach (Transform invader in this.transform) {
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

    // utility function to move the row downward and in the opposite
    // horizontal direction
    private void AdvanceRow() {
        direction.x *= -1.0f;

        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;
    }

    private void InvaderKilled() {
        this.amountKilled++;

        // load win scene scene if game won
        if (this.amountKilled >= this.totalInvaders) {
            SceneLoader.LoadWinScene();
        }
    }

    // probability of an invader shooting a missle changes based on
    // player performance and game progress (adaptive difficulty)
    private void MissileAttack() {
        foreach (Transform invader in this.transform) {
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }

            if (UnityEngine.Random.value < adaptDiff) {
                Instantiate(
                    this.missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }
}
