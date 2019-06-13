using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for saving the calibration config data
/// of physical display calibration scripts for
/// displays managed by a PhysicalDisplayManager.
/// 
/// We retrieve all displays from the PhysicalDisplayManager.
/// 
/// Author: Christoffer A Træen
/// </summary>
[RequireComponent(typeof(PhysicalDisplayManager))]
public class PhysicalDisplayCalibrationSaver : MonoBehaviour
{
    /// <summary>
    /// Holds the display manager which manages
    /// a set of displays. Where the controlled displays has
    /// PhysicalDisplayCalibration scripts attached.
    /// </summary>
    [SerializeField]
    [Tooltip("The display manager parrent")]
    private PhysicalDisplayManager physicalDisplayManager;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
