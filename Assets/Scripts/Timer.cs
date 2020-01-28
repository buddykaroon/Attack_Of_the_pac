using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour
{
    //Simple time counter. Used for scoring purposes. 
    public float time = 0;
    public Text myText; 
    public int newtime;
    void Update()
    { 
        time += Time.deltaTime; 
        newtime = (int) (System.Math.Round(time,1) * 10);
        myText.text = "Score: " + newtime.ToString();
    }
}
