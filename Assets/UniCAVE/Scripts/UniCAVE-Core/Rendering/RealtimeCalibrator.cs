using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class RealtimeCalibrator : NetworkBehaviour
{
    public struct CalibrationSelection
    {
        public string machineName;
        public PhysicalDisplayCalibration calibration;
    }

    private enum CalibrationType
    {
        VERTEX,
        POSITION,
        ROTATION
    }

    private CalibrationType calibrationType = CalibrationType.VERTEX;

    public int selectedIndex = 0;
    public int lastSelectedIndex = 0;
    public List<CalibrationSelection> allOptions;

    public int vertexIndex = 0;

    /// <summary>
    /// Holds a reference of the display object to be shown
    /// when calibrating
    /// </summary>
    [SerializeField]
    private InfoDisplay infoDisplay;

    /// <summary>
    /// The instantiated instance of InfoDisplay for the right eye/cam
    /// </summary>
    private InfoDisplay infoDisplayInstance;

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
        if (this.infoDisplay == null) return;
        this.infoDisplayInstance = Instantiate(infoDisplay);
        this.infoDisplayInstance.gameObject.SetActive(false);
    }

    private void LocalShift(Vector2 direction, float delta, int selectedIndex, int vertexIndex)
    {
        PhysicalDisplayCalibration lastCalibration = allOptions[lastSelectedIndex].calibration;
        lastCalibration.HideVisualMarker();
        PhysicalDisplayCalibration calibration = allOptions[selectedIndex].calibration;
        calibration.SetVisualMarkerVertextPoint(vertexIndex);

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
            meshFilter.mesh.RecalculateBounds();
            meshFilter.mesh.RecalculateTangents();
        }

        calibration.UpdateMeshPositions(lastWarpedFilter?.sharedMesh.vertices);

    }

    private void LocalPositionShift(Vector3 direction, float delta, int selectedIndex)
    {
        PhysicalDisplayCalibration lastCalibration = allOptions[lastSelectedIndex].calibration;
        lastCalibration.HideVisualMarker();
        PhysicalDisplayCalibration calibration = allOptions[selectedIndex].calibration;
        calibration.SetVisualMarkerVertextPoint(vertexIndex);

        calibration.MoveDisplay(new Vector3(direction.x * delta, direction.y * delta, direction.z * delta));
    }

    private void LocalRotationShift(Vector3 direction, float delta, int selectedIndex)
    {
        PhysicalDisplayCalibration lastCalibration = allOptions[lastSelectedIndex].calibration;
        lastCalibration.HideVisualMarker();
        PhysicalDisplayCalibration calibration = allOptions[selectedIndex].calibration;
        calibration.SetVisualMarkerVertextPoint(vertexIndex);

        calibration.RotateDisplay(new Vector3(direction.x * delta, direction.y * delta, direction.z * delta));
    }

    /// <summary>
    /// Shifts the info window around to the display on the given index
    /// </summary>
    /// <param name="selectedIndex">index of the display</param>
    private void InfoDisplayShift(int selectedIndex)
    {
        PhysicalDisplayCalibration currentDisplay = this.allOptions[selectedIndex].calibration;
        if (currentDisplay == null || this.infoDisplayInstance == null) return;

        if (currentDisplay.GetDisplayWarpsValues().Count() > 0)
        {
            this.SetInfoDisplay(infoDisplayInstance.gameObject, currentDisplay.GetDisplayWarpsValues().First().GetDewarpGameObject().transform);
            this.infoDisplayInstance.SetText(this.calibrationType.ToString());
        }
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

    [ClientRpc]
    void RpcMovePosition(Vector2 direction, float delta, int selectedIndex)
    {
        LocalPositionShift(direction, delta, selectedIndex);
    }

    [ClientRpc]
    void RpcRotate(Vector2 direction, float delta, int selectedIndex)
    {
        LocalRotationShift(direction, delta, selectedIndex);
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

    private void PositionShift(Vector3 direction, float delta)
    {
        this.LocalPositionShift(direction, delta, selectedIndex);
        this.RpcMovePosition(direction, delta, selectedIndex);
    }

    private void RotationShift(Vector3 direction, float delta)
    {
        this.LocalRotationShift(direction, delta, selectedIndex);
        this.RpcRotate(direction, delta, selectedIndex);
    }

    private void DisplayShift()
    {
        InfoDisplayShift(selectedIndex);
        RpcInfoDisplayShift(selectedIndex);
    }

    private CalibrationType CycleNextCalibrationType()
    {
        this.calibrationType = (from CalibrationType val in Enum.GetValues(typeof(CalibrationType))
                                where val > this.calibrationType
                                orderby val
                                select val).DefaultIfEmpty().First();

        this.infoDisplayInstance.SetText(this.calibrationType.ToString());
        return this.calibrationType;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.CycleNextCalibrationType();
        }

        Vector3 direction = Vector3.zero;
        bool anyPressed = false;
        bool noOptions = allOptions.Count == 0;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            this.vertexIndex = (vertexIndex + 1) % 15;
            if (!noOptions)
            {
                this.VertexShift(direction, 1f);
            }
        }


        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                int index = this.selectedIndex - 1;
                if (index < 0)
                {
                    this.selectedIndex = allOptions.Count - 1;
                }
                else
                {
                    this.selectedIndex = Mathf.Abs((selectedIndex - 1) % allOptions.Count);
                }
                if (!noOptions)
                {
                    DisplayShift();
                    VertexShift(direction, 1f);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            this.selectedIndex = (selectedIndex + 1) % allOptions.Count;
            Debug.Log(selectedIndex);
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
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftControl))
            {
                direction.z = 1;
            }
            else
            {
                direction.y = 1;
            }
            anyPressed = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction.x = -1;
            anyPressed = true;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftControl))
            {
                direction.z = -1;
            }
            else
            {
                direction.y = -1;
            }
            anyPressed = true;
        }

        if (anyPressed)
        {
            Debug.Log("RealtimeCalibration: isServer = " + isServer);
            if (isServer)
            {
                switch (this.calibrationType)
                {
                    case CalibrationType.POSITION:
                        this.PositionShift(direction, 0.003f);
                        break;
                    case CalibrationType.ROTATION:
                        this.RotationShift(direction, 0.15f);
                        break;
                    case CalibrationType.VERTEX:
                        this.VertexShift(direction, 0.003f);
                        break;
                }

            }
        }
        this.lastSelectedIndex = this.selectedIndex;
    }
}
