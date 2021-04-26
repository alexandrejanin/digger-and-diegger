using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Logo : MonoBehaviour {
    [SerializeField, Min(0)] private float angleScale = 30, angleSpeed = 1, sizeScale = 0.1f, sizeSpeed = 2;
    private RectTransform rectTransform;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update() {
        rectTransform.localEulerAngles = new Vector3(0, 0, angleScale * Mathf.Cos(angleSpeed * Time.time));
        var scale = 1 + sizeScale * Mathf.Sin(sizeSpeed * Time.time);
        rectTransform.localScale = new Vector3(scale, scale, 1);
    }
}
