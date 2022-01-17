// Roman Baranov 28.12.2021

using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AI_Sensor : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private float _distance = 10f;
    /// <summary>
    /// Sensor vision distance
    /// </summary>
    public float Distance { get { return _distance; } }

    [SerializeField] private float _angle = 30f;
    /// <summary>
    /// Sensor vision angle
    /// </summary>
    public float Angle { get { return _angle; } }

    [SerializeField] private float _height = 1.0f;
    [SerializeField] private Color _meshColor = Color.red;
    [SerializeField] private int _scanFrequency = 30;

    [Header("Enemy layers")]
    [SerializeField] private LayerMask _layers;
    //To check obstacles in the field of view
    [SerializeField] private LayerMask _occlusionLayers;

    // Filter objects to store only those which are int the vision radius
    private List<GameObject> _objects = new List<GameObject>();

    private Collider[] _colliders = new Collider[50];
    private Mesh _mesh = null;
    private int _count;
    private float _scanInterval;
    private float _scanTimer;
    #endregion

    #region UNITY Methods
    // Start is called before the first frame update
    void Start()
    {
        _scanInterval = 1.0f / _scanFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        _scanTimer -= Time.deltaTime;
        if (_scanTimer <= 0)
        {
            _scanTimer += _scanInterval;
            Scan();
        }
    }


    #region DEBUG
    private void OnValidate()
    {
        _mesh = CreateWedgeMesh();
        _scanInterval = 1.0f / _scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if (_mesh)
        {
            Gizmos.color = _meshColor;
            Gizmos.DrawMesh(_mesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, _distance);
        for (int i = 0; i < _count; i++)
        {
            Gizmos.DrawSphere(_colliders[i].transform.position, 1.0f);
        }

        Gizmos.color = Color.green;
        foreach (var obj in _objects)
        {
            Gizmos.DrawSphere(obj.transform.position, 1.0f);
        }
    }
    #endregion

    #endregion

    #region PUBLIC Methods
    public int Filter(GameObject[] buffer, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        int count = 0;
        foreach (var obj in _objects)
        {
            if (obj != null)
            {
                if (obj.layer == layer)
                {
                    buffer[count++] = obj;
                }
            }
            

            if (buffer.Length == count)
            {
                break; // Buffer is full
            }
        }

        return count;
    }

    /// <summary>
    /// Check if game object is in sight radius
    /// </summary>
    /// <param name="obj">Object to check</param>
    /// <returns>True if object is in sight radius. False if not.</returns>
    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;
        if (direction.y < -0.5 || direction.y > _height)
        {
            return false;
        }

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > _angle)
        {
            return false;
        }

        origin.y += _height / 2;
        dest.y = origin.y;
        if (Physics.Linecast(origin, dest, _occlusionLayers))
        {
            return false;
        }

        return true;
    }
    #endregion

    #region PRIVATE Methods
    /// <summary>
    /// Creates a new sensor mesh and returns it
    /// </summary>
    /// <returns>Instance of the new sensor mesh</returns>
    private Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -_angle, 0) * Vector3.forward * _distance;
        Vector3 bottomRight = Quaternion.Euler(0, _angle, 0) * Vector3.forward * _distance;

        Vector3 topCenter = bottomCenter + Vector3.up * _height;
        Vector3 topRight = bottomRight + Vector3.up * _height;
        Vector3 topLeft = bottomLeft + Vector3.up * _height;

        int vert = 0;

        // left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        // right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -_angle;
        float deltaAngle = (_angle * 2) / segments;
        for (int i = 0; i < segments; i++)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * _distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * _distance;

            topRight = bottomRight + Vector3.up * _height;
            topLeft = bottomLeft + Vector3.up * _height;

            // far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            // top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            // bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }

        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    /// <summary>
    /// Scans the radius for the colliders and add them to objects list
    /// </summary>
    private void Scan()
    {
        _count = Physics.OverlapSphereNonAlloc(transform.position, _distance, _colliders, _layers, QueryTriggerInteraction.Collide);

        _objects.Clear();
        GameObject prevObj = gameObject;

        for (int i = 0; i < _count; i++)
        {
            GameObject obj = _colliders[i].gameObject;
            // Check if the collider belong to same game object
            if (prevObj.transform.root.name != obj.transform.root.name)
            {
                if (IsInSight(obj))
                {
                    _objects.Add(obj);
                }

                prevObj = obj;
            }
        }
    }
    #endregion

}
