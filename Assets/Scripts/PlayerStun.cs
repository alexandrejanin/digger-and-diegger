using UnityEngine;

public class PlayerStun : MonoBehaviour {
    public bool active;

    private void Update() {
        if (!active) {
            transform.up = Vector3.up;
            return;
        }

        transform.up = new Vector3(Mathf.Cos(5 * Time.time + transform.position.x) / 5, 1, Mathf.Sin(5 * Time.time + transform.position.x) / 5);
    }
}
