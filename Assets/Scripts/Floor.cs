using UnityEngine;

public class Floor : MonoBehaviour {
    private Vector3 targetPosition;

    private ParticleSystem dust;

    public float Depth {
        get => targetPosition.y;
        set {
            targetPosition.y = value;
            dust.Play();
        }
    }

    private void Awake() {
        targetPosition = transform.position;
        dust = GetComponentInChildren<ParticleSystem>();
    }

    private void Update() {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            transform.position = targetPosition;
    }
}
