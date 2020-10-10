using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    void Awake() {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 30;
        Cursor.visible = false;
    }
    // Start is called before the first frame update
    void Start()
    {                
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }  
    }
}
