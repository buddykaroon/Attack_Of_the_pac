using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public GameObject Car;
    public float waveIntensity = 10.0f;
    private float gradualWave = 10.0f;
    private Color gradualColor;
    private int bombtimer = 0;
    private bool hitBomb = false;
    public UnityStandardAssets.Vehicles.Car.CarController carScript;
    // Start is called before the first frame update
    void Start()
    {
        carScript = Car.GetComponent<UnityStandardAssets.Vehicles.Car.CarController>();
        
    }

    // Update is called once per frame
    void Update()
    { 

        if (carScript.boosted)
        {
            waveIntensity =  carScript.boost_duration_max/(carScript.boost_duration + 1);
            gradualColor = new Color(0, carScript.boost_duration/carScript.boost_duration_max, 
            1 - carScript.boost_duration/carScript.boost_duration_max
            ,1);
        }
        else
        {
            waveIntensity = 10f; 
            gradualColor = Color.blue;
        } 
        if (carScript.exploded && !hitBomb)
        {
            hitBomb = true;
            bombtimer = 100;
        }
        if (hitBomb)
        { 
            waveIntensity = 80.0f/ (bombtimer+1.0f);
            
            if (carScript.boosted)
            {
                gradualColor = new Color( bombtimer/100.0f,1 - bombtimer/100.0f, 0,1);
            }
            else
            {
                gradualColor = new Color( bombtimer/100.0f, 0, 1 - bombtimer/100.0f,1);
            }
            //Debug.Log(bombtimer);
            //Debug.Log(gradualColor);
            bombtimer--; 
            if (bombtimer <= 0)
            {
                hitBomb = false;
            }
        } 
        this.gameObject.GetComponent<Renderer>().material.SetColor("_FrontColor", gradualColor);
         
        
        this.transform.position = new Vector3(Car.transform.position.x, this.transform.position.y, Car.transform.position.z);
        Shader.SetGlobalFloat("waveIntensity", waveIntensity);
    }
}
