using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* this method is adapted from http://www.jakecaspick.com/post/endlessroad/?fbclid=IwAR3z_UHz15fQohIkY6tSgBI6VLrnCgbXhqNJS6u6vl8Rl6A8P3y2pC-mDmQ
 */
public class Arc : MonoBehaviour
{
    public float radius;
    public float angle;
    public float width;
    public float startAngle;
    public float endAngle;
    public Vector3 endPos;
    public float totalDistance;
    public float distanceOffset;
    public float slope;
    public bool flipped = true;
    private Vector3 center;
    public int arcDivisions = 24;
    public int widthDivisions = 2;
    public float uvOffset = 0;
    public float textureScale = 4;
    public Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uv;
    public Arc nextArc;

    /* this method is adapted from http://www.jakecaspick.com/post/endlessroad/?fbclid=IwAR3z_UHz15fQohIkY6tSgBI6VLrnCgbXhqNJS6u6vl8Rl6A8P3y2pC-mDmQ
    */
    public void Initialize()
    {


        if (!flipped)
        {
            startAngle = NormalizeAngle(startAngle);
            endAngle = NormalizeAngle(startAngle + angle);
        }
        else
        {
            startAngle = NormalizeAngle(-startAngle);
            endAngle = NormalizeAngle(-startAngle - angle);
        }
        center = GetCenter();
        totalDistance = angle / 180 * Mathf.PI * radius;
        GameObject obj = new GameObject();
        ArcPoint newArc = obj.AddComponent<ArcPoint>();
        newArc.Arcarcpoint(angle + startAngle, radius);


        endPos = ArcToWorld(newArc);
    }

    internal void onDestroy()
    {
        Destroy(gameObject);
    }

    public void ResetArc()
    {
        mesh.Clear();
        distanceOffset = 0;
        transform.position = Vector3.zero;
        center = Vector3.zero;
        startAngle = 0;
        endPos = Vector3.zero;
        nextArc = null;
    }
    /* this method is adapted from http://www.jakecaspick.com/post/endlessroad/?fbclid=IwAR3z_UHz15fQohIkY6tSgBI6VLrnCgbXhqNJS6u6vl8Rl6A8P3y2pC-mDmQ
    */
    public void GenerateMesh()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        vertices = new Vector3[(arcDivisions + 1) * (widthDivisions + 1)];
        uv = new Vector2[vertices.Length];
        float arcStep = angle / arcDivisions;
        float widthStep = width / widthDivisions;
        GameObject obj1 = new GameObject();
        ArcPoint nextVertex = obj1.AddComponent<ArcPoint>();

