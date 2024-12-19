using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quark : MonoBehaviour
{
    public int cost;
    public float size = 0.1f;
    public Material material;
    public float posX;
    public float posY;
    public float posZ;

    public void Init(Vector3 pos, int material)
    {
        gameObject.transform.position = pos;
        gameObject.transform.localScale = Vector3.one * size;
        GetComponent<MeshRenderer>().enabled = false;
        cost = material * 10; //TO DO: переписать, добавить список материалов и их плотность.
    }
}
