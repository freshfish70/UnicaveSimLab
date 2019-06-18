using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VertexText : MonoBehaviour
{

    private MeshRenderer renderer;

    private MeshFilter filter;

    public Camera cam;

    public Material mat;

    // void Start()
    // {
    // 	this.filter = this.GetComponent<MeshFilter>();

    // 	Mesh mesh = new Mesh();
    // 	this.filter.mesh = mesh;
    // 	// X Y Z
    // 	Vector3[] verts = {
    // 		// TOP RIGHT
    // 		new Vector3(1,1,0),

    // 		// TOP LEFT
    // 		new Vector3(-1,1,0),

    // 		//CENTERS LEFT
    // 		new Vector3(-1,0.5f,0),
    // 		new Vector3(-1,0,0),
    // 		new Vector3(-1,-0.5f,0),

    // 		// BOTTOM LEFT
    // 		new Vector3(-1,-1,0),

    // 		// BOTTOM RIGHT
    // 		new Vector3(1,-1,0),

    // 		// CENTER RIGHT
    // 		new Vector3(1,-0.5f,0),
    // 		new Vector3(1,0,0),
    // 		new Vector3(1,0.5f,0),
    // };

    // 	int[] tri = {
    // 	0,2,1,
    // 	0,9,2,

    // 	9,3,2,
    // 	9,8,3,

    // 	8,4,3,
    // 	8,7,4,

    // 	7,5,4,
    // 	7,6,5
    // };

    // 	// mesh.uv
    // 	Vector2[] uvs = {
    // 			// TOP RIGHT
    // 			new Vector2(1.0f, 1.0f),
    // 			// TOP LEFT
    // 			new Vector2(0.0f, 1.0f),

    // 			// LEFT CENTER
    // 			new Vector2(0.0f, 3/4f),
    // 			new Vector2(0.0f, 2/4f),
    // 			new Vector2(0.0f, 1/4f),

    // 			// LOWER LEFT
    // 			new Vector2(0.0f, 0.0f),
    // 			// LOWER RIGHT
    // 			new Vector2(1.0f, 0.0f),

    // 			// RIGHT CENTER
    // 			new Vector2(1.0f, 1/4f),
    // 			new Vector2(1.0f, 2/4f),
    // 			new Vector2(1.0f, 3/4f),
    // 		};
    // 	Vector3[] normals = {
    // 			-Vector3.forward,
    // 			-Vector3.forward,
    // 			-Vector3.forward,
    // 			-Vector3.forward
    // 		};
    // 	mesh.vertices = verts;
    // 	mesh.triangles = tri;
    // 	mesh.uv = uvs;
    // 	mesh.RecalculateNormals();

    // 	RenderTexture rt = new RenderTexture(250, 250, 0);
    // 	this.cam.targetTexture = rt;

    // 	Material mate = new Material(this.mat);
    // 	mate.name = "supremayt";

    // 	//assign the render texture to the material and the material to the warpMesh
    // 	mate.mainTexture = rt;
    // 	this.GetComponent<MeshRenderer>().material = mate;
    // }
    public int xSize = 50, ySize = 50;
    private Mesh mesh;

    private Vector3[] vertices;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
    // https://catlikecoding.com/unity/tutorials/procedural-grid/
    private IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
                tangents[i] = tangent;
                yield return wait;
            }
        }


        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.tangents = tangents;

    }
    void Start()
    {
        StartCoroutine(Generate());
        // Generate();
        //     this.filter = this.GetComponent<MeshFilter>();

        //     Mesh mesh = new Mesh();
        //     this.filter.mesh = mesh;
        //     // X Y Z
        //     Vector3[] verts = {
        // 		// 0 0 0 0 0 0 0 0 0
        // 		// TOP RIGHT
        // 		new Vector3(1.0f, 1.0f, 0.0f),
        // 		// TOP M RIGHT
        // 		new Vector3(0.5f, 1f, 0.0f),
        // 		// TOP CENTER
        // 		new Vector3(0.0f, 1.0f, 0.0f),
        // 		// TOP M LEFT
        // 		new Vector3(-0.5f, 1f, 0.0f),
        // 		// TOP LEFT
        // 		new Vector3(-1.0f, 1.0f, 0.0f),

        // 		// 1 1 1 1 1 1 1 1 1
        // 		// 1 LEFT
        // 		new Vector3(-1.0f, 0.5f, 0.0f),
        // 		// 1 M LEFT
        // 		new Vector3(-0.5f ,0.5f, 0.0f),
        // 		// 1 CENTER
        // 		new Vector3(0.0f, 0.5f, 0.0f),
        // 		// 1 M RIGHT
        // 		new Vector3(0.5f, 0.5f, 0.0f),
        // 		// 1 RIGHT
        // 		new Vector3(1.0f, 0.5f, 0.0f),

        // 		// 2 2 2 2 2 2 2 2 2 
        // 		// 2 LEFT
        // 		new Vector3(-1.0f, 0.0f, 0.0f),
        // 		// 2 M LEFT
        // 		new Vector3(-0.5f, 0.0f, 0.0f),
        // 		// 2 CENTER
        // 		new Vector3(0.0f, 0.0f, 0.0f),
        // 		// 2 M RIGHT
        // 		new Vector3(0.5f, 0.0f, 0.0f),
        // 		// 2 RIGHT
        // 		new Vector3(1.0f, 0.0f, 0.0f),

        // 		// 3 3 3 3 3 3 3 3 3 3 
        // 		// 3 LEFT
        // 		new Vector3(-1.0f, -0.5f, 0.0f),
        // 		// 3 M LEFT
        // 		new Vector3(-0.5f, -0.5f, 0.0f),
        // 		// 3 CENTER
        // 		new Vector3(0.0f, -0.5f, 0.0f),
        // 		// 3 M RIGHT
        // 		new Vector3(0.5f, -0.5f, 0.0f),
        // 		// 3 RIGHT
        // 		new Vector3(1.0f, -0.5f, 0.0f),

        // 		// 4 4 4 4 4 4 4 4 4 4 4 
        // 		// 4 LEFT
        // 		new Vector3(-1.0f, -1.0f, 0.0f),
        // 		// 4 M LEFT
        // 		new Vector3(-0.5f, -0.5f, 0.0f),
        // 		// 4 CENTER
        // 		new Vector3(0.0f, -0.5f, 0.0f),
        // 		// 4 M RIGHT
        // 		new Vector3(0.5f, -0.5f, 0.0f),
        // 		// 4 RIGHT
        // 		new Vector3(1.0f, -0.5f, 0.0f),


        // };

        //     int[] tri = {
        //     0,5,1,
        //     0,9,5,

        //     1,7,2,
        //     1,5,7,

        //     2,6,3,
        //     2,7,6,

        //     3,5,4,
        //     3,6,5,

        //     4,6,3,
        //     4,7,6,

        //     4,6,3,
        //     4,7,6,

        //     4,6,3,
        //     4,7,6,

        //     4,6,3,
        //     4,7,6,

        //     4,6,3,
        //     4,7,6,

        //     4,6,3,
        //     4,7,6,

        //     4,6,3,
        //     4,7,6,

        //     4,6,3,
        //     4,7,6,

        //     4,6,3,
        //     4,7,6,

        //     4,6,3,
        //     4,7,6,

        //     4,6,3,
        //     4,7,6,

        //     4,6,3,
        //     4,7,6
        // };

        //     // mesh.uv
        //     Vector2[] uvs = {
        // 			// TOP RIGHT
        // 			new Vector2(1.0f, 1.0f),
        // 			// CENTER TOP
        // 			new Vector2(0.5f, 1.0f),
        // 			// TOP LEFT
        // 			new Vector2(0.0f, 1.0f),

        // 			// LEFT
        // 			new Vector2(0.0f, 0.5f),
        // 			// CENTER
        // 			new Vector2(0.5f, .5f),
        // 			// RIGHT
        // 			new Vector2(1.0f, 0.5f),

        // 			// LEFT
        // 			new Vector2(0.0f, 0.0f),
        // 			// CENTER
        // 			new Vector2(0.5f, 0.0f),
        // 			// RIGHT
        // 			new Vector2(1.0f, 0.0f),


        //         };
        //     Vector3[] normals = {
        //             -Vector3.forward,
        //             -Vector3.forward,
        //             -Vector3.forward,
        //             -Vector3.forward
        //         };
        //     mesh.vertices = verts;
        //     mesh.triangles = tri;
        //     mesh.uv = uvs;
        //     mesh.RecalculateNormals();

    }

    private int vertexIndex = 0;
    private float delta = 0.005f;

    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            Vector3[] verts = this.filter.sharedMesh.vertices;
            verts[vertexIndex] = new Vector3(verts[vertexIndex].x + 0 * delta, verts[vertexIndex].y + 1 * delta, verts[vertexIndex].z);
            this.filter.sharedMesh.vertices = verts;
            this.filter.sharedMesh.UploadMeshData(false);
        }
        if (Input.GetKey(KeyCode.O))
        {
            Vector3[] verts = this.filter.sharedMesh.vertices;
            verts[vertexIndex] = new Vector3(verts[vertexIndex].x - 0 * delta, verts[vertexIndex].y + -1 * delta, verts[vertexIndex].z);
            this.filter.sharedMesh.vertices = verts;
            this.filter.sharedMesh.UploadMeshData(false);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            this.vertexIndex += 1;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {

            RenderTexture rt = new RenderTexture(250, 250, 0);
            this.cam.targetTexture = rt;

            Material mate = new Material(this.mat);
            mate.name = "supremayt";

            //assign the render texture to the material and the material to the warpMesh
            mate.mainTexture = rt;
            this.GetComponent<MeshRenderer>().material = mate;
        }

    }
}
