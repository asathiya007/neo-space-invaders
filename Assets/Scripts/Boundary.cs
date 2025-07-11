using UnityEngine;
using UnityEngine.SceneManagement;


public class Boundary : MonoBehaviour
{
    // bunker disappears if hit by invader
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
