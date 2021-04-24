using UnityEngine;

public class Floor : MonoBehaviour {
    [SerializeField, Min(0)] private float digDistance = 0.25f;
    [SerializeField] private KeyCode[] inputCycle;
    [SerializeField] private int inputIndex;

    private KeyCode NextInput => inputCycle[inputIndex];

    private void Update() {
        if (Input.GetKeyDown(NextInput)) {
            transform.position += digDistance * Vector3.down;
            inputIndex++;
            inputIndex %= inputCycle.Length;
        }
    }
}
