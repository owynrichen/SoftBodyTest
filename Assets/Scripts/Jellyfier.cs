using UnityEngine;

public class Jellyfier : MonoBehaviour
{
    public float bounceSpeed;
    public float fallForce;
    public float stiffness;

    private MeshFilter meshFilter;
    private Mesh mesh;

    
    SoftJellyVertex[] jellyVertices;
    Vector3[] currentMeshVerticies;

    // Use this for initialization
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;

        GetVertices();
    }

    private void GetVertices()
    {
        jellyVertices = new SoftJellyVertex[mesh.vertices.Length];
        currentMeshVerticies = new Vector3[mesh.vertices.Length];

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            jellyVertices[i] = new SoftJellyVertex(i, mesh.vertices[i], mesh.vertices[i], Vector3.zero);
            currentMeshVerticies[i] = mesh.vertices[i];
        }
    }

    private void UpdateVertices()
    {
        for (int i = 0; i < jellyVertices.Length; i++)
        {
            jellyVertices[i].UpdateVelocity(bounceSpeed);
            jellyVertices[i].Settle(stiffness);

            jellyVertices[i].currentVertexPosition += jellyVertices[i].currentVelocity * Time.deltaTime;
            currentMeshVerticies[i] = jellyVertices[i].currentVertexPosition;
        }

        mesh.vertices = currentMeshVerticies;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVertices();
    }

    public void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] collisionPoints = collision.contacts;
        for (int i = 0; i < collisionPoints.Length; i++)
        {
            Vector3 inputPoint = collisionPoints[i].point + (collisionPoints[i].point * .1f);
            ApplyPressureToPoint(inputPoint, fallForce);
        }
    }

    public void ApplyPressureToPoint(Vector3 _point, float _pressure)
    {
        for(int i = 0; i < jellyVertices.Length; i++)
        {
            jellyVertices[i].ApplyPressureToVertex(transform, _point, _pressure);
        }
    }
}
