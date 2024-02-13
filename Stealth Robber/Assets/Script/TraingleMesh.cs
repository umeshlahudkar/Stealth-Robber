using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraingleMesh : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Mesh mesh;
    public float sideLength = 2f;

    public int circleIteration = 10;

    private void Start()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        mesh = new Mesh();

        meshFilter.mesh = mesh;
        //CreateTriangle();
        //CreateQuad();
        //CreateCircle();
        //CreateRandom();
        DrawRandomCircle();
    }

    private void CreateTriangle()
    {
        Vector3[] vertices = new Vector3[3];

        vertices[0] = new Vector3(0, 1, 0);
        vertices[1] = Vector3.forward * sideLength;
        vertices[2] = new Vector3(sideLength, 0, sideLength);

        int[] traingle = new int[3];
        traingle[0] = 0;
        traingle[1] = 1;
        traingle[2] = 2;

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = traingle;

        mesh.RecalculateNormals();
    }

    private void CreateQuad()
    {
        Vector3[] vertices = new Vector3[4];

        vertices[0] = Vector3.zero;
        vertices[1] = Vector3.forward * sideLength;
        vertices[2] = new Vector3(sideLength, 0, sideLength);
        vertices[3] = Vector3.right * sideLength;

        int[] traingle = new int[6];
        traingle[0] = 0;
        traingle[1] = 1;
        traingle[2] = 2;

        traingle[3] = 0;
        traingle[4] = 2;
        traingle[5] = 3;

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = traingle;

        mesh.RecalculateNormals();
    }

    private void CreateRandom()
    {
        Vector3[] vertices = new Vector3[5];

        vertices[0] = Vector3.zero;
        vertices[1] = Vector3.forward * sideLength;
        vertices[2] = new Vector3(sideLength, 0, sideLength);
        vertices[3] = new Vector3(sideLength, 0, -sideLength);
        vertices[4] = Vector3.back * sideLength;

        int[] traingle = new int[6];
        traingle[0] = 0;
        traingle[1] = 1;
        traingle[2] = 2;

        traingle[3] = 0;
        traingle[4] = 3;
        traingle[5] = 4;

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = traingle;

        mesh.RecalculateNormals();
    }

    private void DrawRandomCircle()
    {
        Vector3[] vertices = new Vector3[5];

        vertices[0] = Vector3.zero;
        for(int i = 0; i < 4; i++)
        {
            float angle = 360 * i / 4;
            float x = Mathf.Sin(angle * Mathf.Deg2Rad) * sideLength;
            float z = Mathf.Cos(angle * Mathf.Deg2Rad) * sideLength;
            vertices[i + 1] = new Vector3(x, 0, z);
        }

        int[] triangles = new int[12];
        for(int i = 0; i < 3; i++)
        {
            triangles[i * 3] = 0;
            triangles[(i * 3) + 1] = i + 1;
            triangles[(i * 3) + 2] = i + 2;
        }

        triangles[3 * 3] = 0;
        triangles[3 * 3 + 1] = 4;
        triangles[3 * 3 + 2] = 1;

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private void CreateCircle()
    {
        List<Vector3> verticesList = new List<Vector3>();
        List<int> trianglesList = new List<int>();

        // Add center vertex
        verticesList.Add(Vector3.zero);

        // Generate vertices
        for (int i = 0; i < circleIteration; i++)
        {
            float angle = 360 * i / circleIteration;
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * sideLength;
            float z = Mathf.Sin(angle * Mathf.Deg2Rad) * sideLength;
            verticesList.Add(new Vector3(x, 0, z));
        }

        Debug.Log(verticesList.Count);

        // Generate triangles
        for (int i = 1; i <= circleIteration; i++)
        {
            trianglesList.Add(0);
            trianglesList.Add(i);
            trianglesList.Add(i % circleIteration + 1);
        }

        // Create mesh
        mesh.vertices = verticesList.ToArray();
        mesh.triangles = trianglesList.ToArray();
        mesh.RecalculateNormals();

    }
}
