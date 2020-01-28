using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
public class mineTrigger : MonoBehaviour
{ 
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerEnter(Collider other)
    { 
        //On collision with player
        if (other.gameObject.tag == "Player")
        { 
            GameObject explode = Instantiate(explosion, transform.position, transform.rotation);
            explode.GetComponent<explosionScript>().playerHit = true;
			GameObject car = GameObject.Find("Car");
			if (!car.GetComponent<CarController>().pacForm)
			{
				int magnitude = 2500000;
				Vector3 force = transform.position - other.transform.position;
				force = force.normalized;
				Rigidbody rb = other.GetComponentInParent<Rigidbody>();
				rb.AddForce(-force * magnitude);	
			}

            Destroy(this.gameObject);
        }
        //On collision with pacman
        if (other.gameObject.tag == "Pacman")
        {
            Destroy(this.gameObject);
		}
        //On collision with bullet.
		if (other.gameObject.tag == "Bullet")
		{           
			Instantiate(explosion, transform.position, transform.rotation);
			
			Destroy(this.gameObject);
		}

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
