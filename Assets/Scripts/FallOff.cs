using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class FallOff : MonoBehaviour
{
    public Track track;
    public CarController carController;
    Arc currArc;

    public Text distanceText;
    private float distance;

    private Vector3 prevLoc;
    private Vector3 currLoc;

    private bool fellOff = false;

    

    // Start is called before the first frame update
    void Start()
    {
        currArc = Track.instance.currArc;
        distance = 0;
        distanceText.text = "Dist:" + distance.ToString() + " metres";

        prevLoc = this.transform.position;
        currLoc = this.transform.position;

        
    }

    // Update is called once per frame
    void Update()
    {
        //currArc = track.currArc;
        //Debug.Log(currArc.transform.localPosition);
        if (this.transform.localPosition.y < -10) {
            Debug.Log(currArc.transform.localPosition);
            this.transform.localPosition = currArc.transform.localPosition + Vector3.up;
            this.transform.localEulerAngles = Vector3.zero;
            carController.m_Rigidbody.velocity = Vector3.zero;
            carController.m_Rigidbody.angularVelocity = Vector3.zero;
			carController.pacForm = true;
			carController.pac_duration = 300;
            transform.LookAt(currArc.nextArc.transform.position);
        }

        //update the current arc
        if (currArc.nextArc != null && currArc.nextArc.ContainsPoint(transform.position, 0.05f))
        {
            Track.instance.IncrementArc();
            currArc = Track.instance.currArc;
        }
        if (this.transform.localPosition.y > -0.5f)
        {
            if (fellOff == false)
            {
                distance += Mathf.Sqrt(Mathf.Pow(currLoc.x - prevLoc.x, 2) + Mathf.Pow(currLoc.z - prevLoc.z, 2));
                prevLoc = currLoc;
                currLoc = this.transform.position;
                
            }
            else
            {
                prevLoc = this.transform.position;
                currLoc = this.transform.position;
                fellOff = false;
                GameObject.FindGameObjectWithTag("Player").GetComponent<BulletManager>().resetAmmo();

            }

        }
        else
        {
            //currLoc = this.transform.position;
            fellOff = true;
            //prevLoc = currLoc;
        }

        



    }
}
