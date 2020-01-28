/* This code is adapted from the video: https://www.youtube.com/watch?v=1HV8GbFnCik,
 * mainly the Generate and the DiamondSquare methods*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondSquareTerrain : MonoBehaviour
{

    public int Divisions;
    public float Size;
    public float mHeight;
    private float initHeight;
    Vector3[] Verts;
    int VertCount;
    Color[] colors;
     

    public Shader shader;
    


    // Start is called before the first frame update
    void Start()
    {
        initHeight = mHeight;
        MeshFilter cubeMesh = this.gameObject.AddComponent<MeshFilter>();
        cubeMesh.mesh = this.Generate();

 

        //Generate Terrain
        Mesh generatedMesh = cubeMesh.mesh;

        //Mesh collider needs  to be added after creating mesh or else 
        //    collider will be in the shape of the initial plane
        gameObject.AddComponent<MeshCollider>();
        gameObject.GetComponent<MeshCollider>().sharedMesh = generatedMesh;
    }

    private void Update()
    { 
         
    }

    //Procedurally Generates terrain, and returns a Mesh object of corresponding terrain.
    //this is adapted from https://www.youtube.com/watch?v=1HV8GbFnCik
    Mesh Generate()
    {
        //calculate the number of verts required for the plane
        VertCount = (Divisions + 1) * (Divisions + 1); //2^n +1
        Verts = new Vector3[VertCount];
        Vector2[] uvs = new Vector2[VertCount];

        //Colors array is used to create gradient map. 
        Color[] colors = new Color[VertCount];
        int[] tris = new int[Divisions * Divisions * 6];

        //keeps track of half the size of the plane for ease of use
        float halfSize = Size * 0.5f;
        //the size of each division
        float divisionSize = Size / Divisions;

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        Bounds bounds = mesh.bounds;

        //keep track of which triangle we're up to
        int triOffset = 0;
        //calculate the squares for the plane of the terrain
        for (int i = 0; i <= Divisions; i++)
        {
            for (int j = 0; j <= Divisions; j++)
            {
                Verts[i * (Divisions + 1) + j] = new Vector3(-halfSize + j * divisionSize, 0.0f, halfSize - i * divisionSize);
                uvs[i * (Divisions + 1) + j] = new Vector2((float)i / Divisions, (float)j / Divisions);

                if (i < Divisions && j < Divisions)
                {
                    int topLeft = i * (Divisions + 1) + j;
                    int bottomLeft = (i + 1) * (Divisions + 1) + j;

                    tris[triOffset] = topLeft;
                    tris[triOffset + 1] = topLeft + 1;
                    tris[triOffset + 2] = bottomLeft + 1;

                    tris[triOffset + 3] = topLeft;
                    tris[triOffset + 4] = bottomLeft + 1;
                    tris[triOffset + 5] = bottomLeft;

                    //increments by 6 because each square is made of 6 verts
                    triOffset += 6;
                }
            }
        }

        Verts[0].y = Random.Range(-mHeight, mHeight);
        Verts[Divisions].y = Random.Range(-mHeight, mHeight);
        Verts[Verts.Length - 1].y = Random.Range(-mHeight, mHeight);
        Verts[Verts.Length - 1 - Divisions].y = Random.Range(-mHeight, mHeight);

        //Diamond square algorithm
        //travers over the verts
        //adapted from https://www.youtube.com/watch?v=1HV8GbFnCik
        int iterations = (int)Mathf.Log(Divisions, 2);
        int numSquares = 1;
        int squareSize = Divisions;
        for (int i = 0; i < iterations; i++)
        {
            int row = 0;
            for (int j = 0; j < numSquares; j++)
            {

                int col = 0;
                for (int k = 0; k < numSquares; k++)
                {
                    DiamondSquare(row, col, squareSize, mHeight);
                    col += squareSize;

                }
                row += squareSize;
            }
            numSquares *= 2;
            squareSize /= 2;
            mHeight *= 0.5f;
        }

  
        
        //assign the verts, uvs and triangles to the mesh
        mesh.vertices = Verts;
        mesh.uv = uvs;
        mesh.triangles = tris;
         

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        return mesh;
    }

    //randomise the heights as per diamond square algorithm
    void DiamondSquare(int row, int col, int size, float offset)
    {
        int halfsize = (int)(size * 0.5f);
        int topLeft = row * (Divisions + 1) + col;
        int botLeft = (row + size) * (Divisions + 1) + col;

        //square step, average the heghts of the 4 corners of the square 
        int mid = (int)(row + halfsize) * (Divisions + 1) + (int)(col + halfsize);
        Verts[mid].y = (Verts[topLeft].y + Verts[topLeft + size].y + Verts[botLeft].y + Verts[botLeft + size].y) * 0.25f + Random.Range(-offset, offset);

        //diamond step, average the heights of the 3 corners of the diamond
        Verts[topLeft + halfsize].y = (Verts[topLeft].y + Verts[topLeft + size].y + Verts[mid].y) / 3 + Random.Range(-offset, offset);
        Verts[mid - halfsize].y = (Verts[topLeft].y + Verts[botLeft].y + Verts[mid].y) / 3 + Random.Range(-offset, offset);
        Verts[mid + halfsize].y = (Verts[topLeft + size].y + Verts[botLeft + size].y + Verts[mid].y) / 3 + Random.Range(-offset, offset);
        Verts[botLeft + halfsize].y = (Verts[botLeft].y + Verts[botLeft + size].y + Verts[mid].y) / 3 + Random.Range(-offset, offset);


    }


}
