using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    #region --Serialized Fields --
    
    [FormerlySerializedAs("planetCamera")]
    [FormerlySerializedAs("vcam")]
    [Header("Camera Target")]
    [SerializeField] private CinemachineCamera defaultCamera;

    [Header("Variables")] 
    [SerializeField] private float panSpeed;
    [SerializeField] float zoomSpeed;
    [FormerlySerializedAs("zoomMax")] [SerializeField] float zoomOutMax;
    [SerializeField] float zoomInMax;
    [SerializeField] float maxCamXRotation;
    
    #endregion

    private bool isPanningLeft;
    bool isPanningRight;
    private bool isPanningUp;
    bool isPanningDown;
    private bool isMovingUp;
    bool isMovingDown;
    private bool isMovingVertical;
    private bool isRotatingCamera;

    int scrollDirection;
    Vector2 currentCameraRotation;
    
    public static CameraManager instance;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
    
        instance = this;
    }

    private void Start()
    {
        currentCameraRotation.x = defaultCamera.transform.eulerAngles.x;
        currentCameraRotation.y = defaultCamera.transform.eulerAngles.y;
    }

    private void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        if (isPanningLeft)
        {
            PanCameraLeft();
        }
        
        if (isPanningRight)
        {
            PanCameraRight();
        }

        if (isPanningUp)
        {
            PanCameraUp();
        }

        if (isPanningDown)
        {
            PanCameraDown();
        }

        if (isMovingUp)
        {
            MoveCameraUp();
        }

        if (isMovingDown)
        {
            MoveCameraDown();
        }
        
        if(isRotatingCamera)
            RotateCamera();
        
        if(scrollDirection != 0)
            MoveCameraVertical();
    }

    #region -- Movement Logic --
    
    void PanCameraLeft()
    {
        Vector3 direction = defaultCamera.transform.right;
        direction.y = 0;
        direction.Normalize();
        defaultCamera.transform.Translate(direction * (Time.unscaledDeltaTime * -panSpeed), Space.World);
    }
    
    void PanCameraRight()
    {
        Vector3 direction = defaultCamera.transform.right;
        direction.y = 0;
        direction.Normalize();
        defaultCamera.transform.Translate(direction * (Time.unscaledDeltaTime * panSpeed), Space.World);
    }
    
    void PanCameraDown()
    {
        defaultCamera.transform.position += -defaultCamera.transform.forward * (Time.deltaTime * panSpeed);
        // defaultCamera.transform.position += Vector3.down * (Time.deltaTime * panSpeed);
    }
    
    void PanCameraUp()
    {
        defaultCamera.transform.position += defaultCamera.transform.forward * (Time.deltaTime * panSpeed);
        // defaultCamera.transform.position += Vector3.up * (Time.deltaTime * panSpeed);
    }

    void MoveCameraUp()
    {
        defaultCamera.transform.position += Vector3.up * (Time.deltaTime * panSpeed);
    }

    void MoveCameraDown()
    {
        defaultCamera.transform.position += Vector3.down * (Time.deltaTime * panSpeed);
    }

    void MoveCameraVertical()
    {
        if (scrollDirection > 0)
        {
            defaultCamera.transform.position += Vector3.up * (Time.deltaTime * panSpeed);
            //defaultCamera.transform.position += defaultCamera.transform.forward * (Time.deltaTime * panSpeed);
        }
        else if (scrollDirection < 0)
        {
            defaultCamera.transform.position += Vector3.down * (Time.deltaTime * panSpeed);
            //defaultCamera.transform.position += -defaultCamera.transform.forward * (Time.deltaTime * panSpeed);
        }
    }
    
    #endregion
    
    #region --Input Listeners --
    
    public void PanLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPanningLeft = true;
        }
        else if (context.canceled)
        {
            isPanningLeft = false;
        }
    }
    
    public void PanRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPanningRight = true;
        }
        else if (context.canceled)
        {
            isPanningRight = false;
        }
    }
    
    public void PanUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPanningUp = true;
        }
        else if (context.canceled)
        {
            isPanningUp = false;
        }
    }
    
    public void PanDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPanningDown = true;
        }
        else if (context.canceled)
        {
            isPanningDown = false;
        }
    }
    
    public void RotateLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isMovingUp = true;
        }
        else if (context.canceled)
        {
            isMovingUp = false;
        }
    }
    
    public void RotateRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isMovingDown = true;
        }
        else if (context.canceled)
        {
            isMovingDown = false;
        }
    }
    
    public void ZoomIn(InputAction.CallbackContext context)
    {
        float scrollY = context.ReadValue<float>();

        if (scrollY > 0)
        {
            scrollDirection = 1;
        }    
        else if (scrollY < 0)
        {
            scrollDirection = -1;
        }
        else if (scrollY == 0)
        {
            scrollDirection = 0;
        }
    }

    public void RotateCamera(InputAction.CallbackContext context)
    {
        if(context.performed)
            isRotatingCamera = true;
        else if (context.canceled)
            isRotatingCamera = false;
    }
    
    void RotateCamera()
    {
        currentCameraRotation.x += -Mouse.current.delta.ReadValue().y;
        
        if(currentCameraRotation.x < -maxCamXRotation)
            currentCameraRotation.x = -maxCamXRotation;
        else if(currentCameraRotation.x > maxCamXRotation)
            currentCameraRotation.x = maxCamXRotation;
        
        currentCameraRotation.y += Mouse.current.delta.ReadValue().x;
        
        defaultCamera.transform.eulerAngles = new Vector3(currentCameraRotation.x, currentCameraRotation.y, defaultCamera.transform.eulerAngles.z);
    }
    #endregion
}
