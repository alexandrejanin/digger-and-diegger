using UnityEngine;

public class Rumble : MonoBehaviour {
    [SerializeField, Min(0)] private float amplitude = 0.1f;

    private Vector3 offset;

    private void Update() {
        transform.position -= offset;

        offset = Random.insideUnitSphere * amplitude;
        offset.z = 0;

        transform.position += offset;
    }
}
