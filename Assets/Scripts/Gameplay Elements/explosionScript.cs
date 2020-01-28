using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
public class explosionScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Car;

    //Either player can hit or bullet can hit to trigger the Explosion.
    public bool playerHit = false;
    private CarController carscript;
    void Start()
    {
        //If player hit 
        if (playerHit)
        {
            Car = GameObject.Find("Car");
            carscript = Car.GetComponent<CarController>();
            carscript.exploded = true;
        }
        StartCoroutine(Die());
    }
    //Kill 
    IEnumerator Die()
    {          
        yield return new WaitForSeconds(2);
        if (playerHit)
        {
            carscript.exploded = false;
        }
        Destroy(this.gameObject);
    }
 
}
