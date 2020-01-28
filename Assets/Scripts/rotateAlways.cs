using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateAlways : MonoBehaviour
{
    public float rotation = 50;
    //Rotate an object based on rotation speed.
    void Update()
    {
            transform.Rotate (0,rotation*Time.deltaTime,0); //rotates 50 degrees per second around z axis

    }
}
