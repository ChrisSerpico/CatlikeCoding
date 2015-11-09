using UnityEngine;
using System.Collections;

// Generate a simple rectangular grid

// Have unity automatically add a MeshFilter and a MeshRenderer to any object
// this script is attached to (if it doesn't already have them)
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class Grid : MonoBehaviour {

    // Size of the grid to generate
    public int xSize, ySize;

    // set of vertices that define the grid
    private Vector3[] vertices;

    // variable for the mesh we are going to generate
    private Mesh mesh;

    // Called when we enter play mode
    private void Awake()
    {
        StartCoroutine(Generate());
    }

    // generate the grid
    private IEnumerator Generate()
    {
        // time paused in-between drawing each vertex
        WaitForSeconds wait = new WaitForSeconds(0.05f);

        // setup mesh, set it as the component's mesh
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        // initialize vertices
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];

        // create a uv array
        Vector2[] uv = new Vector2[vertices.Length];

        // create a tangent array and vector
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        
        // generate vertices and uv locations
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                uv[i] = new Vector2((float) x / xSize, (float) y / ySize);
                tangents[i] = tangent;
            }
        }

        // add the newly created vertices to the mesh
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;

        // generate triangles for the mesh
        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
                mesh.triangles = triangles;
                yield return wait;
            }
        }

        // use the vertices and triangles to calculate normals
        mesh.RecalculateNormals();
    }

    // draw gizmos on screen so we can see the vertices
    // gizmos are essentially sprites drawn on-screen in the scene view rather than
    // the play view
    private void OnDrawGizmos()
    {
        // only draw gizmos if vertices is defined
        if (vertices == null)
        {
            return;
        }

        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
