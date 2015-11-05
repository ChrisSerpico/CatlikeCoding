using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour 
{
    // Components that make up the fractal
    public Mesh[] meshes;
    public Material material;

    // how deep this fractal is allowed to go 
    // ie how many generations of children it can have
    public int maxDepth;

    // the depth of this fractal
    private int depth;

    // how much children of this fractal will be scaled by
    public float childScale;

    // how likely this fractal is to spawn children
    public float spawnProbability;

    // maximum possible rotation speed
    public float maxRotationSpeed;

    // the current rotation speed 
    private float rotationSpeed;

    // how far a mesh can be twisted out of its 'correct' position
    public float maxTwist;

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

    // variously colored materials, used to differentiate children
    private Material[,] materials;

    // initialize materials, assigning them different colors
    private void InitializeMaterials()
    {
        materials = new Material[maxDepth + 1, 2];

        for (int i = 0; i <= maxDepth; i++)
        {
            float t = i / (maxDepth - 1f);
            t *= t;
            materials[i, 0] = new Material(material);
            materials[i, 0].color = Color.Lerp(Color.white, Color.yellow, t);
            materials[i, 1] = new Material(material);
            materials[i, 1].color = Color.Lerp(Color.white, Color.cyan, t);
        }
        materials[maxDepth, 0].color = Color.magenta;
        materials[maxDepth, 1].color = Color.red;
    }

    private void Start()
    {
        // set rotation speed
        rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);

        // twist transform
        transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);

        // initialize materials if they haven't been
        if (materials == null)
        {
            InitializeMaterials();
        }
        gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
        gameObject.AddComponent<MeshRenderer>().material = materials[depth, Random.Range(0, materials.GetLength(1))];

        // only create a child fractal if we aren't too deep
        if (depth < maxDepth)
        {
            StartCoroutine(CreateChildren());
        }
    }

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    // create multiple children in multiple different directions
    private IEnumerator CreateChildren()
    {
        for (int i = 0; i < childDirections.Length; i++)
        {
            if (Random.value < spawnProbability)
            {
                // pause before creating a child, so the user can see each child being added
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
                new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
            }
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
        meshes = parent.meshes;
        materials = parent.materials;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;
        spawnProbability = parent.spawnProbability;
        maxRotationSpeed = parent.maxRotationSpeed;
        maxTwist = parent.maxTwist;

        // make this fractal a child of parent
        transform.parent = parent.transform;

        // scale and move child
        transform.localScale = Vector3.one * childScale;
        transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);

        // rotate the child
        transform.localRotation = childOrientations[childIndex];
    }
}
