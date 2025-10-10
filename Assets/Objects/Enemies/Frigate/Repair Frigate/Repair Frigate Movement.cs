using UnityEngine;

public class RepairFrigateMovement : MonoBehaviour
{
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private int standoffRange;
    [SerializeField] private int raycastRange;

    private GameObject target;
    Vector3 targetPos;
    Rigidbody rigidbody;
    
    private bool isDead;
    private bool isMoving = true;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(!isDead && target == null)
        {
            SetNewTarget();
        }

        if (!isDead & target != null && isMoving)
        {
            CheckDistanceToTarget();
        }
    }

    private void FixedUpdate()
    {
        if (!isDead && target != null && isMoving)
        {
            MoveTowardsTarget();
            RotateTowardsTarget();
        }
    }

    public void SetDead()
    {
        isDead = true;
    }

    void SetNewTarget()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, Mathf.Infinity, enemySO.targetLayer);
        
        if (possibleTargets.Length > 0)
        {
            float closestEnemy = Mathf.Infinity;

            for (int x = 0; x < possibleTargets.Length; x++)
            {
                if(possibleTargets[x].transform.root.gameObject == gameObject)
                    continue;
                    
                float distanceToEnemy =
                    Vector3.Distance(possibleTargets[x].transform.position, transform.position);
                
                if (distanceToEnemy < closestEnemy)
                {
                    if (possibleTargets[x].transform.root.gameObject.TryGetComponent<IRepairable>(out IRepairable targetShip))
                        if(targetShip.GetHealth() < 1)
                        {
                            closestEnemy = distanceToEnemy;
                            // target = possibleTargets[x].transform.root.gameObject;
                            target = possibleTargets[x].transform.root.gameObject;
                        }
                }
            }
        }
    }
    
    bool IsLoSClear(Vector3 pos)
    {
        // if (!Physics.Raycast(raycastOrigin.position, pos - raycastOrigin.position, out RaycastHit hit, raycastRange))
        // {
        //     return true;
        // }
        //
        // return false;
        
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.transform.forward * raycastRange, out RaycastHit hit, raycastRange))
        {
            return false;
        }
        
        return true;
    }
    
    void MoveTowardsTarget()
    {
        rigidbody.MovePosition(transform.position + transform.forward * (enemySO.moveSpeed * Time.deltaTime));
    }

    void RotateTowardsTarget()
    {
        Vector3 targetVector = target.transform.position - transform.position;
        targetVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetVector);

        rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation,
            enemySO.turnSpeed * Time.deltaTime));
    }

    void CheckDistanceToTarget()
    {
        if (Vector3.Distance(transform.position, target.transform.position) <= standoffRange)
            isMoving = false;
    }

    void MoveForward()
    {
        rigidbody.MovePosition(transform.position + transform.forward * (enemySO.moveSpeed * Time.deltaTime));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, standoffRange);
    }
}
