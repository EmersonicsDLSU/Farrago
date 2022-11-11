using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[ExecuteInEditMode]
public class AISensor : MonoBehaviour
{
    public float distance = 10;
    public float angle = 30;
    public float height = 1.0f;
    public Color meshColor = Color.red;

    Mesh mesh;

    //how many times do we scan per frame
    public int scanFrequency = 30;
    public LayerMask layers;
    Collider[] colliders = new Collider[50];
    int count;
    float scanInterval;
    float scanTimer;
    
    private List<GameObject> objects = new List<GameObject>();
    public List<GameObject> Objects
    {
        get {
            objects.RemoveAll(obj => !obj);
            return objects; }
    }
    public LayerMask occlusionLayers;

    // Start is called before the first frame update
    void Start()
    {
        scanInterval = 1.0f / scanFrequency;
    }

    // Update is called once per frame
    public void update(AIAgent agent)
    {
        scanTimer -= Time.deltaTime;
        if(scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    private void Scan()
    {
        //checks if it detects an object with the set layer
        count = Physics.OverlapSphereNonAlloc(transform.position,
            distance, colliders, layers, QueryTriggerInteraction.Collide);

        //refreshes the list
        objects.Clear();
        for (int i = 0; i < count; i++)
        {
            //access the GO of the objs that were around the sphere
            GameObject obj = colliders[i].gameObject;
            //Checks if the object around the sphere is seen in the FOV
            if(IsInSight(obj))
            {
                //adds the seen obj
                objects.Add(obj);
                //Debug.LogError($"Obj seen: {obj.transform.name}");
            }
        }
    }

    public bool IsInSight(GameObject obj)
    {
        //enemy position
        Vector3 origin = transform.position;
        //checked obj position
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;
        //checks if its in the range of the y axis range / fov's height
        if(direction.y < 0 || direction.y > height)
        {
            return false;
        }
        //checks if its in the range of the x axis range / fov's width
        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if(deltaAngle > angle)
        {
            return false;
        }
        //also checks if the object is covered by a wall / obstacle
        origin.y += height / 2;
        dest.y = origin.y;
        if(Physics.Linecast(origin, dest, occlusionLayers))
        {
            return false;
        }

        return true;
    }

    Mesh CreatWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 +2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int [] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

        int vert = 0;

        //left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; ++i)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;

            //far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            //top side
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            //bottom side
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }

        for (int i = 0; i < numVertices; ++i)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreatWedgeMesh();
    }

    private void OnEnable()
    {
        //when the object is realease from the pool
        mesh = CreatWedgeMesh();
    }

    /*
    private void OnDrawGizmos()
    {
        if(mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        //remove if the "pickup" function is implemented
        //color red for the objects around the sphere
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i < count; ++i)
        {
            Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
        }
        

        //color green for the captured objects
        Gizmos.color = Color.green;
        foreach (var obj in Objects)
        {
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
        }
    }
    */
}
