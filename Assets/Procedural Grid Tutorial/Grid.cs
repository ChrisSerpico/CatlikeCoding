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

        // initialize vertices
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                yield return wait;
            }
        }
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
