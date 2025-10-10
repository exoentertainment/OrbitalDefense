using UnityEngine;

public class spaceshower : MonoBehaviour
{
    [SerializeField] private int range;
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
        
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(transform.position, turretSO.minRange);
    }
}
