using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pacScript : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			UnityStandardAssets.Vehicles.Car.CarController pi = other.GetComponentInParent<UnityStandardAssets.Vehicles.Car.CarController>();
			pi.pacForm = true;
			pi.pac_duration = 500;
			Destroy(this.gameObject);
		}
		if (other.gameObject.tag == "Pacman")
		{
			Destroy(this.gameObject);
		}
	}
	// Update is called once per frame
	void Update()
	{

	}
}
