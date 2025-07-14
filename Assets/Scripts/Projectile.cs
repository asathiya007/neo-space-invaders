using UnityEngine;


public class Projectile : MonoBehaviour {
    public Vector3 direction; 
    public float speed;
    public System.Action destroyed;

    // updates position of projectile as it moves
    private void Update() {
        this.transform.position +=
            this.direction * this.speed * Time.deltaTime;
    }

    // runs additional logic when projectile is destroyed, and destroys
    // projectile object
    private void OnTriggerEnter2D(Collider2D other) {
        if (this.destroyed != null) {
            this.destroyed.Invoke();
        }
        Destroy(this.gameObject);
    }
}
