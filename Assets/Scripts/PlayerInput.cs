using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //Changed to current speed, a private variable affected by speed.
    private float curr_speed;
    public bool boosted = false;
    public float speed = 0.1f;
    private float original_speed;
    //public Camera camera;
    public int boost_duration = 500;
    public float boost_multiplier = 2;

    float m_FieldOfView;
    float m_FieldOfView_boosted = 140.0f;
    // Start is called before the first frame update
    void Start()
    {
        m_FieldOfView = 60.0f;
        curr_speed = 0.0f;
        original_speed = speed;
    } 
    // Update is called once per frame
    void Update()
    {
        //Start of boost
        if (boosted)
        {
            speed = original_speed + ((boost_duration)/300);
            Camera.main.fieldOfView = (boost_duration/25) + m_FieldOfView;
            boost_duration--;
        }
        //End of boost
        if (boosted && boost_duration <= 0)
        {
            boosted = false;
            boost_duration = 500;
            speed = original_speed;
            Camera.main.fieldOfView = m_FieldOfView;
        }
        if(Input.GetKey(KeyCode.UpArrow)) {
            if(curr_speed < 0) {
                curr_speed /= 1.05f;
            }
            curr_speed += speed;
        } else if(Input.GetKey(KeyCode.DownArrow)) {
            if(curr_speed > 0) {
                curr_speed /= 1.05f;
            }
            curr_speed -= speed;
        } if (Input.GetKey(KeyCode.RightArrow)) {
            //currently shifting the worlds euler angles instead of it's own
            this.transform.localEulerAngles += Vector3.up;
        } else if (Input.GetKey(KeyCode.LeftArrow)) {
            this.transform.localEulerAngles += Vector3.down;
        }else {
            curr_speed /= 1.02f;
        }
        this.transform.Translate(Vector3.forward * curr_speed * Time.deltaTime);
    }
}
