using System;
using UnityEngine;

/// <summary>
/// Warper object is responsible for warping/edge blend
/// 
/// Author: Christoffer A Træen
/// </summary>
public class Dewarp
{
	/// <summary>
	/// Holds the positions for the Dewarp mesh
	/// </summary>
	[Serializable]
	public class DewarpMeshPosition
	{
		/// <summary>
		/// Upper right corner position
		/// </summary>
		public Vector2 upperRightPosition;
		/// <summary>
		/// Lower Right corner position
		/// </summary>
		public Vector2 lowerRightPosition;
		/// <summary>
		/// Upper left corner position
		/// </summary>
		public Vector2 upperLeftPosition;
		/// <summary>
		/// Lower left corner position
		/// </summary>
		public Vector2 lowerLeftPosition;

		/// <summary>
		/// Sets vertecies positions
		/// </summary>
		/// <param name="upperLeftPosition">The upper left corner position</param>
		/// <param name="upperRightPosition">The upper right corner position</param>
		/// <param name="lowerRightPosition">The lower right corner position</param>
		/// <param name="lowerLeftPosition">The lower left corner position</param>
		/// <exception cref="ArgumentNullException">Thrown if any of the arguments are null</exception>  
		public DewarpMeshPosition(Vector3 upperRightPosition, Vector3 upperLeftPosition, Vector3 lowerLeftPosition, Vector3 lowerRightPosition)
		{
			if (upperRightPosition == null) throw new ArgumentNullException();
			if (upperLeftPosition == null) throw new ArgumentNullException();
			if (lowerLeftPosition == null) throw new ArgumentNullException();
			if (lowerRightPosition == null) throw new ArgumentNullException();

			this.upperRightPosition = upperRightPosition;
			this.upperLeftPosition = upperLeftPosition;
			this.lowerLeftPosition = lowerLeftPosition;
			this.lowerRightPosition = lowerRightPosition;
		}

		/// <summary>
		/// Returns an array of all positions,
		/// positions is returned in order upper right, upper left,
		/// lower left, lower right.
		/// </summary>
		/// <returns>array of all mesh positions</returns>
		public Vector3[] getAllPositions()
		{
			return new Vector3[] {
				this.upperRightPosition,
				this.upperLeftPosition,
				this.lowerLeftPosition,
				this.lowerRightPosition
			};
		}

	};

	/// <summary>
	/// The game object name for the dewarp mesh
	/// </summary>
	private readonly string objectName = "Dewarp Mesh For:";

	/// <summary>
	/// Holds the gameobject reference for the dewarp mesh
	/// </summary>
	private GameObject dewarpObject;

	/// <summary>
	/// Holds the reference to the dewarp mesh
	/// </summary>
	private Mesh warpMesh;

	/// <summary>
	/// Holds the reference to the dewarp mesh filter
	/// </summary>
	private MeshFilter warpMeshFilter;

	/// <summary>
	/// Render materioal
	/// </summary>
	private Material renderMaterial;

	/// <summary>
	/// Holds the dewarp positions
	/// </summary>
	public DewarpMeshPosition dewarpPositions;

	public Dewarp(string name, Material postProcessMaterial, DewarpMeshPosition vertices, RenderTexture cameraTexture)
	{
		this.dewarpPositions = vertices;

		dewarpObject = new GameObject(objectName + name + "HELLO");
		dewarpObject.layer = 8;

		this.warpMeshFilter = dewarpObject.AddComponent<MeshFilter>();

		this.warpMeshFilter.mesh = CreateMesh();
		dewarpObject.layer = 8; //post processing layer is 8

		//create material for left warpMesh
		this.renderMaterial = new Material(postProcessMaterial);
		this.renderMaterial.name = $"Material for {name}";

		//assign the render texture to the material and the material to the warpMesh
		renderMaterial.mainTexture = cameraTexture;
		dewarpObject.AddComponent<MeshRenderer>().material = renderMaterial;
	}

	/// <summary>
	/// Creates the dewarp mesh
	/// </summary>
	/// <returns>the created dewarp mesh</returns>
	private Mesh CreateMesh()
	{
		this.warpMesh = new Mesh();
		Vector2[] uvs = {
				new Vector2(1.0f, 1.0f),
				new Vector2(0.0f, 1.0f),
				new Vector2(0.0f, 0.0f),
				new Vector2(1.0f, 0.0f)
			};
		Vector3[] normals = {
				-Vector3.forward,
				-Vector3.forward,
				-Vector3.forward,
				-Vector3.forward
			};
		int[] triangles = {
				0, 3, 2,
				0, 2, 1
			};
		warpMesh.vertices = dewarpPositions.getAllPositions();
		warpMesh.triangles = triangles;
		warpMesh.uv = uvs;
		warpMesh.normals = normals;

		return this.warpMesh;
	}

	/// <summary>
	/// Returns the GameObject reference for the dewarp mesh
	/// </summary>
	/// <returns>GameObject reference for the dewarp mesh</returns>
	public GameObject GetDewarpGameObject()
	{
		return this.dewarpObject;
	}

	/// <summary>
	/// Returns the Dewarp mesh reference
	/// </summary>
	/// <returns>reference of the warp mesh</returns>
	public Mesh GetDewarpMesh()
	{
		return this.warpMesh;
	}

	/// <summary>
	/// Returns the Dewarp mesh filter reference
	/// </summary>
	/// <returns>reference of the warp mesh filter </returns>
	public MeshFilter GetDewarpMeshFilter()
	{
		return this.warpMeshFilter;
	}

}
