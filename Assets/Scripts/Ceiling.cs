using UnityEngine;

public class Ceiling : MonoBehaviour {
    [SerializeField] private Players players;
    [SerializeField, Min(0)] private float maxDistance = 10;
    [SerializeField, Min(0)] private float speed = 0.75f;

    private void Update() {
        transform.position += Time.deltaTime * speed * Vector3.down;

        if (Mathf.Abs(transform.position.y - players.transform.position.y) > maxDistance)
            transform.position = new Vector3(transform.position.x, players.transform.position.y + maxDistance, transform.position.z);
    }
}
