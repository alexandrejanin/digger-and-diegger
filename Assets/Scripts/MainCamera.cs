using UnityEngine;

public class MainCamera : MonoBehaviour {
    [SerializeField] private Players players;

    private GameManager manager;

    private float yOffset;

    private void Awake() {
        manager = FindObjectOfType<GameManager>();
        yOffset = transform.position.y - players.transform.position.y;
    }

    private void Update() {
        var ceilingDistance = manager.Ceiling.transform.position.y - manager.Players.transform.position.y;

        var fovT = Mathf.InverseLerp(manager.Ceiling.MaxDistance, manager.Ceiling.MinDistance, ceilingDistance);
        var z = 20 * fovT - 30;

        var targetPos = new Vector3(0, players.transform.position.y + yOffset, z);

        transform.position = Vector3.Lerp(transform.position, targetPos, 0.05f);
    }
}
