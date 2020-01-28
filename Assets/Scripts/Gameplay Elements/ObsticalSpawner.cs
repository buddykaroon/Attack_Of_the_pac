using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticalSpawner : MonoBehaviour
{
    public GameObject Boost;
    public GameObject Mine;
	public GameObject Ghost;
	public GameObject Ammo;
	public GameObject Shield;

public GameObject SuperGhost;
    private GameObject scorekeep;
    public int mine_field_spread = 20; //Minefield Spread (Distance)
    public int mine_field_max = 6; //Maximum amount of mines
    private float seed;
    private float score;
    public float threshold = 10f; 
    void Start()
    {
        scorekeep = GameObject.Find("ScoreKeeper");
        score = scorekeep.GetComponent<Timer>().time;
        int difficulty = (int) score/60;
        
        if (score < 5.0f) //If 5 seconds or below, spawn ONLY power ups.
        {
        seed = Random.Range(0.0f, 0.5f); 
        }
        else //Enemies can now spawn alongside power ups. 
        {
            seed = Random.Range(0.0f, (1.0f)); 
        }
        if (seed < 0.3f) //30% chance: Spawn ammo
        {
            SpawnAmmo();
        }
        if (seed >= 0.2f && seed <= 0.5f) // 30% chance: Spawn a boost.
        {
            SpawnBoost();
        }
        if (seed >= 0.4f && seed <= 0.5f) // 10% Chance. Spawn a Shield.
        {              
            SpawnShield();
        }
        if (seed > 0.5f) // Spawn Bad things.
		{ 
            
            float enemy = Random.Range(0.0f, 1.0f);
            if (enemy < 0.5f)
            {
                SpawnMinefield(difficulty);
            } 
            else 
            {
                SpawnGhost(difficulty);
            }

		}
    } 
    void SpawnMinefield(int difficulty)
    {
        for (int x = 0; x < (mine_field_max + difficulty); x++)
            {
                Vector3 minePosition = new Vector3(transform.position.x + Random.Range(-mine_field_spread, mine_field_spread),
                    0, transform.position.z + Random.Range(-mine_field_spread, mine_field_spread));
                Instantiate(Mine, minePosition, transform.rotation);
            }
    }
    void SpawnBoost()
    {
        
    Instantiate(Boost, new Vector3(transform.position.x + Random.Range(-mine_field_spread, mine_field_spread),
            1, transform.position.z + Random.Range(-mine_field_spread, mine_field_spread))
            , transform.rotation);
    }
    void SpawnGhost(int difficulty)
    {
        float ghostseed = Random.Range(0.0f, (1.0f + (difficulty/10))); 
        if (ghostseed < 0.9f)
        {
            Instantiate(Ghost, transform.position, transform.rotation);
        }
        else{
            Instantiate(SuperGhost, transform.position, transform.rotation);
            }
    }
    void SpawnAmmo()
    {	
        Instantiate(Ammo, new Vector3(transform.position.x + Random.Range(-mine_field_spread, mine_field_spread),
        1, transform.position.z + Random.Range(-mine_field_spread, mine_field_spread))
        , transform.rotation);
    }
    void SpawnShield()
    {
    Instantiate(Shield, new Vector3(transform.position.x + Random.Range(-mine_field_spread, mine_field_spread),
    1, transform.position.z + Random.Range(-mine_field_spread, mine_field_spread))
    , transform.rotation);
    }
}
