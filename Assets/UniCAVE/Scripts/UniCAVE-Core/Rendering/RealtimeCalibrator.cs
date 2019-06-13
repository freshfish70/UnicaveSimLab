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
		List<MeshFilter> toUpdate = new List<MeshFilter>();
		if (calibration.leftChild != null)
		{
			toUpdate.Add(calibration.leftChild.GetComponent<MeshFilter>());
		}
		if (calibration.rightChild != null)
		{
			toUpdate.Add(calibration.rightChild.GetComponent<MeshFilter>());
		}

		foreach (MeshFilter selected in toUpdate)
		{
			Vector3[] verts = selected.sharedMesh.vertices;
			verts[vertexIndex] = new Vector3(verts[vertexIndex].x + direction.x * delta, verts[vertexIndex].y + direction.y * delta, verts[vertexIndex].z);
			selected.sharedMesh.vertices = verts;
			selected.sharedMesh.UploadMeshData(false);
		}

		if (toUpdate.Count != 0)
		{

			Vector3[] verts = toUpdate[0].sharedMesh.vertices;
			calibration.upperRightPosition = new Vector3(verts[0].x / calibration.displayRatio, verts[0].y);
			calibration.upperLeftPosition = new Vector3(verts[1].x / calibration.displayRatio, verts[1].y);
			calibration.lowerLeftPosition = new Vector3(verts[2].x / calibration.displayRatio, verts[2].y);
			calibration.lowerRightPosition = new Vector3(verts[3].x / calibration.displayRatio, verts[3].y);
			calibration.SaveWarpFile();
		}
	}

	/// <summary>
	/// Shifts the info window around to the display on the given index
	/// </summary>
	/// <param name="selectedIndex">index of the display</param>
	private void InfoDisplayShift(int selectedIndex)
	{
		PhysicalDisplayCalibration currentDisplay = this.allOptions[selectedIndex].calibration;

		if (currentDisplay != null && this.display != null)
		{
			this.leftInfoDisplayInstance.gameObject.SetActive(true);
			this.rightInfoDisplayInstance.gameObject.SetActive(true);

			this.leftInfoDisplayInstance.transform.SetParent(currentDisplay.GetLeftWarpObject().transform);
			this.rightInfoDisplayInstance.transform.SetParent(currentDisplay.GetRightWarpObject().transform);

			this.leftInfoDisplayInstance.transform.localPosition = new Vector2(0, 0);
			this.rightInfoDisplayInstance.transform.localPosition = new Vector2(0, 0);
		}

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

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			selectedIndex = (selectedIndex + 1) % allOptions.Count;
			InfoDisplayShift(selectedIndex);
			RpcInfoDisplayShift(selectedIndex);
		}

		if (Input.GetKeyDown(KeyCode.Tab))
		{
			vertexIndex = (vertexIndex + 1) % 4;
		}

		if (allOptions.Count == 0) { return; }

		Vector2 direction = Vector2.zero;
		bool anyPressed = false;
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
				LocalShift(direction, 0.005f, selectedIndex, vertexIndex);
				RpcShift(direction, 0.005f, selectedIndex, vertexIndex);
			}
		}
	}
}
