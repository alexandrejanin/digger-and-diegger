using UnityEngine;

public class MainCamera : MonoBehaviour {
    [SerializeField] private Players players;

    private void Update() {
        var targetPos = players.transform.position;
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.1f);
    }
}
