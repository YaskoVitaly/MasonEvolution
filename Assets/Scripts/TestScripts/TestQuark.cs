using UnityEngine;

public class TestQuark : MonoBehaviour
{
    public string materialName;  // ��� ���������
    public int cost;             // ��������� ������
    public Vector3 size = new Vector3(0.1f, 0.1f, 0.1f); // ������ ������

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

