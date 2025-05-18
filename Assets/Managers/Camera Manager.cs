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
    [FormerlySerializedAs("platformCamera")] [SerializeField] private CinemachineCamera closeupCamera;
    [SerializeField] private CinemachineCamera bossCamera;

    [Header("Variables")] 
    [SerializeField] LayerMask cameraLayerMask;
    [SerializeField] private float panSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] float zoomSpeed;
    [FormerlySerializedAs("zoomMax")] [SerializeField] float zoomOutMax;
    [SerializeField] float zoomInMax;
    
    #endregion

    private bool isPanningLeft;
    bool isPanningRight;
    private bool isPanningUp;
    bool isPanningDown;
    private bool isRotatingLeft;
    bool isRotatingRight;
    private bool isZoomingIn;

    int scrollDirection;
    private float defaultYPos;

    private GameObject currentFocus;
    
    public static CameraManager instance;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
    
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        defaultYPos = defaultCamera.transform.position.y;
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

        if (isRotatingLeft)
        {
            RotateCameraLeft();
        }

        if (isRotatingRight)
        {
            RotateCameraRight();
        }

        if (scrollDirection != 0)
        {
            ZoomCamera();
        }
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
        Vector3 direction = defaultCamera.transform.forward;
        direction.y = 0;
        direction.Normalize();
        defaultCamera.transform.Translate(direction * (Time.unscaledDeltaTime * -panSpeed), Space.World);
    }
    
    void PanCameraUp()
    {
        Vector3 direction = defaultCamera.transform.forward;
        direction.y = 0;
        direction.Normalize();
        defaultCamera.transform.Translate(direction * (Time.unscaledDeltaTime * panSpeed), Space.World);
    }

    void RotateCameraLeft()
    {
        float newAngle = defaultCamera.transform.eulerAngles.y + (rotateSpeed * Time.unscaledDeltaTime);
        
        defaultCamera.transform.rotation = Quaternion.Euler(defaultCamera.transform.rotation.eulerAngles.x, newAngle, 0);
    }

    void RotateCameraRight()
    {
        float newAngle = defaultCamera.transform.eulerAngles.y - (rotateSpeed * Time.unscaledDeltaTime);
        
        defaultCamera.transform.rotation = Quaternion.Euler(defaultCamera.transform.rotation.eulerAngles.x, newAngle, 0);
    }

    void ZoomCamera()
    {
        if (scrollDirection > 0)
        {
            if(defaultCamera.transform.position.y + defaultCamera.transform.forward.y * (zoomSpeed * Time.unscaledDeltaTime) > zoomInMax) 
                defaultCamera.transform.position += defaultCamera.transform.forward * (zoomSpeed * Time.unscaledDeltaTime);
            else
                defaultCamera.transform.position = new Vector3(defaultCamera.transform.position.x, zoomInMax, defaultCamera.transform.position.z);
            
            // if(defaultCamera.transform.position.y < defaultYPos)
            //     defaultCamera.transform.position = new Vector3(defaultCamera.transform.position.x, defaultYPos, defaultCamera.transform.position.z);
        }
        if (scrollDirection < 0)
        {
            //defaultCamera.transform.position -= defaultCamera.transform.forward * (zoomSpeed * Time.deltaTime);
            
            if(defaultCamera.transform.position.y - defaultCamera.transform.forward.y * (zoomSpeed * Time.unscaledDeltaTime) < zoomOutMax) 
                defaultCamera.transform.position -= defaultCamera.transform.forward * (zoomSpeed * Time.unscaledDeltaTime);
            else
                defaultCamera.transform.position = new Vector3(defaultCamera.transform.position.x, zoomOutMax, defaultCamera.transform.position.z);
        }
    }
    
    #endregion
    
    #region --Input Listeners --
    
    public void PanLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPanningLeft = true;
            RemoveTrackingObject();
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
            RemoveTrackingObject();
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
            RemoveTrackingObject();
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
            RemoveTrackingObject();
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
            isRotatingLeft = true;
        }
        else if (context.canceled)
        {
            isRotatingLeft = false;
        }
    }
    
    public void RotateRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRotatingRight = true;
        }
        else if (context.canceled)
        {
            isRotatingRight = false;
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

    public void FocusTarget(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SwitchCamera();
        }
    }
    
    #endregion

    void SwitchCamera()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, cameraLayerMask))
        {
            if (hit.collider != null)
            {
                if (currentFocus != hit.collider.gameObject)
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Weapon Platform") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Resource Station"))
                    {
                        if (hit.collider.gameObject.CompareTag("Boss"))
                        {
                            closeupCamera.gameObject.SetActive(false);
                            defaultCamera.gameObject.SetActive(false);
                            bossCamera.gameObject.SetActive(true);
                        
                            currentFocus = hit.collider.gameObject;
                            bossCamera.Follow = hit.collider.transform;
                        }
                        else
                        {
                            closeupCamera.gameObject.SetActive(true);
                            defaultCamera.gameObject.SetActive(false);
                        
                            currentFocus = hit.collider.gameObject;
                            closeupCamera.Follow = hit.collider.transform;
                        }
                    }
                    else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Celestial Body"))
                    {
                        closeupCamera.gameObject.SetActive(false);
                        defaultCamera.gameObject.SetActive(true);
                        
                        currentFocus = hit.collider.gameObject;
                        defaultCamera.Follow = hit.collider.transform;
                    }
                }
            }
        }
    }

    public void ZoomOnBoss(Transform boss)
    {
        closeupCamera.gameObject.SetActive(false);
        defaultCamera.gameObject.SetActive(false);
        bossCamera.gameObject.SetActive(true);
        
        bossCamera.Follow = boss;

        StartCoroutine(ZoomOnBossRoutine());
    }

    IEnumerator ZoomOnBossRoutine()
    {
        yield return new WaitForSeconds(5);
        
        RemoveTrackingObject();
    }
    
    public bool IsObjectInView(Transform target)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(target.position);

        return (viewPos.x >= 0 && viewPos.y >= 0) && (viewPos.x <= 1 && viewPos.y <= 1) && viewPos.z >= 0;
    }

    public void RemoveTrackingObject()
    {
        if (defaultCamera.gameObject.activeSelf)
        {
            if (defaultCamera.Follow != null)
            {
                defaultCamera.Follow = null;
                currentFocus = null;
                defaultCamera.transform.position = new Vector3(defaultCamera.transform.position.x,
                    defaultCamera.transform.position.y, defaultCamera.transform.position.z);
            }
        }
        else if (closeupCamera.gameObject.activeSelf)
        {
            if (closeupCamera.Follow != null)
            {
                closeupCamera.Follow = null;
                currentFocus = null;
                defaultCamera.transform.position = new Vector3(closeupCamera.transform.position.x,
                    closeupCamera.transform.position.y, closeupCamera.transform.position.z);
                
                closeupCamera.gameObject.SetActive(false);
                defaultCamera.gameObject.SetActive(true);
            }
        }
        else if (bossCamera.gameObject.activeSelf)
        {
            if (bossCamera.Follow != null)
            {
                bossCamera.Follow = null;
                currentFocus = null;
                
                bossCamera.gameObject.SetActive(false);
                closeupCamera.gameObject.SetActive(false);
                defaultCamera.gameObject.SetActive(true);
            }
        }
    }
}
