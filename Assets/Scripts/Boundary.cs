using UnityEngine;


public class Boundary : MonoBehaviour {
    // if invader reaches boundary, load game over scene
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader")) {
            SceneLoader.LoadGameOverScene();
        }
    }
}
