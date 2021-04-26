using UnityEngine;

public class MainCamera : MonoBehaviour {
    private GameManager manager;

    private void Awake() {
        manager = FindObjectOfType<GameManager>();
    }

    private void Update() {
        if (!manager.Ceiling.isActiveAndEnabled || !manager.Players)
            return;

        var ceilingDistance = manager.Ceiling.transform.position.y - manager.Players.transform.position.y;

        var ceilingDistanceNormalized = Mathf.InverseLerp(manager.Ceiling.MaxDistance, manager.Ceiling.MinDistance, ceilingDistance);

        var yOffset = Mathf.Max(2, ceilingDistance / 2);
        var z = Mathf.Lerp(-30, -10, ceilingDistanceNormalized);

        var targetPos = new Vector3(0, manager.Players.transform.position.y + yOffset, z);

        transform.position = Vector3.Lerp(transform.position, targetPos, 0.05f);
    }
}
