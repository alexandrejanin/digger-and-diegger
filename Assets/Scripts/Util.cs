using UnityEngine;

public static class Util {
    public static ButtonType RandomButton => buttonTypes[Random.Range(0, 4)];
    private static readonly ButtonType[] buttonTypes = {ButtonType.Down, ButtonType.Up, ButtonType.Left, ButtonType.Right};
}