        nextVertex.Arcarcpoint(0, 0);
        Vector2 nextUV = Vector2.zero;
        if (!flipped)
        {
            for (int t = 0; t <= arcDivisions; t++)
            {
                for (int w = 0; w <= widthDivisions; w++)
                {
                    nextVertex.Set(t * arcStep + startAngle, (w * widthStep - width / 2) + radius);
                    vertices[t * (widthDivisions + 1) + w] = ArcToLocal(nextVertex);
                    nextUV.Set((float)w / widthDivisions / textureScale, (float)t / arcDivisions * totalDistance / textureScale + uvOffset);
                    uv[t * (widthDivisions + 1) + w] = nextUV;
                }
            }
        }
        else
        {
            for (int t = 0; t <= arcDivisions; t++)
            {
                for (int w = 0; w <= widthDivisions; w++)
                {
                    nextVertex.Set(t * arcStep + startAngle, (w * -widthStep + width / 2) + radius);
                    vertices[t * (widthDivisions + 1) + w] = ArcToLocal(nextVertex);
                    nextUV.Set((float)w / widthDivisions / textureScale, (float)t / arcDivisions * totalDistance / textureScale + uvOffset);
                    uv[t * (widthDivisions + 1) + w] = nextUV;
                }
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        triangles = new int[arcDivisions * widthDivisions * 6];
        for (int ti = 0, vi = 0, t = 0; t < arcDivisions; t++, vi++)
        {
            for (int w = 0; w < widthDivisions; w++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + widthDivisions + 1;
                triangles[ti + 5] = vi + widthDivisions + 2;
                mesh.triangles = triangles;
            }
        }
        mesh.RecalculateNormals();


        //Mesh collider needs  to be added after creating mesh or else 
        //    collider will be in the shape of the initial plane
        gameObject.AddComponent<MeshCollider>();
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }
    /* this method is adapted from http://www.jakecaspick.com/post/endlessroad/?fbclid=IwAR3z_UHz15fQohIkY6tSgBI6VLrnCgbXhqNJS6u6vl8Rl6A8P3y2pC-mDmQ
    */
    public bool ContainsPoint(Vector3 point, float margin)
    {
        ArcPoint p = WorldToArc(point);
        float rp = NormalizeAngle(p.t - startAngle);
        if (rp >= 0 && rp <= angle && p.d <= radius + width / 2 + margin && p.d >= radius - width / 2 - margin)
        {
            return true;
        }
        return false;
    }
    /* this method is adapted from http://www.jakecaspick.com/post/endlessroad/?fbclid=IwAR3z_UHz15fQohIkY6tSgBI6VLrnCgbXhqNJS6u6vl8Rl6A8P3y2pC-mDmQ
    */
    public float GetAngle(Vector3 point)
    {
        float raw = Mathf.Atan2(point.z, point.x);
        if (flipped)
        {
            return -raw * Mathf.Rad2Deg + 180;
        }
        return raw * Mathf.Rad2Deg;
    }
    /* this method is adapted from http://www.jakecaspick.com/post/endlessroad/?fbclid=IwAR3z_UHz15fQohIkY6tSgBI6VLrnCgbXhqNJS6u6vl8Rl6A8P3y2pC-mDmQ
    */
    float NormalizeAngle(float angle)
    {
        // expresses negative angles and angles over 360 degrees as their equivalent angles from 0-360 degrees
        while (angle < 0)
        {
            angle += 360;
        }
        while (angle >= 360)
        {
            angle -= 360;
        }
        return angle;
    }

    public ArcPoint WorldToArc(Vector3 world)
    {
        float t = NormalizeAngle(GetAngle(new Vector3(world.x,0.0f,world.z) - new Vector3(center.x, 0.0f, center.z)));
        float d = (new Vector3(world.x, 0.0f, world.z) - new Vector3(center.x, 0.0f, center.z)).magnitude;
        GameObject obj2 = new GameObject();
        ArcPoint newArc = obj2.AddComponent<ArcPoint>();
        newArc.Arcarcpoint(t, d);
        return newArc;
    }
    public Vector3 ArcToLocal(ArcPoint arcPoint)
    {
        float tr = arcPoint.t * Mathf.Deg2Rad;
        float d = arcPoint.d;
        if (flipped)
        {
            tr = (-arcPoint.t + 180) * Mathf.Deg2Rad;
        }
        return new Vector3(d * Mathf.Cos(tr), 0, d * Mathf.Sin(tr)) + center - transform.position;
    }
    public Vector3 ArcToWorld(ArcPoint arcPoint)
    {
        float tr = arcPoint.t * Mathf.Deg2Rad;
        float d = arcPoint.d;
        if (flipped)
        {
            tr = (-arcPoint.t + 180) * Mathf.Deg2Rad;
        }
        return new Vector3(d * Mathf.Cos(tr), 0, d * Mathf.Sin(tr)) + center;
    }
    
    /* this method is adapted from http://www.jakecaspick.com/post/endlessroad/?fbclid=IwAR3z_UHz15fQohIkY6tSgBI6VLrnCgbXhqNJS6u6vl8Rl6A8P3y2pC-mDmQ
    */
    Vector3 GetCenter()
    {
        float tr = (startAngle + 180) * Mathf.Deg2Rad;
        if (flipped)
        {
            tr = -startAngle * Mathf.Deg2Rad;
        }
        return new Vector3(radius * Mathf.Cos(tr), 0f, radius * Mathf.Sin(tr)) + transform.position;
    }

    
}
