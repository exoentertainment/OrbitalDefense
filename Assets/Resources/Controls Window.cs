using UnityEngine;

public class ControlsWindow : MonoBehaviour
{
    public void CloseWindow()
    {
        GameObject window = (GameObject)Instantiate(Resources.Load("Pause Menu"));
        Destroy(gameObject);
    }
}
