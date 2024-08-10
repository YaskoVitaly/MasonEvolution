using UnityEngine;

public class TestQuark : MonoBehaviour
{
    public string materialName;  // Имя материала
    public int cost;             // Стоимость кварка
    public Vector3 size = new Vector3(0.1f, 0.1f, 0.1f); // Размер кварка

    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer not found!");
            return;
        }
    }

    public void SetMaterial(Material material, int materialCost)
    {
        meshRenderer.material = material;
        cost = materialCost;
        materialName = material.name;
    }
}

