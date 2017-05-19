using UnityEngine;
using System.Collections;

public class GrapplingHook : MonoBehaviour {

	public GameObject grapplePoint; 
	public GameObject grappleGuide; 
	public GameObject grapplePointPreFab; 
	public GameObject grappleGuidePreFab; 
	public GameObject grappleConnection; 
	public Transform grappleOrigin; 
	public float grappleSpeed = 10.0f; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.LeftShift) && !GameObject.Find ("GrappleGuide(Clone)"))
		{
			//Spawn guide at hand 
			Instantiate(grappleGuidePreFab, grappleOrigin.position, Quaternion.identity);
			//Assign it 
			grappleGuide = GameObject.FindGameObjectWithTag ("GrappleGuide");
			//Fire it away from you
			grappleGuide.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * grappleSpeed);
			//Detect when it hits the ground 
		}
			
		if (GameObject.Find ("GrappleGuide(Clone)")) {
			RaycastHit hit;
			if (Physics.Raycast (grappleGuide.transform.position, -Vector3.up, out hit, 1f)) {
				//Destroy guide 
				Destroy (grappleGuide);
				//Spawn grapplepoint at hit location
				Instantiate (grapplePointPreFab, hit.point, Quaternion.identity);
				//Assign it 
				grapplePoint = GameObject.FindGameObjectWithTag ("GrapplePoint");

				grappleConnection = GameObject.FindGameObjectWithTag ("Player");
				grappleConnection.GetComponent<RBCharacterController> ().isGrappling = true; 
				//grapplePoint.GetComponent<ConfigurableJoint> ().connectedBody = grappleConnection.GetComponent<Rigidbody> ();
			}
		}

		if (!Input.GetKey(KeyCode.LeftShift)) {
			print("Release");
			Destroy (grapplePoint);
			grappleConnection.GetComponent<RBCharacterController> ().isGrappling = false; 
		}
	}
}
