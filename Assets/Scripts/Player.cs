using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    public Projectile laserPrefab;
    public float speed = 5.0f;
    private bool laserActive;

    // moves player based on keyboard/mouse input
    private void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            this.transform.position += 
                Vector3.left * this.speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.D)
                || Input.GetKey(KeyCode.RightArrow)) {
            this.transform.position +=
                Vector3.right * this.speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space)
                || Input.GetMouseButtonDown(0)) {
            Shoot();
        } 
    }

    // shoots laser (only one active laser at a time)
    private void Shoot()
    {
        if (!laserActive) {
            Projectile projectile = Instantiate(
                this.laserPrefab, this.transform.position,
                Quaternion.identity);
            projectile.destroyed += LaserDestroyed;
            laserActive = true;
        }
    }

    // resets active status when laser is destroyed
    private void LaserDestroyed()
    {
        laserActive = false;
    }

    // reload scene when hit by invader or missile
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader") ||
            other.gameObject.layer == LayerMask.NameToLayer("Missile")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
