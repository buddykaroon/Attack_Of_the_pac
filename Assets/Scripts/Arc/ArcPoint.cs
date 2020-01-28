using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* this code is adapted from http://www.jakecaspick.com/post/endlessroad/?fbclid=IwAR3z_UHz15fQohIkY6tSgBI6VLrnCgbXhqNJS6u6vl8Rl6A8P3y2pC-mDmQ
*/
public class ArcPoint : MonoBehaviour
{
    public float t, d;

    public void Arcarcpoint(float t, float d)
    {
        this.t = t;
        this.d = d;
    }

    public void Set(float newt, float newd)
    {
        t = newt;
        d = newd;
    }
}
