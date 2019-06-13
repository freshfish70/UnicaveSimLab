using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for saving the calibration config data
/// for objects that has script <see cref="PhysicalDisplayCalibration" /> attached.
/// 
/// The class can handle idividual displays with <c>PhysicalDisplayCalibration</c> attached to them
///  or a set of displays managed by a <see cref="PhysicalDisplayManager" />.
/// 
/// Author: Christoffer A Træen
/// </summary>
public class PhysicalDisplayCalibrationSaver : MonoBehaviour
{
	/// <summary>
	/// Flag to tell if we are using DisplayManager for 
	/// our displays or not
	/// </summary>
	public bool isUsingDisplayManager = false;

	/// <summary>
	/// Holds the display manager which manages
	/// a set of displays. Where the controlled displays has
	/// <see cref="PhysicalDisplayCalibration" /> scripts attached.
	/// </summary>
	[SerializeField]
	[Tooltip("The display manager parrent")]
	[ConditionalHide("isUsingDisplayManager", false, true)]
	public PhysicalDisplayManager physicalDisplayManager;

	/// <summary>
	/// Holds all instances of displays that 
	/// </summary>
	/// <returns></returns>
	[SerializeField]
	[Tooltip("The display manager parrent")]
	[ConditionalHide("isUsingDisplayManager", true, true)]
	public PhysicalDisplayCalibrationHolder calibratedDisplays = new PhysicalDisplayCalibrationHolder();


	/// <summary>
	/// Check if we are using display manager, if we are
	/// check if a reference is added, if not log an error.
	/// </summary>
	void Start()
	{
		if (isUsingDisplayManager)
		{
			if (this.physicalDisplayManager == null)
			{
				Debug.LogError($"Please provide a Physical display manager: field isUsingDisplayManager is {isUsingDisplayManager}");
			}
		}

	}

	/// <summary>
	/// Saves the calibrations for provided PhysicalDisplayManager
	/// or from the list of calibrated displays in <c>calibratedDisplays</c>.
	/// If is using display manager, calls <see cref="SaveManagerDisplays" /> else
	/// calls <see cref="SaveListOfCalibratedDisplays" />
	/// </summary>
	public void SaveCalibrations()
	{
		if (this.isUsingDisplayManager)
		{
			this.SaveManagerDisplays();
		}
		else
		{
			this.SaveListOfCalibratedDisplays();
		}
	}

	/// <summary>
	/// Loops over all displays on the manager, gets the <c>PhysicalDisplayCalibration</c> reference and calls <see cref="PhysicalDisplayCalibration.SaveWarpFile">
	/// on them to save the WarpFile to disk.
	/// </summary>
	private void SaveManagerDisplays()
	{
		this.physicalDisplayManager.displays.ForEach(display =>
		{
			display.gameObject.GetComponent<PhysicalDisplayCalibration>()?.SaveWarpFile();
		});
	}

	/// <summary>
	/// Loops over all <c>PhysicalDisplayCalibration</c> objects in <c>calibratedDisplays</c> and calls <see cref="PhysicalDisplayCalibration.SaveWarpFile">
	/// on them to save the WarpFile to disk.
	/// </summary>
	private void SaveListOfCalibratedDisplays()
	{
		IEnumerable<PhysicalDisplayCalibration> calibrationEnumerator = this.calibratedDisplays.GetCalibratedDisplays();
		foreach (var display in calibrationEnumerator)
		{
			display.SaveWarpFile();
		}
	}



}
