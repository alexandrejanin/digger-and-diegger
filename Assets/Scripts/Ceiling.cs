using UnityEngine;

public class Ceiling : MonoBehaviour {
    [SerializeField, Min(0)] private float speed = 1;
    [SerializeField, Min(0)] private float minDistance = 4;
    [SerializeField, Min(0)] private float maxSeconds = 4;

    public float MinDistance => minDistance;
    public float MaxDistance => minDistance + maxSeconds * speed;

    private GameManager manager;

    private void Awake() {
        manager = FindObjectOfType<GameManager>();
    }

    private void Update() {
        if (!manager.IsPlaying)
            return;

        var diff = Time.deltaTime * speed * Vector3.down;

        if (!manager.InDigPhase)
            diff /= 2;

        transform.position += diff;

        if (Mathf.Abs(transform.position.y - manager.Players.transform.position.y) > MaxDistance)
            transform.position = new Vector3(transform.position.x, manager.Players.transform.position.y + MaxDistance, transform.position.z);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player"))
            FindObjectOfType<GameManager>().Lose();
    }
}
