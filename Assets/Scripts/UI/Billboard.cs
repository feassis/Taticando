using MVC.View.CameraControl;
using Tools;
using UnityEngine;

namespace MVC.View.UI
{
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
}


