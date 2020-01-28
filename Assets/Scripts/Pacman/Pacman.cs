using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Vehicles.Car;
//Pacman Script is in charge of telling the Pac man to follow the path,
// and controls its acceleration & speed. Additionally, it also includes
// the game over cut scene upon collision with the player. 
public class Pacman : MonoBehaviour
{
    
    List<Arc> tracklist;
    Arc currAim;
    public AudioClip deathSound;
    public AudioSource audioSource;
    public float speed;
    public float MAX_SPEED = 40f;
    public GameObject playerDeath;
    public float acceleration = 0.005f;
    private Text finalScore;
    public float accel;
    public float EPSILON { get; private set; }
    private Camera reverseCam;
    private Camera mainCam;
    private GameObject player;
    private Text gameOver;
    void Awake()
    {
        Track track = GameObject.Find("Track").GetComponent<Track>();
        speed = 1f;

        reverseCam = GameObject.Find("PacCam").GetComponent<Camera>();
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.Find("Car");
        gameOver = GameObject.Find("GameOverText").GetComponent<Text>();
        finalScore = GameObject.Find("finalScore").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad > 5 && speed< MAX_SPEED)
        {
            speed += acceleration;
        }
        

        tracklist = GameObject.Find("Track").GetComponent<Track>().track;
        float step = speed * Time.deltaTime; // calculate distance to move
        currAim = tracklist[0];
        Transform sphere = currAim.transform.GetChild(1);

        if (System.Math.Abs(transform.position.x - sphere.position.x) < 0.01f && System.Math.Abs(transform.position.z - sphere.position.z) < 0.01f)
        {
            //Debug.Log("YES");
            tracklist[0].onDestroy();
            tracklist.RemoveRange(0, 1);
            currAim = tracklist[0];
            sphere = currAim.transform.GetChild(1);
        }        
        
        //turn towards target
        transform.LookAt(sphere);

        //move towards target
        transform.position = Vector3.MoveTowards(transform.position, sphere.position, step);
        if (Time.timeSinceLevelLoad > 5 && speed< MAX_SPEED)
        {
        speed += accel * Time.deltaTime;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered");
        if (other.gameObject.tag == "Player")
        {
            Instantiate(playerDeath, other.transform.position, other.transform.rotation);
                            //Enable the second Camera
            reverseCam.enabled = true; 
            mainCam.enabled = false; 
            Destroy(player);
            gameOver.text = "GAME OVER";
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = deathSound;
            audioSource.Play();
            audioSource.loop = false;
            finalScore.text = "Final Score: " + (GameObject.Find("ScoreKeeper").GetComponent<Timer>().newtime);
            StartCoroutine(Exit());
        }
    }
    IEnumerator Exit()
    {          
        yield return new WaitForSeconds(5); 
        SceneManager.LoadScene("GameOver"); 
    }

}
