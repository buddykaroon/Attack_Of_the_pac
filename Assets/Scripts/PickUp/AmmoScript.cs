using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            BulletManager pi = other.GetComponentInParent<BulletManager>();
            pi.ammo += 10;  
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
