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
        [Header("Mesh vertecies(Do not add extra verts)")]
        public Vector3[] verts;
        // {
        // 		// 0 0 0 0 0 0 0 0 0
        // 		// TOP RIGHT
        // 		new Vector3(1.0f, 1.0f, 0.0f),
        // 		// TOP CENTER
        // 		new Vector3(0.0f, 1.0f, 0.0f),
        // 		// TOP LEFT
        // 		new Vector3(-1.0f, 1.0f, 0.0f),

        // 		// 1 1 1 1 1 1 1 1 1
        // 		// 1 LEFT
        // 		new Vector3(-1.0f, 0.5f, 0.0f),
        // 		// 1 CENTER
        // 		new Vector3(0.0f, 0.5f, 0.0f),
        // 		// 1 RIGHT
        // 		new Vector3(1.0f, 0.5f, 0.0f),

        // 		// 2 2 2 2 2 2 2 2 2 
        // 		// 2 LEFT
        // 		new Vector3(-1.0f, 0.0f, 0.0f),
        // 		// 2 CENTER
        // 		new Vector3(0.0f, 0.0f, 0.0f),
        // 		// 2 RIGHT
        // 		new Vector3(1.0f, 0.0f, 0.0f),

        // 		// 3 3 3 3 3 3 3 3 3 3 
        // 		// 3 LEFT
        // 		new Vector3(-1.0f, -0.5f, 0.0f),
        // 		// 3 CENTER
        // 		new Vector3(0.0f, -0.5f, 0.0f),
        // 		// 3 RIGHT
        // 		new Vector3(1.0f, -0.5f, 0.0f),

        // 		// 4 4 4 4 4 4 4 4 4 4 4 
        // 		// 4 LEFT
        // 		new Vector3(-1.0f, -1.0f, 0.0f),
        // 		// 4 CENTER
        // 		new Vector3(0.0f, -1.0f, 0.0f),
        // 		// 4 RIGHT
        // 		new Vector3(1.0f, -1.0f, 0.0f)


        // };

        public Vector3[] generatedVerts;

        /// <summary>
        /// Upper right corner position
        /// </summary>
        public Vector2 upperRightPosition;

        /// <summary>
        /// <summary>
        /// Upper left corner position
        /// </summary>
        public Vector2 upperLeftPosition;

        /// <summary>
        /// <summary>
        /// center left corner position
        /// </summary>
        public Vector2 centerLeftPosition;

        /// <summary>
        /// <summary>
        /// center right corner position
        /// </summary>
        public Vector2 centerRightPosition;

        /// Lower Right corner position
        /// </summary>
        public Vector2 lowerRightPosition;

        /// <summary>
        /// Lower left corner position
        /// </summary>
        public Vector2 lowerLeftPosition;

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

    public readonly int xSize = 10;
    public readonly int ySize = 10;

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

        dewarpObject = new GameObject(objectName + name);
        dewarpObject.layer = 8;

        this.warpMeshFilter = dewarpObject.AddComponent<MeshFilter>();

        this.warpMeshFilter.mesh = Generate();
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
        // Vector2[] uvs = {
        //         new Vector2(1.0f, 1.0f),
        //         new Vector2(0.0f, 1.0f),
        //         new Vector2(0.0f, 0.0f),
        //         new Vector2(1.0f, 0.0f)
        //     };
        // Vector3[] normals = {
        //         -Vector3.forward,
        //         -Vector3.forward,
        //         -Vector3.forward,
        //         -Vector3.forward
        //     };
        // int[] triangles = {
        //         0, 3, 2,
        //         0, 2, 1
        //     };

        // mesh.uv
        int[] tri = {

            // 0,2,1,
            // 0,3,2,

            // 3,4,2,
            // 3,5,4,

            // 5,6,4,
            // 5,7,6,

            // 7,8,6,
            // 7,9,8,

            0,4,1,
            0,5,4,

            1,3,2,
            1,4,3,

            5,7,4,
            5,8,7,

            4,6,3,
            4,7,6,

            8,10,7,
            8,11,10,

            7,9,6,
            7,10,9,

            11,13,10,
            11,14,13,

            10,12,9,
            10,13,12,


        };
        Vector2[] uvs = {
			// TOP
        			// TOP RIGHT
        			new Vector2(1.0f, 1.0f),
        			// CENTER TOP
        			new Vector2(0.5f, 1.0f),
        			// TOP LEFT
        			new Vector2(0.0f, 1.0f),


        			// LEFT
        			new Vector2(0.0f, 3/4f),
        			// CENTER
        			new Vector2(0.5f, 3/4f),
        			// RIGHT
        			new Vector2(1.0f, 3/4f),

        			// LEFT
        			new Vector2(0.0f, 2/4f),
        			// CENTER
        			new Vector2(0.5f, 2/4f),
        			// RIGHT
        			new Vector2(1.0f, 2/4f),

        			// LEFT
        			new Vector2(0.0f, 1/4f),
        			// CENTER
        			new Vector2(0.5f,  1/4f),
        			// RIGHT
        			new Vector2(1.0f,  1/4f),

        			// LEFT
        			new Vector2(0.0f, 0.0f),
        			// CENTER
        			new Vector2(0.5f, 0.0f),
        			// RIGHT
        			new Vector2(1.0f, 0.0f),


                };


        // warpMesh.vertices = dewarpPositions.verts;
        warpMesh.triangles = tri;
        warpMesh.uv = uvs;
        // warpMesh.normals = normals;
        warpMesh.RecalculateNormals();
        warpMesh.RecalculateTangents();
        return this.warpMesh;
    }

    private Mesh Generate()
    {
        // WaitForSeconds wait = new WaitForSeconds(0.01f);
        this.warpMesh = new Mesh();
        this.warpMesh.name = "Warp grid";

        this.dewarpPositions.generatedVerts = new Vector3[(xSize + 1) * (ySize + 1)];
        Vector2[] uv = new Vector2[this.dewarpPositions.generatedVerts.Length];
        Vector4[] tangents = new Vector4[this.dewarpPositions.generatedVerts.Length];
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
                if (this.dewarpPositions.verts.Length == this.dewarpPositions.generatedVerts.Length)
                {
                    this.dewarpPositions.generatedVerts[i] = new Vector3((float)this.dewarpPositions.verts[i].x, (float)this.dewarpPositions.verts[i].y);
                }
                else
                {
                    this.dewarpPositions.generatedVerts[i] = new Vector3((float)lastX, (float)lastY);
                }
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
                tangents[i] = tangent;
                // yield return wait;
            }
            lastX = -1;
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
        this.warpMesh.vertices = this.dewarpPositions.generatedVerts;
        this.warpMesh.triangles = triangles;
        this.warpMesh.uv = uv;
        this.warpMesh.tangents = tangents;
        this.warpMesh.RecalculateNormals();
        this.warpMesh.RecalculateTangents();
        Debug.Log(this.dewarpPositions.verts);
        if (this.dewarpPositions.verts.Length < 16)
        {
            // this.dewarpPositions.verts = new Vector3[(xSize + 1) * (ySize + 1)];
            this.dewarpPositions.verts = this.dewarpPositions.generatedVerts;
        }

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
