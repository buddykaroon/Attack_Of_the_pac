using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public Transform Player;
    public Camera NormalCam, ReverseCam;
    public KeyCode TKey;
    public bool camSwitch = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            
            //camSwitch2 = !camSwitch2;
            //NormalCam.gameObject.SetActive(camSwitch1);
            ReverseCam.gameObject.SetActive(camSwitch);
            camSwitch = !camSwitch;
        }
        /*else
        {
            NormalCam.gameObject.SetActive(true);
            ReverseCam.gameObject.SetActive(false);
        }*/
    }
}
