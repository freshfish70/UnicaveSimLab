using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RealtimeCalibrator : NetworkBehaviour
{
	public struct CalibrationSelection
	{
		public string machineName;
		public PhysicalDisplayCalibration calibration;
	}

	public int selectedIndex = 0;
	public List<CalibrationSelection> allOptions;

	public int vertexIndex = 0;

	/// <summary>
	/// Holds a reference of the display object to be shown
	/// when calibrating
	/// </summary>
	[SerializeField]
	private InfoDisplay display;

	/// <summary>
	/// The instantiated instance of InfoDisplay for the left eye/cam
	/// </summary>
	private InfoDisplay leftInfoDisplayInstance;

	/// <summary>
	/// The instantiated instance of InfoDisplay for the right eye/cam
	/// </summary>
	private InfoDisplay rightInfoDisplayInstance;

	void Start()
	{
		allOptions = new List<CalibrationSelection>();
		//generate list of options
		List<PhysicalDisplay> displays = gameObject.GetComponent<UCNetwork>().GetAllDisplays();
		foreach (PhysicalDisplay disp in displays)
		{
			PhysicalDisplayCalibration cali = disp.gameObject.GetComponent<PhysicalDisplayCalibration>();
			if (cali != null)
			{
				CalibrationSelection selection;
				selection.machineName = (disp.manager == null) ? disp.machineName : disp.manager.machineName;
				selection.calibration = cali;
				allOptions.Add(selection);
			}
		}

		Debug.Log("RealtimeCalibration: Found " + allOptions.Count + " calibration objects");

		StartCoroutine(InitiateInfoScreen());
	}

	/// <summary>
	/// Instatiate the info screen with a delay.
	/// So we are sure everything has initialized before
	/// setting the info screen
	/// </summary>
	/// <returns></returns>
	private IEnumerator InitiateInfoScreen()
	{
		yield return new WaitForSeconds(1);
		this.CreateInfoDisplays();
		this.InfoDisplayShift(this.selectedIndex);
		this.RpcInfoDisplayShift(this.selectedIndex);
		yield break;
	}

	/// <summary>
	/// Instantiates the info displays.
	/// And set them to disabled at start
	/// </summary>
	private void CreateInfoDisplays()
	{
		if (this.display == null) return;
		this.leftInfoDisplayInstance = Instantiate(display);
		this.leftInfoDisplayInstance.gameObject.SetActive(false);

		this.rightInfoDisplayInstance = Instantiate(display);
		this.rightInfoDisplayInstance.gameObject.SetActive(false);
	}

	private void LocalShift(Vector2 direction, float delta, int selectedIndex, int vertexIndex)
	{
		PhysicalDisplayCalibration calibration = allOptions[selectedIndex].calibration;
		calibration.SetVertextPoint(vertexIndex);
		Debug.Log("RealtimeCalibration: LocalShift called " + delta + ", " + selectedIndex + ", " + vertexIndex);


		MeshFilter lastWarpedFilter = null;
		foreach (Dewarp dewarp in calibration.GetDisplayWarpsValues())
		{
			MeshFilter meshFilter = dewarp.GetDewarpMeshFilter();
			lastWarpedFilter = meshFilter;
			Vector3[] verts = meshFilter.sharedMesh.vertices;
			verts[vertexIndex] = new Vector3(verts[vertexIndex].x + direction.x * delta, verts[vertexIndex].y + direction.y * delta, verts[vertexIndex].z);
			meshFilter.sharedMesh.vertices = verts;
			meshFilter.sharedMesh.UploadMeshData(false);
		}

		calibration.UpdateMeshPositions(lastWarpedFilter?.sharedMesh.vertices);

	}

	/// <summary>
	/// Shifts the info window around to the display on the given index
	/// </summary>
	/// <param name="selectedIndex">index of the display</param>
	private void InfoDisplayShift(int selectedIndex)
	{
		PhysicalDisplayCalibration currentDisplay = this.allOptions[selectedIndex].calibration;
		if (currentDisplay == null || this.display == null) return;

		// if (currentDisplay.GetDisplay().is3D)
		// {
		// 	if (this.leftInfoDisplayInstance != null)
		// 	{
		// 		this.SetInfoDisplay(leftInfoDisplayInstance.gameObject, currentDisplay.GetLeftWarp().GetDewarpGameObject().transform);
		// 	}
		// 	if (this.rightInfoDisplayInstance != null)
		// 	{
		// 		this.SetInfoDisplay(leftInfoDisplayInstance.gameObject, currentDisplay.GetLeftWarp().GetDewarpGameObject().transform);
		// 	}
		// }
		// else
		// {
		// 	this.SetInfoDisplay(leftInfoDisplayInstance.gameObject, currentDisplay.GetCenterWarp().GetDewarpGameObject().transform);
		// }

	}

	/// <summary>
	/// Activates the info display, sets it parent and resets its local position
	/// so it is center to the parent
	/// </summary>
	/// <param name="infoDisplay"></param>
	/// <param name="parent"></param>
	private void SetInfoDisplay(GameObject infoDisplay, Transform parent)
	{
		infoDisplay.gameObject.SetActive(true);
		infoDisplay.transform.SetParent(parent);
		infoDisplay.transform.localPosition = new Vector2(0, 0);
	}

	[ClientRpc]
	void RpcShift(Vector2 direction, float delta, int selectedIndex, int vertexIndex)
	{
		LocalShift(direction, delta, selectedIndex, vertexIndex);
	}

	/// <summary>
	/// Triggers <c>InfoDisplayShift</c> on connected clients
	/// </summary>
	/// <param name="selectedIndex">the selected display index</param>
	[ClientRpc]
	void RpcInfoDisplayShift(int selectedIndex)
	{
		this.InfoDisplayShift(selectedIndex);
	}

	private void VertexShift(Vector2 direction, float delta)
	{
		LocalShift(direction, delta, selectedIndex, vertexIndex);
		RpcShift(direction, delta, selectedIndex, vertexIndex);
	}

	private void DisplayShift()
	{
		InfoDisplayShift(selectedIndex);
		RpcInfoDisplayShift(selectedIndex);
	}

	void Update()
	{


		Vector2 direction = Vector2.zero;
		bool anyPressed = false;
		bool noOptions = allOptions.Count == 0;

		if (Input.GetKeyDown(KeyCode.Tab))
		{
			this.vertexIndex = (vertexIndex + 1) % 4;
			if (!noOptions)
			{
				this.VertexShift(direction, 1f);
			}
		}

		if (Input.GetKeyDown(KeyCode.Return))
		{
			this.selectedIndex = (selectedIndex + 1) % allOptions.Count;
			if (!noOptions)
			{
				DisplayShift();

				VertexShift(direction, 1f);
			}

		}

		if (noOptions) { return; }

		if (Input.GetKey(KeyCode.RightArrow))
		{
			direction.x = 1;
			anyPressed = true;
		}
		else if (Input.GetKey(KeyCode.UpArrow))
		{
			direction.y = 1;
			anyPressed = true;
		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			direction.x = -1;
			anyPressed = true;
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			direction.y = -1;
			anyPressed = true;
		}

		if (anyPressed)
		{
			Debug.Log("RealtimeCalibration: isServer = " + isServer);
			if (isServer)
			{
				VertexShift(direction, 0.005f);
			}
		}
	}
}
