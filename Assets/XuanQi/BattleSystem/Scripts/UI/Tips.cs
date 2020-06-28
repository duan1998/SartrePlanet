using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tips : MonoBehaviour
{
    public KeyCode NeededKey;
    private void Awake()
    {
        Time.timeScale = 0;
    }
    private void Update()
    {
        if(Input.anyKeyDown)
        {
            if (NeededKey != KeyCode.None)
            {
                if (Input.GetKeyDown(NeededKey))
                    gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}
