using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	void Start()
	{
		this.filter = this.GetComponent<MeshFilter>();

		Mesh mesh = new Mesh();
		this.filter.mesh = mesh;
		// X Y Z
		Vector3[] verts = {
			// TOP RIGHT
			new Vector3(1,1,0),
			// TOP CENTER
			new Vector3(0,1,0),
			// TOP LEFT
			new Vector3(-1,1,0),

			// MIDDLE LEFT
			new Vector3(-1,0,0),
			// MIDDLE CENTER
			new Vector3(0,0,0),
			// MIDDLE RIGHT
			new Vector3(1,0,0),

			// BOTTOM LEFT
			new Vector3(-1,-1,0),
			// BOTTOM CENTER
			new Vector3(0,-1,0),
			// BOTTOM RIGHT
			new Vector3(1,-1,0),

	};

		int[] tri = {
		0,4,1,
		0,5,4,

		1,4,3,
		1,3,2,

		5,7,4,
		5,8,7,

		4,6,3,
		4,7,6
	};

		// mesh.uv
		Vector2[] uvs = {
				// TOP RIGHT
				new Vector2(1.0f, 1.0f),
				// CENTER TOP
				new Vector2(0.5f, 1.0f),
				// TOP LEFT
				new Vector2(0.0f, 1.0f),

				// LEFT
				new Vector2(0.0f, 0.5f),
				// CENTER
				new Vector2(0.5f, .5f),
				// RIGHT
				new Vector2(1.0f, 0.5f),

				// LEFT
				new Vector2(0.0f, 0.0f),
				// CENTER
				new Vector2(0.5f, 0.0f),
				// RIGHT
				new Vector2(1.0f, 0.0f),


			};
		Vector3[] normals = {
				-Vector3.forward,
				-Vector3.forward,
				-Vector3.forward,
				-Vector3.forward
			};
		mesh.vertices = verts;
		mesh.triangles = tri;
		mesh.uv = uvs;
		mesh.RecalculateNormals();

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
