using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityStandardAssets.Vehicles.Car;
public class BoostParticle : MonoBehaviour
{
    private ParticleSystem ps;
    public bool moduleEnabled = false;

    public GameObject player;
    private CarController cs;
    
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        cs = player.GetComponent<CarController>();
        //Starts off
        var emission = ps.emission;
        emission.enabled = false;
    }

    void Update()
    {
        if (cs.boosted)
        {
            moduleEnabled = true;
            TurnOn();
        }
        else
        {
            moduleEnabled = false;
            TurnOff();
        }
    }

    void TurnOff()
    {
        var emission = ps.emission;
        emission.enabled = false;
    } 
    void TurnOn()
    {
        var emission = ps.emission;
        emission.enabled = true;
    } 
}
