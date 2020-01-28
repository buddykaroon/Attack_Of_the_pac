using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;


public class enemyScript : MonoBehaviour
{
    // Start is called before the first frame update
	public GameObject Player;
	public GameObject explosion; 

    public float speed = 10f;
    void Start()
    { 
        Player = GameObject.Find("Car");
    }

    // Update is called once per frame
    void Update()
	{
		Vector3 lookPos = Player.transform.position - transform.position;
		lookPos.y = 0;
		Quaternion rotation = Quaternion.LookRotation(lookPos);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime);


    }  
	void OnTriggerEnter(Collider other)
	{ 
		if (other.gameObject.tag == "Player")
		{ 
			Instantiate(explosion, transform.position, transform.rotation);
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
		if (other.gameObject.tag == "Pacman")
		{
			Destroy(this.gameObject);
		}
		if (other.gameObject.tag == "Bullet")
		{           
			Instantiate(explosion, transform.position, transform.rotation);

			Destroy(this.gameObject);
		}

	}

}
