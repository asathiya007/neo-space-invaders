using UnityEngine;


public class Bunker : MonoBehaviour
{
    // bunker disappears if hit by invader
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader")) {
            this.gameObject.SetActive(false);
        }
    }
}
