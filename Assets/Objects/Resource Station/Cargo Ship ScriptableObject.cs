using UnityEngine;

[CreateAssetMenu(fileName = "Cargo Ship", menuName = "Cargo Ship")]
public class CargoShipScriptableObject : ScriptableObject
{
    public int moveSpeed;
    public float moveDelay;
    public float turnSpeed;
    public int maxHealth;
    public int resourceAmount;
    public GameObject explodePrefab;
    public LayerMask resourceStationLayerMask;
}
