using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    Arc currArc;
    public PlayerInput player;

    //taken
    public float maxDistance = 0;
    float playerHeight = 0;
    public static Manager instance;
    public bool paused = true;
    


    //taken
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //taken
        Track.instance.InitializeTrack();
        currArc = Track.instance.currArc;
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
