using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject player; 
	public Transform followTarget;
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	public float minimumX = -360F;
	public float maximumX = 360F;
	public float minimumY = -60F;
	public float maximumY = 60F;
	public float walkingFOV = 90; 
	public float skiingFOV = 95; 
	public float FOVTransistionSpeed = 5; 
	float rotationX = 0F;
	float rotationY = 0F;
	public float smoothTime = 0.1F;
	private Vector3 velocity = Vector3.zero;


	Quaternion originalRotation;
	void Update ()
	{
		SmoothFollow (); 

		//Increase fov slightly if player is skiiing 
		if (player.GetComponent<RBCharacterController>().isSkiing) {
			float curFov = Camera.main.fieldOfView;
			Camera.main.fieldOfView = Mathf.Lerp(curFov, skiingFOV, FOVTransistionSpeed * Time.deltaTime);
		} else {
			float curFov = Camera.main.fieldOfView;
			Camera.main.fieldOfView = Mathf.Lerp(curFov, walkingFOV, FOVTransistionSpeed * Time.deltaTime);
		}

		//lean slightly in direction of movement 
		Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity; 
		//Vector3 curPos = Camera.main.transform.position; 


		if (axes == RotationAxes.MouseXAndY)
		{
			// Read the mouse input axis
			rotationX += Input.GetAxis("Mouse X") * sensitivityX;
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationX = ClampAngle (rotationX, minimumX, maximumX);
			rotationY = ClampAngle (rotationY, minimumY, maximumY);
			Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
			Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, -Vector3.right);
			transform.localRotation = originalRotation * xQuaternion;
			Camera.main.transform.localRotation = originalRotation * yQuaternion;
		}
		else if (axes == RotationAxes.MouseX)
		{
			rotationX += Input.GetAxis("Mouse X") * sensitivityX;
			rotationX = ClampAngle (rotationX, minimumX, maximumX);
			Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
			transform.localRotation = originalRotation * xQuaternion;
		}
		else
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = ClampAngle (rotationY, minimumY, maximumY);
			Quaternion yQuaternion = Quaternion.AngleAxis (-rotationY, Vector3.right);
			transform.localRotation = originalRotation * yQuaternion;
		}
	}
	void Start ()
	{
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
		originalRotation = transform.localRotation;
	}
	public static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp (angle, min, max);
	}

	public IEnumerator IncreaseFov(){
		for (float i = walkingFOV; i < skiingFOV; i++){
			Camera.main.fieldOfView = i; 
			yield return new WaitForSeconds (1);
		}
	}

	void SmoothFollow () {
		//Smoothly follow rotation 
		Quaternion targetRotation = followTarget.transform.rotation; 
		transform.rotation = Quaternion.Slerp (transform.localRotation, targetRotation, Time.deltaTime);

		//Exactly follow position
		Vector3 targetLocation = followTarget.transform.position;
		if (velocity.sqrMagnitude > 0) {
			transform.position = -Vector3.Lerp (transform.position, targetLocation + new Vector3 (0, 0, 0), Time.deltaTime * smoothTime);  
		} else {
			transform.position = followTarget.transform.position + new Vector3(0, 0, 0); 
		}
		//transform.position = followTarget.transform.position + new Vector3(0,2,0); 	

	}
}
