using UnityEngine;
using System.Collections;

public class ShakeCamera : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform

    public Transform camTrans;

    // How long the object should shake for.
    public float Duration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.01f;
    public float decreaseFactor = 1.0f;

    public bool shook = true;

    Vector3 originalPos;

    private void Update()
    {
        GameObject pacman = GameObject.FindGameObjectWithTag("Pacman");
        if(Vector3.Distance(this.transform.position, pacman.transform.position) < 200 && Time.timeSinceLevelLoad>8 && shook==false)
        {
            originalPos = camTrans.localPosition;
            shakeCam();
            shook = true;
        }
        else
        {
            resetCam();
            shook = false;
        }
        
    }

    void shakeCam()
    {
        camTrans.localPosition =  originalPos + (Random.insideUnitSphere * shakeAmount * 0.3f);


    }

    void resetCam()
    {
        camTrans.transform.localPosition = new Vector3(0, 4.65034f, -7.0998f);
        camTrans.transform.localEulerAngles = new Vector3(20f, 0, 0);
        
        //camTrans.transform.LookAt(this.transform.position);
    }
}