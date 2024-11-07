using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;

public class CenterObject : MonoBehaviour
{

	
    public GameObject placementObject;
	
	private ARSessionOrigin arOrigin;
	private Pose placementPose;
	private bool placementPoseIsValid = false;
	private ARRaycastManager aRRaycastManager;
	
	
	private void UpdatePlacementPose()
	{
		var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
		var hits = new List<ARRaycastHit>();
		aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
		
		placementPoseIsValid = hits.Count > 0;
		if (placementPoseIsValid)
		{
			placementPose = hits[0].pose;
		}
	}
	
	private void UpdatePlacementObject()
	{
		
		
		
		if (placementPoseIsValid)
		{
			placementObject.SetActive(true);
			placementObject.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
			placementObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
			placementObject.transform.LookAt(this.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
			
			
		}
		else
		{
			placementObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
			placementObject.transform.LookAt(this.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
			
		}
	}
	
	
	
    void Start()
    {
		arOrigin = FindObjectOfType<ARSessionOrigin>();
		aRRaycastManager = FindObjectOfType<ARRaycastManager>();

		placementObject.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);

	    

    }

    
    void Update()
    {
		UpdatePlacementPose();
		UpdatePlacementObject();
    }
}
