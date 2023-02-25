using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace MVC.View.CameraControl
{
    public class CameraService : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform mainCameraTransform;
        [SerializeField] private Transform mainCameraRigTransform;

        private void Awake()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            if (mainCameraTransform == null)
            {
                mainCameraTransform = mainCamera.transform;
            }

            if (mainCameraRigTransform == null)
            {
                mainCameraRigTransform = mainCamera.GetComponentInParent<Transform>();
            }

            ServiceLocator.RegisterService<CameraService>(this);
        }

        public Transform GetMainCameraTransform()
        {
            return mainCameraTransform;
        }

        public Transform GetMainCameraRigTransform()
        {
            return mainCameraRigTransform;
        }

        private void OnDestroy()
        {
            ServiceLocator.DeregisterService<CameraService>();
        }
    }
}

