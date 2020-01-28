using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BulletManager : MonoBehaviour
{   
    public Text countText;
    public int max_ammo = 50;
    public int ammo = 10;
    public GameObject BulletPrefab;
    public GameObject Turret1; 
    public GameObject Turret2; 

    // Update is called once per frame
    void Update()
    {       
        UpdateBullet();
        if (ammo > max_ammo)
        {ammo = max_ammo;}
        if (Input.GetKeyDown("space") && ammo > 0)
        {
            GameObject bullet = Instantiate<GameObject>(BulletPrefab, Turret1.transform.position ,Turret1.transform.rotation);
            GameObject bullet2 = Instantiate<GameObject>(BulletPrefab, Turret2.transform.position ,Turret2.transform.rotation);
            ammo--;   
        }
    }
    void UpdateBullet ()
    {
        countText.text = "Ammo: " + ammo.ToString(); 
    }

    //In event of falling down.
    public void resetAmmo()
    {
        if (ammo > 10)
        {
        ammo -= 10;
        }
        else
        {
            ammo = 0;
        }
    }
}
