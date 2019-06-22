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
	public int xSize = 5, ySize = 5;
	private Mesh mesh;

	private Vector3[] vertices;

	// private void OnDrawGizmos()
	// {
	// 	Gizmos.color = Color.black;
	// 	for (int i = 0; i < vertices.Length; i++)
	// 	{
	// 		Gizmos.DrawSphere(vertices[i], 0.05f);
	// 	}
	// }
	// https://catlikecoding.com/unity/tutorials/procedural-grid/
	private void Generate()
	{
		// WaitForSeconds wait = new WaitForSeconds(0.01f);
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		mesh.name = "Procedural Grid";

		vertices = new Vector3[(xSize + 1) * (ySize + 1)];
		Vector2[] uv = new Vector2[vertices.Length];
		Vector4[] tangents = new Vector4[vertices.Length];
		Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);


		decimal xx = xSize;
		decimal yy = ySize;

		decimal ymodifier = (2 / yy);
		decimal lastY = -1;

		decimal xmodifier = (2 / xx);
		decimal lastX = -1;

		for (int i = 0, y = 0; y <= ySize; y++)
		{
			lastY += (decimal)ymodifier;
			for (int x = 0; x <= xSize; x++, i++)
			{
				lastX += (decimal)xmodifier;
				vertices[i] = new Vector3((float)lastX, (float)lastY);
				uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
				tangents[i] = tangent;
				// yield return wait;
			}
			lastX = -1;
		}
		Debug.Log((decimal)lastX);
		Debug.Log((decimal)lastY);

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
		Generate();
		// this.filter = this.GetComponent<MeshFilter>();

		// return;
		// // StartCoroutine(Generate());
		// // Generate();
		// this.filter = this.GetComponent<MeshFilter>();

		// Mesh mesh = new Mesh();
		// this.filter.mesh = mesh;
		// // X Y Z
		// Vector3[] verts = {
		// 		// 0 0 0 0 0 0 0 0 0
		// 		// BOTTOM RIGHT
		// 		new Vector3(-1.0f, -1.0f, 0.0f),
		// 		new Vector3(0.0f, -1.0f, 0.0f),
		// 		new Vector3(1.0f, -1.0f, 0.0f),

		// 		new Vector3(-1.0f, -2.0f/3.0f, 0.0f),
		// 		new Vector3(0.0f, -0.5f, 0.0f),
		// 		new Vector3(1.0f, -2.0f/3.0f, 0.0f),

		// 		new Vector3(-1.0f, -1.0f/3.0f, 0.0f),
		// 		new Vector3(1.0f, -1.0f/3.0f, 0.0f),

		// 		new Vector3(-1.0f, 0.0f, 0.0f),
		// 		new Vector3(0.0f, 0.0f, 0.0f),
		// 		new Vector3(1.0f, 0.0f, 0.0f),

		// 		new Vector3(-1.0f, 1.0f/3.0f, 0.0f),
		// 		new Vector3(0.0f, 0.5f, 0.0f),
		// 		new Vector3(1.0f, 1.0f/3.0f, 0.0f),

		// 		new Vector3(-1.0f, 2.0f/3.0f, 0.0f),
		// 		new Vector3(1.0f, 2.0f/3.0f, 0.0f),

		// 		new Vector3(-1.0f, 1.0f, 0.0f),
		// 		new Vector3(0.0f, 1.0f, 0.0f),
		// 		new Vector3(1.0f, 1.0f, 0.0f),

		// };

		// int[] tri = {

		// 	4,1,0,
		// 	4,0,3,
		// 	4,3,6,

		// 	7,5,4,
		// 	5,2,4,
		// 	2,1,4,

		// 	9,4,6,
		// 	9,6,8,
		// 	9,8,11,

		// 	13,10,9,
		// 	10,7,9,
		// 	7,4,9,

		// 	12,9,11,
		// 	12,11,14,
		// 	12,14,16,
		// 	12,16,17,

		// 	18,12,17,
		// 	18,15,12,
		// 	15,13,12,
		// 	13,9,12





		// };

		// // mesh.uv
		// Vector2[] uvs = {
		// 			// 0
		// 			new Vector2(0.0f, 0.0f),
		// 			// 1
		// 			new Vector2(0.5f, 0.0f),
		// 			// 2
		// 			new Vector2(1.0f, 0.0f),

		// 			// 3
		// 			new Vector2(0.0f, 1.0f/6.0f),
		// 			// 4
		// 			new Vector2(0.5f, 1.0f/4.0f),
		// 			// 5
		// 			new Vector2(1.0f, 1.0f/6.0f),

		// 			// 6
		// 			new Vector2(0.0f, 2.0f/6.0f),
		// 			// 7
		// 			new Vector2(1.0f, 2.0f/6.0f),

		// 			// 8
		// 			new Vector2(0.0f, 3.0f/6.0f),
		// 			// 9
		// 			new Vector2(0.5f, 2.0f/4.0f),
		// 			// 10
		// 			new Vector2(1.0f, 3.0f/6.0f),

		// 			// 11
		// 			new Vector2(0.0f, 4.0f/6.0f),
		// 			// 12
		// 			new Vector2(0.5f, 3.0f/4.0f),
		// 			// 13
		// 			new Vector2(1.0f, 4.0f/6.0f),

		// 			// 14
		// 			new Vector2(0.0f, 5.0f/6.0f),
		// 			// 15
		// 			new Vector2(1.0f, 5.0f/6.0f),

		// 			// 16
		// 			new Vector2(0.0f, 1.0f),
		// 			// 17
		// 			new Vector2(0.5f, 1.0f),
		// 			// 18
		// 			new Vector2(1.0f, 1.0f),

		// 		};
		// Vector3[] normals = {
		// 			-Vector3.forward,
		// 			-Vector3.forward,
		// 			-Vector3.forward,
		// 			-Vector3.forward
		// 		};
		// mesh.vertices = verts;
		// mesh.triangles = tri;
		// mesh.uv = uvs;
		// mesh.RecalculateNormals();
		// mesh.RecalculateBounds();
		// mesh.RecalculateTangents();
		this.filter = this.GetComponent<MeshFilter>();

		RenderTexture rt = new RenderTexture(3000, 3000, 0);
		this.cam.targetTexture = rt;

		Material mate = new Material(this.mat);
		mate.name = "supremayt";

		//assign the render texture to the material and the material to the warpMesh
		mate.mainTexture = rt;
		this.GetComponent<MeshRenderer>().material = mate;

		foreach (var item in this.filter.sharedMesh.triangles)
		{
			Debug.Log(item);
		}

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
			this.vertices = this.filter.sharedMesh.vertices;
			this.moveVerts(vertexIndex, new Vector3(0, 1 * delta / 2, 0));
		}
		if (Input.GetKey(KeyCode.O))
		{
			Vector3[] verts = this.filter.sharedMesh.vertices;
			verts[vertexIndex] = new Vector3(verts[vertexIndex].x - 0 * delta, verts[vertexIndex].y + -1 * delta, verts[vertexIndex].z);
			this.filter.sharedMesh.vertices = verts;
			this.filter.sharedMesh.UploadMeshData(false);
			this.vertices = this.filter.sharedMesh.vertices;
			this.moveVerts(vertexIndex, new Vector3(0, -1 * delta / 2, 0));


		}
		if (Input.GetKey(KeyCode.Keypad8))
		{
			Vector3[] verts = this.filter.sharedMesh.vertices;
			verts[vertexIndex] = new Vector3(verts[vertexIndex].x - 0 * delta, verts[vertexIndex].y + 0 * delta, verts[vertexIndex].z + 1 * delta);
			this.filter.sharedMesh.vertices = verts;
			this.filter.sharedMesh.UploadMeshData(false);
			this.vertices = this.filter.sharedMesh.vertices;

		}
		if (Input.GetKey(KeyCode.Keypad2))
		{
			Vector3[] verts = this.filter.sharedMesh.vertices;
			verts[vertexIndex] = new Vector3(verts[vertexIndex].x - 0 * delta, verts[vertexIndex].y + 0 * delta, verts[vertexIndex].z - 1 * delta);
			this.filter.sharedMesh.vertices = verts;
			this.filter.sharedMesh.UploadMeshData(false);
			this.vertices = this.filter.sharedMesh.vertices;

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

	public void moveVerts(int index, Vector3 dir)
	{

		int xmax = this.xSize;
		int ymax = this.ySize;

		int row = (index / (xmax + 1)) < 1.0f ? 0 : (int)Mathf.Floor(index / (xmax + 1));

		List<int> vertsToShift = new List<int>();
		for (int i = 0; i < this.vertices.Length; i++)
		{
			if (row - 1 == (int)Mathf.Floor(i / (xmax + 1)))
			{
				int negativeSelector = index - xmax;
				if (negativeSelector == i) vertsToShift.Add(i);
				if (negativeSelector - 1 == i) vertsToShift.Add(i);
				if (negativeSelector - 2 == i) vertsToShift.Add(i);
			}
			if (row + 1 == (int)Mathf.Floor(i / (xmax + 1)))
			{
				int positiveSelector = index + xmax;
				if (positiveSelector == i) vertsToShift.Add(i);
				if (positiveSelector + 1 == i) vertsToShift.Add(i);
				if (positiveSelector + 2 == i) vertsToShift.Add(i);
			}
			if (row == (int)Mathf.Floor(i / (xmax + 1)))
			{
				if (index - 1 == i) vertsToShift.Add(i);
				if (index + 1 == i) vertsToShift.Add(i);
			}
		}

		Vector3[] vertices = this.filter.sharedMesh.vertices;
		foreach (var ind in vertsToShift)
		{
			Vector3 e = new Vector3(vertices[ind].x, vertices[ind].y, vertices[ind].z);
			vertices[ind] = e += dir;
		}
		this.filter.sharedMesh.vertices = vertices;

	}

	public void moveVerts2(int index, Vector3 dir)
	{

		int sizeX = this.xSize;
		int ymax = this.ySize;

		int indexSizeX = this.xSize + 1;



		// The row the selected index is on
		int indexRow = (index / (sizeX + 1)) < 1.0f ? 0 : (int)Mathf.Floor(index / (sizeX + 1));

		// The size of the select grid
		int selectGridSize = 2;

		int startRow = (indexRow - selectGridSize) >= 0 ? (indexRow - selectGridSize) : 0;
		int endRow = (indexRow + selectGridSize) <= sizeX ? (indexRow - selectGridSize) : sizeX;

		int startIndex = startRow * indexSizeX;
		int endIndex = startRow * indexSizeX;

		// Vertecies to move
		List<int> vertsToShift = new List<int>();

		int currentRow = 0;

		for (int row = startRow; row <= endRow; row++)
		{

		}


		for (int i = startIndex; i < this.vertices.Length; i++)
		{
			currentRow = (int)Mathf.Floor(i / (sizeX + 1));


			if (indexRow - 1 == currentRow)
			{
				int negativeSelector = index - sizeX;
				if (negativeSelector == i) vertsToShift.Add(i);
				for (int gridIndex = 0; gridIndex < selectGridSize; gridIndex++)
				{
					if (negativeSelector - gridIndex == i) vertsToShift.Add(i);
				}
			}
			if (indexRow + 1 == currentRow)
			{
				int positiveSelector = index + sizeX;
				if (positiveSelector == i) vertsToShift.Add(i);
				for (int gridIndex = 0; gridIndex < selectGridSize; gridIndex++)
				{
					if (positiveSelector + gridIndex == i) vertsToShift.Add(i);
				}
			}
			if (indexRow == currentRow)
			{
				for (int gridIndex = 1; gridIndex < selectGridSize; gridIndex++)
				{
					if (index - gridIndex == i) vertsToShift.Add(i);
					if (index + gridIndex == i) vertsToShift.Add(i);
				}

			}
		}

		Vector3[] vertices = this.filter.sharedMesh.vertices;
		foreach (var ind in vertsToShift)
		{
			Vector3 e = new Vector3(vertices[ind].x, vertices[ind].y, vertices[ind].z);
			vertices[ind] = e += dir;
		}
		this.filter.sharedMesh.vertices = vertices;

	}
}

