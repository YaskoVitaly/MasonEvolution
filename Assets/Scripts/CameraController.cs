using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public Vector3 zeroPoint;


    void Start()
    {
        mainCamera.transform.position = Vector3.one*6;

    }

    void Update()
    {
        mainCamera.transform.LookAt(zeroPoint);
    }
}
