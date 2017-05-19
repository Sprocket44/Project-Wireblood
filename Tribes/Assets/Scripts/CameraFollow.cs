using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	public float smoothTime = 10F;
	private Vector3 velocity = Vector3.zero;

	/*void Update () {
		Vector3 targetPosition = target.TransformPoint (new Vector3 (0, 2, 0));
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
	}*/

	void Update () {
		//Smoothly follow rotation 
		Quaternion targetRotation = target.rotation; 
		transform.rotation = Quaternion.Slerp (transform.localRotation, targetRotation, Time.deltaTime);

		//Exactly follow position
		transform.position = target.transform.position + new Vector3(0,2,0); 
	}
}
