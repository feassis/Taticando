using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inspired by: https://www.youtube.com/watch?v=rnqF6S7PfFA&list=PLL_zf3MigDAPckjYBJ1nha_Toww3zHY7j&index=2&t=32s
public class CameraController : MonoBehaviour
{
    public float FastSpeed;
    public float NormalSpeed;
    public float MovementTime;
    public float RotationAmount;
    public Vector3 ZoomAmount;
    public float MouseZoomMultiplier;

    [SerializeField] private Transform cameraTransform;

    private Quaternion newRotation;
    private Vector3 newPosition;
    private Vector3 newZoom;
    private float movementSpeed;
    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;
    private Vector3 rotateStartPositon;
    private Vector3 rotateCurrentPositon;

    private void Awake()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    private void Update()
    {
        HandleKeyBoardInput();
        HandleMouseInput();

        UpdateMovement();
        UpdateRotation();
        UpdateZoom();
    }

    private void HandleKeyBoardInput()
    {
        HandleMovementInputKeyBoadInput();
        HandleRotationInputKeyBoardInput();
        HandlesZoomInputKeyBoardInput();
    }

    private void HandleMouseInput()
    {
        HandlesMouseZoom();
        HandleMouseMovement();
        HandlesMouseRotation();
    }

    private void HandlesMouseZoom()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * ZoomAmount * MouseZoomMultiplier;
        }
    }

    private void HandleMouseMovement()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }

        if (Input.GetMouseButton(2))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }
    }

    private void HandlesMouseRotation()
    {
        if (Input.GetMouseButtonDown(1))
        {
            rotateStartPositon = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            rotateCurrentPositon = Input.mousePosition;

            Vector3 diference = rotateStartPositon - rotateCurrentPositon;

            rotateStartPositon = rotateCurrentPositon;

            newRotation *= Quaternion.Euler(Vector3.up * (-diference.x / 5f));
        }
    }


    private void HandleMovementInputKeyBoadInput()
    {
        movementSpeed = NormalSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = FastSpeed;
        }

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += transform.forward * movementSpeed;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition -= transform.forward * movementSpeed;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += transform.right * movementSpeed;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition -= transform.right * movementSpeed;
        }
    }

    private void HandleRotationInputKeyBoardInput()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * RotationAmount);
        }

        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * (-RotationAmount));
        }  
    }

    private void HandlesZoomInputKeyBoardInput()
    {
        if (Input.GetKey(KeyCode.R))
        {
            newZoom += ZoomAmount;
        }

        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= ZoomAmount;
        }
    }

    private void UpdateMovement()
    {
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * MovementTime);
    }
    private void UpdateRotation()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * MovementTime);
    }

    private void UpdateZoom()
    {
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * MovementTime);
    }
}
