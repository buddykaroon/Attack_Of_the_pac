using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boostScript : MonoBehaviour
{ 
    //On hit Boost
    void OnTriggerEnter(Collider other)
    {
        //Boost player
        if (other.gameObject.tag == "Player")
        {
            UnityStandardAssets.Vehicles.Car.CarController pi = other.GetComponentInParent<UnityStandardAssets.Vehicles.Car.CarController>();
            pi.boosted = true;
            pi.boost_duration = 500;
            Destroy(this.gameObject);
        }
        //Kill power up
        if (other.gameObject.tag == "Pacman")
        {
            Destroy(this.gameObject);
        }
    } 
}
