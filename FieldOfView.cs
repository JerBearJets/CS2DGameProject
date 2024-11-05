using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    private Mesh mesh;

    //Ensures the Field of View is only blocked by Objects and not Enemeis
    [SerializeField] private LayerMask layerMask;

    private Vector3 origin = Vector3.zero;

    //Values for Field of View Cone

    public float fov = 90f;

    private float initialAngle;

    public float viewDistance = 10f;

    private const int rayCount = 180;

    //Setup for Vertices, UV, Triangles

    private Vector3[] vertices;

    private Vector2[] uv;

    private int[] triangles;

    private void Start()
    {
        mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = mesh;

        //Initialize vertex, UV, and triangle arrays once

        vertices = new Vector3[rayCount + 2];

        uv = new Vector2[vertices.Length];

        triangles = new int[rayCount * 3];
    }

    private void LateUpdate()
    {
        //Reset the origin and update vertices for FoV

        vertices[0] = origin;

        float angle = initialAngle;

        float angleIncrease = fov / rayCount;

        int vertexIndex = 1;

        int triangleIndex = 0;

        Vector3 viewVector = Vector3.zero;

        //Raycast to prevent Player from seeing behind Objects
        for (int i = 0; i <= rayCount; i++)
        {
            //Calculate ray direction once per angle increment

            viewVector = GetVectorFromAngle(angle);

            RaycastHit2D hit = Physics2D.Raycast(origin, viewVector, viewDistance, layerMask);

            vertices[vertexIndex] = hit.collider == null

                ? origin + viewVector * viewDistance

                : hit.point;

            //Generates the FoV Cone
            if (i > 0)
            {
                triangles[triangleIndex] = 0;

                triangles[triangleIndex + 1] = vertexIndex - 1;

                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;

            angle -= angleIncrease;
        }

        //Assign to mesh
        mesh.Clear();

        mesh.vertices = vertices;

        mesh.uv = uv;

        mesh.triangles = triangles;

        mesh.RecalculateBounds();
    }

    //Converts an angle to a Vector3 direction
    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad;

        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    //Sets origin to player's position
    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    //Sets FoV direction
    public void SetFoVDirection(float fovDirection)
    {
        initialAngle = fovDirection + fov * 0.5f;
    }
}