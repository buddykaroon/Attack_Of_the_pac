using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//all taken
public class Track : MonoBehaviour
{
    public static Track instance;

    public Arc arcPrefab;
    public Transform player;

    Vector3 nextArcPos = Vector3.zero;
    Vector3 nextArcHeightOffset = Vector3.zero;
    float nextArcAngle = 0;
    float nextArcUVOffset = 0;
    float textureScale = 12;

    public Arc currArc;
    private Arc furthestArc;
    public float furthestDistance = 0;
    public float slope = -0.00f;

    public List<Arc> track;
    Transform trackHolder;
    Vector3 trackYOffset = Vector3.zero;

    float minRadius = 100;
    float maxRadius = 120;
    float minAngle = 10;
    float maxAngle = 20;

    public float totalDistance = 5000;


    float meshQuality = 1f;

    //set the player to this player!
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

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    //initialise the track!
    public void InitializeTrack()
    {
        trackHolder = new GameObject("Track").transform;
        //keep a list of the track
        track = new List<Arc>();
        //add more tracks if we get too close
        while ((nextArcPos - player.position).magnitude < 1000)
        {
            AddArc();
            //Debug.Log(track.Count);
        }

        currArc = track[0];
    }
    /* this method is adapted from http://www.jakecaspick.com/post/endlessroad/?fbclid=IwAR3z_UHz15fQohIkY6tSgBI6VLrnCgbXhqNJS6u6vl8Rl6A8P3y2pC-mDmQ
    */
    void Update()
    {
        for (int i = 0; i < track.Count; i++)
        {
            if (track[i].distanceOffset + track[i].totalDistance < Manager.instance.maxDistance - 2)
            {
                track[i].transform.SetParent(null);
                track[i].ResetArc();
                track[i].gameObject.SetActive(false);
            }
        }

        if ((nextArcPos - player.position).magnitude < 1000)
        {
            AddArc();
            IncrementArc();
        }
    }
    /* this method is adapted from http://www.jakecaspick.com/post/endlessroad/?fbclid=IwAR3z_UHz15fQohIkY6tSgBI6VLrnCgbXhqNJS6u6vl8Rl6A8P3y2pC-mDmQ
    */
    void AddArc()
    {
        Arc newSegment = GetPooledArc();

        newSegment.transform.position = nextArcPos;
        newSegment.startAngle = nextArcAngle;
        newSegment.width = 40f;
        newSegment.slope = slope;

        float randomRadius = Random.Range(minRadius, maxRadius);
        float randomAngle = Random.Range(minAngle, maxAngle);
        newSegment.radius = randomRadius;
        newSegment.angle = randomAngle;
        if (Random.value > 0.5)
        {
            newSegment.flipped = true;
        }
        else
        {
            newSegment.flipped = false;
        }

        newSegment.Initialize();

        newSegment.distanceOffset = furthestDistance;
        newSegment.uvOffset = nextArcUVOffset;
        newSegment.textureScale = textureScale;

        furthestDistance += newSegment.totalDistance;
        nextArcHeightOffset.Set(0, furthestDistance * slope, 0);
        nextArcAngle = newSegment.endAngle;
        nextArcPos = newSegment.endPos;
        nextArcUVOffset += newSegment.totalDistance / textureScale - Mathf.Floor(newSegment.totalDistance / textureScale);
        if (nextArcUVOffset >= 1)
        {
            nextArcUVOffset = nextArcUVOffset - 1;
        }

        if (furthestArc != null)
        {
            furthestArc.nextArc = newSegment; // each arc contains a reference to the next arc in the track
        }
        furthestArc = newSegment;

        newSegment.arcDivisions = Mathf.RoundToInt(newSegment.totalDistance / meshQuality); // adjust mesh divisions based on the length of the arc
        newSegment.GenerateMesh();

        track.Add(newSegment);
        trackHolder.position = Vector3.zero;
        newSegment.transform.SetParent(trackHolder);
        trackHolder.position = trackYOffset;

        newSegment.gameObject.SetActive(true);
    }
    /* this method is adapted from http://www.jakecaspick.com/post/endlessroad/?fbclid=IwAR3z_UHz15fQohIkY6tSgBI6VLrnCgbXhqNJS6u6vl8Rl6A8P3y2pC-mDmQ
    */
    Arc GetPooledArc()
    {
        for (int i = 0; i < track.Count; i++)
        {
            if (!track[i].gameObject.activeInHierarchy)
            {
                return track[i];
            }
        }

        // if none is found create a new object only if the pool has not exceeded its hard cap
        Arc newSegment = Instantiate(arcPrefab) as Arc; 

        return newSegment;
    }
    /* this method is adapted from http://www.jakecaspick.com/post/endlessroad/?fbclid=IwAR3z_UHz15fQohIkY6tSgBI6VLrnCgbXhqNJS6u6vl8Rl6A8P3y2pC-mDmQ
    */
    public void OffsetHeight(float offset) // the entire track actually moves up in the Y axis, instead of the player descending.  Keeps player from straying ever further from origin.
    {
        trackYOffset.Set(0, -offset, 0);
        trackHolder.position = trackYOffset;
    }
    //update the current arc!
    public void IncrementArc()
    {
        Arc nextArc = currArc.nextArc;
        currArc = nextArc;
    }
}
