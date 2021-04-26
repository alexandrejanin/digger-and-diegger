using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class Wall : MonoBehaviour {
    [SerializeField] private Mesh[] meshes;
    [SerializeField] private Material[] materials;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private void Awake() {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        var meshIndex = Random.Range(0, meshes.Length);
        meshFilter.mesh = meshes[meshIndex];
        if (meshIndex == 0)
            return;

        var mats = meshRenderer.materials;
        mats[2] = materials[Random.Range(0, materials.Length)];
        meshRenderer.materials = mats;
    }
}
