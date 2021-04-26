using UnityEngine;

public abstract class Minigame : MonoBehaviour {
    public abstract string Description { get; }
    public abstract ButtonType? DiggerButton { get; }
    public abstract ButtonType? DiggurButton { get; }
}
