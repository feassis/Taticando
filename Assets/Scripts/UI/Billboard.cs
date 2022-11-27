using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cam;

    private void Start()
    {
        cam = ServiceLocator.GetService<CameraService>().GetMainCameraRigTransform();
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
