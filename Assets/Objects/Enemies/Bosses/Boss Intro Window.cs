using System;
using UnityEngine;

public class BossIntroWindow : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 0;
        
    }

    private void Update()
    {
        //transform.LookAt(Camera.main.transform);
    }

    public void CloseWindow()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
