using System.Collections.Generic;
using UnityEngine;

public class TestScheme : MonoBehaviour
{
    public enum ShapeType
    {
        Cube,
        Sphere,
        Cylinder,
        Cone,
        Capsule
    }

    public ShapeType shape; // Выбор формы в инспекторе
    public GameObject quarkPrefab; // Префаб кварка
    public Vector3Int dimensions;  // Размеры схемы (длина, ширина, высота)
    public Vector3 spacing = new Vector3(0.1f, 0.1f, 0.1f); // Расстояние между кварками
    public List<MaterialProbability> materialProbabilities; // Вероятности материалов

    private List<TestQuark> quarksList = new List<TestQuark>();

    [System.Serializable]
    public struct MaterialProbability
    {
        public Material material;
        public float probability;
        public int cost;
    }

    void Start()
    {
        GenerateSchema();
    }

    void GenerateSchema()
    {
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                for (int z = 0; z < dimensions.z; z++)
                {
                    if (IsPositionValid(x, y, z)) // Проверка для круглых объектов
                    {
                        Vector3 position = new Vector3(x * spacing.x, y * spacing.y, z * spacing.z);
                        GameObject newQuark = Instantiate(quarkPrefab, position, Quaternion.identity, transform);
                        TestQuark quark = newQuark.GetComponent<TestQuark>();

                        AssignMaterialToQuark(quark);
                        quarksList.Add(quark);
                    }
                }
            }
        }
    }

    bool IsPositionValid(int x, int y, int z)
    {
        switch (shape)
        {
            case ShapeType.Cube:
                return true; // Куб заполняется полностью
            case ShapeType.Sphere:
                return IsInSphere(x, y, z);
            case ShapeType.Cylinder:
                return IsInCylinder(x, y, z);
            case ShapeType.Cone:
                return IsInCone(x, y, z);
            case ShapeType.Capsule:
                return IsInCapsule(x, y, z);
            default:
                return true;
        }
    }
    void AssignMaterialToQuark(TestQuark quark)
    {
        float randomValue = Random.Range(0f, 1f);
        float cumulativeProbability = 0f;

        foreach (var materialProbability in materialProbabilities)
        {
            cumulativeProbability += materialProbability.probability;
            if (randomValue <= cumulativeProbability)
            {
                quark.SetMaterial(materialProbability.material, materialProbability.cost);
                break;
            }
        }
    }
    bool IsInSphere(int x, int y, int z)
    {
        Vector3 center = new Vector3(dimensions.x / 2.0f, dimensions.y / 2.0f, dimensions.z / 2.0f);
        Vector3 position = new Vector3(x, y, z);
        float radius = Mathf.Min(dimensions.x, dimensions.y, dimensions.z) / 2.0f;
        return (position - center).sqrMagnitude <= radius * radius;
    }

    bool IsInCylinder(int x, int y, int z)
    {
        Vector3 center = new Vector3(dimensions.x / 2.0f, 0, dimensions.z / 2.0f);
        Vector3 position = new Vector3(x, 0, z);
        float radius = Mathf.Min(dimensions.x, dimensions.z) / 2.0f;
        return (position - center).sqrMagnitude <= radius * radius && y < dimensions.y;
    }

    bool IsInCone(int x, int y, int z)
    {
        Vector3 center = new Vector3(dimensions.x / 2.0f, 0, dimensions.z / 2.0f);
        Vector3 position = new Vector3(x, 0, z);
        float maxRadius = Mathf.Min(dimensions.x, dimensions.z) / 2.0f;
        float height = dimensions.y;
        float currentRadius = maxRadius * (1.0f - (y / height));
        return (position - center).sqrMagnitude <= currentRadius * currentRadius && y < dimensions.y;
    }

    bool IsInCapsule(int x, int y, int z)
    {
        Vector3 centerTop = new Vector3(dimensions.x / 2.0f, dimensions.y - Mathf.Min(dimensions.x, dimensions.z) / 2.0f, dimensions.z / 2.0f);
        Vector3 centerBottom = new Vector3(dimensions.x / 2.0f, Mathf.Min(dimensions.x, dimensions.z) / 2.0f, dimensions.z / 2.0f);
        Vector3 position = new Vector3(x, y, z);
        float radius = Mathf.Min(dimensions.x, dimensions.z) / 2.0f;
        if (y >= centerBottom.y && y <= centerTop.y)
        {
            return (new Vector3(position.x, 0, position.z) - new Vector3(centerBottom.x, 0, centerBottom.z)).sqrMagnitude <= radius * radius;
        }
        return (position - centerBottom).sqrMagnitude <= radius * radius || (position - centerTop).sqrMagnitude <= radius * radius;
    }
    public TestQuark GetQuarkAt(int index)
    {
        if (index >= 0 && index < quarksList.Count)
        {
            return quarksList[index];
        }
        return null;
    }

    public List<TestQuark> GetAllQuarks()
    {
        return quarksList;
    }
}


