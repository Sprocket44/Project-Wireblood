using UnityEngine;
using System.Collections;

public class RBCharacterController : MonoBehaviour {

	public float speed = 10.0f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	public bool canJump = true;
	public float jumpHeight = 2.0f;
	public float jetPackFuel = 1.0f;
	public float jetPackStr = 5.0f;
	public bool isGrappling = false; 
	private bool grounded = false;
	public bool isSkiing = false;
	private bool isJetPacking = false;
	private float feulRemaining;


	public Animator animator; 
	public Transform CameraTransform; 



	void Awake () {
		float feulRemaining = 5;
		//GetComponent<Rigidbody>().freezeRotation = true;
		GetComponent<Rigidbody>().useGravity = false;
		animator = GetComponentInChildren<Animator> (); 
	}

	void Update(){
		if (isGrappling) {
			GetComponent<Rigidbody> ().freezeRotation = false;
		} else {
			GetComponent<Rigidbody> ().freezeRotation = true;
		}

		if (Input.GetKey (KeyCode.Space)) {
			isSkiing = true;
			GetComponent<CapsuleCollider> ().material = (PhysicMaterial)Resources.Load ("SkiingMaterial");
		} else {
			isSkiing = false;
			GetComponent<CapsuleCollider> ().material = (PhysicMaterial)Resources.Load ("WalkingMaterial");
		}

		//JetPack control

		if (Input.GetMouseButton (1)) {
			isJetPacking = true;
		} else {
			isJetPacking = false; 
		}

		//Check if on ground 
		RaycastHit hit; 
		if (Physics.Raycast (transform.position + new Vector3(0,0.1f,0), -Vector3.up, out hit, 0.3f)) {
			animator.SetBool("landed", true);
			animator.SetBool("isInAir",false);
		} else {
			animator.SetBool("isInAir",true);
			animator.SetBool("landed", false);
		}
		Debug.DrawRay (transform.position + new Vector3(0,1,0), -Vector3.up * 1.1f);

		animator.SetBool("isSkiing",isSkiing);
	}

	void FixedUpdate () {
		Quaternion q = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
		transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * 20);

		//Movement is normal while on the ground and not skiing 
		if (!isSkiing) {

			// Calculate how fast we should be moving
			Vector3 euler = CameraTransform.rotation.eulerAngles;   //get target's rotation
			Quaternion CamRot = Quaternion.Euler(0, euler.y,0); //transpose values
			Vector3 Direction = CamRot * Vector3.forward; 
			//Vector3 CamRot = new Vector3(Camera.main.transform.forward.x, Camera.main.transform.forward.y, Camera.main.transform.forward.z);
			Vector3 ForwardBack = Direction * Input.GetAxis("Vertical");
			Vector3 LeftRight = (Quaternion.AngleAxis (90, Vector3.up) * Direction * Input.GetAxis("Horizontal"));
			Vector3 targetVelocity = ForwardBack + LeftRight;
			targetVelocity *= speed;
			//Debug.DrawRay (CameraTransform.position, Direction);
			//Debug.DrawRay (CameraTransform.position, targetVelocity);

			move (targetVelocity);

			animator.SetFloat("Speed",Input.GetAxis("Vertical"));
			print (Input.GetAxis ("Vertical"));

			// Jump
			if (canJump && Input.GetButton ("Jump")) {
				//GetComponent<Rigidbody> ().velocity = new Vector3 (velocity.x, CalculateJumpVerticalSpeed (), velocity.z);
			} 
		}

		// Forward backward movement isn't possible while skiing and only minor left right course corrections can be made 
		if (!grounded) {
			Vector3 euler = CameraTransform.rotation.eulerAngles;   //get target's rotation
			Quaternion CamRot = Quaternion.Euler(0, euler.y,0); //transpose values
			Vector3 Direction = CamRot * Vector3.forward; 
			Vector3 ForwardBack = Vector3.zero;
			if (Input.GetAxis ("Vertical") < 0) {
				ForwardBack = Direction * Input.GetAxis("Vertical");
			}
			Vector3 LeftRight = (Quaternion.AngleAxis (90, Vector3.up) * Direction * Input.GetAxis("Horizontal"));
			Vector3 targetVelocity = ForwardBack + LeftRight;
			GetComponent<Rigidbody>().AddForce(targetVelocity * 40);


		}

		if (isJetPacking) {
			GetComponent<Rigidbody> ().AddForce (Vector3.up * jetPackStr);
		}

		// We apply gravity manually for more tuning control
		GetComponent<Rigidbody>().AddForce(new Vector3 (0, -gravity * GetComponent<Rigidbody>().mass, 0));
		grounded = false;
	}

	public void move(Vector3 target){
		// Apply a force that attempts to reach our target velocity
		Vector3 velocity = GetComponent<Rigidbody>().velocity;
		Vector3 velocityChange = (target - velocity);
		velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
		velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
		velocityChange.y = 0;
		GetComponent<Rigidbody>().AddForce(velocityChange, ForceMode.VelocityChange);
		//GetComponent<Rigidbody> ().transform.Translate (target.normalized * speed);

	}

	void OnCollisionStay () {
		animator.SetBool("isInAir",false);
		grounded = true;    
	}

	float CalculateJumpVerticalSpeed () {
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * jumpHeight * gravity);
	}


		
}
