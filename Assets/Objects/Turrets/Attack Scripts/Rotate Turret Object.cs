using UnityEngine;
using UnityEngine.Serialization;

public class RotateTurretObject : MonoBehaviour
{
    [SerializeField] private Transform rotatingObject;
    [FormerlySerializedAs("platformSO")] [SerializeField] protected TurretSO turretSO;
    [SerializeField] private float minRotation;
    [SerializeField] float maxRotation;
    
    float currentAngle;
    GameObject target;
    
    protected void Update()
    {
        if (target != null)
        {
            if (target.activeSelf)
            {
                RotateTowardsTarget();
            }
        }
    }
    
    void RotateTowardsTarget()
    {
        Vector3 dir = transform.InverseTransformDirection(target.transform.position - transform.position);
        dir.y = 0;
        
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        targetAngle = Mathf.Clamp(targetAngle, minRotation, maxRotation);
        currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, turretSO.baseTrackingSpeed * Time.deltaTime);
        rotatingObject.localEulerAngles = Vector3.up * currentAngle; 
    }
    
    public void SetTarget(GameObject target)
    {
        this.target = target;    
    }
}
