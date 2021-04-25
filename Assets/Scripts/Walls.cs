using UnityEngine;

public class Walls : MonoBehaviour {
    [SerializeField] private float wallHeight = 17.15f;

    private bool spawned;

    private void Update() {
        var camY = Camera.main.transform.position.y;

        if (camY < transform.position.y && !spawned) {
            Instantiate(this, transform.position + wallHeight * Vector3.down, transform.rotation, transform.parent);
            spawned = true;
        }

        if (camY < transform.position.y - 2 * wallHeight)
            Destroy(gameObject);
    }
}
