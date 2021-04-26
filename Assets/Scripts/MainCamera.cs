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

        var yOffset = manager.Minigame is MoleMinigame ? ceilingDistance / 2 : Mathf.Lerp(3, 1.5f, ceilingDistanceNormalized);
        var z = Mathf.Lerp(-30, -10, ceilingDistanceNormalized);

        var targetPos = new Vector3(0, manager.Players.transform.position.y + yOffset, z);

        transform.position = Vector3.Lerp(transform.position, targetPos, 0.05f);
    }
}
