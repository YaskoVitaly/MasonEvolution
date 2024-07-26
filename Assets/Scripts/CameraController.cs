using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public Vector3 zeroPoint;

    public void Init(Transform _schemePrefab)
    {
        mainCamera.transform.position = Vector3.one * 6;
        mainCamera.transform.LookAt(_schemePrefab);
    }
}
