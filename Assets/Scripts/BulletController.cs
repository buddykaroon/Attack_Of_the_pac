using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    //public Vector3 velocity;
    public float movementSpeed = 100;
    //public GameObject car=GameObject.FindGameObjectWithTag("Player");
	IEnumerator Kill()
	{
		yield return new WaitForSeconds(2);
		Destroy(this.gameObject);
	}
    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(Kill());
    }

    // Update is called once per frame
    void Update()
    {
        
        //velocity = car.transform.position;
        //this.transform.Translate(velocity * Time.deltaTime);movementSpeed
        this.transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        
    }
}
