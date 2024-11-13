using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public void Init(Camera cam, Vector3 zeroPoint)
    {
        cam.transform.position = Vector3.one * 6;
        cam.transform.LookAt(zeroPoint);
        Debug.Log("Cam init");
    }
}
