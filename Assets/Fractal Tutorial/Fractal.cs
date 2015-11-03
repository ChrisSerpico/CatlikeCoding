using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour 
{
    // Components that make up the fractal
    public Mesh mesh;
    public Material material;

    // how deep this fractal is allowed to go 
    // ie how many generations of children it can have
    public int maxDepth;

    // the depth of this fractal
    private int depth;

    // how much children of this fractal will be scaled by
    public float childScale;

    // the directions in which children are added
    private static Vector3[] childDirections = 
    {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };

    // the orientations at which each direction of child is initialized
    private static Quaternion[] childOrientations =
    {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f)
    };

    private void Start()
    {
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = material;

        // only create a child fractal if we aren't too deep
        if (depth < maxDepth)
        {
            StartCoroutine(CreateChildren());
        }
    }

    // create multiple children in multiple different directions
    private IEnumerator CreateChildren()
    {
        for (int i = 0; i < childDirections.Length; i++)
        {
            // pause before creating a child, so the user can see each child being added
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
        }
    }

    // initialize this fractal by copying data from its parent
    // direction is the vector pointing towards where we want to child to move relative to its parent
    // orientation is how the child is rotated relative to its parent
    // both of these are defined by childIndex, which refers to the direction and orientation
    // in childDirection and childOrientation, respectively
    private void Initialize (Fractal parent, int childIndex)
    {
        // initialize values
        mesh = parent.mesh;
        material = parent.material;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;

        // make this fractal a child of parent
        transform.parent = parent.transform;

        // scale and move child
        transform.localScale = Vector3.one * childScale;
        transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);

        // rotate the child
        transform.localRotation = childOrientations[childIndex];
    }
}
