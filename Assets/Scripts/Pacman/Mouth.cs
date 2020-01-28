using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : MonoBehaviour
{
    public Renderer rend;
    public float frequency;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        // Find out whether current second is odd or even
        bool oddeven = Mathf.FloorToInt(Time.time * frequency) % 2 == 0;

        // Enable renderer accordingly
        rend.enabled = oddeven;
    }
}
