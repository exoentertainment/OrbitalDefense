using UnityEngine;

public interface IRepairable
{
    float GetHealth();
    void RepairHealth(int value);
}
