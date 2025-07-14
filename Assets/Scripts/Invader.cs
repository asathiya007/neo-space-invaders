using UnityEngine;


public class Invader : MonoBehaviour {
    public Sprite[] animationSprites;
    public float animationTime = 1.0f;
    public System.Action killed;
    public SpriteRenderer spriteRenderer;
    private int animationFrame;

    // gets sprite renderer
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // animates sprite on a regular time interval
    private void Start() {
        InvokeRepeating(
            nameof(AnimateSprite), this.animationTime, this.animationTime);
    }

    // animates by switching between images of sprite 
    private void AnimateSprite() {
        animationFrame++;

        if (animationFrame >= this.animationSprites.Length) {
            animationFrame = 0;
        }

        spriteRenderer.sprite = this.animationSprites[animationFrame];
    }

    // deactivate invader if hit by laser
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser")) {
            this.killed.Invoke();
            this.gameObject.SetActive(false);
        }
    }
}
