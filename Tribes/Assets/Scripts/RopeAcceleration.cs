using UnityEngine;
using System.Collections;

public class RopeAcceleration : MonoBehaviour {
	private Vector3 String;
	public GameObject Player; 
	public const float SpringConstant = 1000; 
	private Vector3 playerAccel;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag("Player");
		String = transform.position - Player.transform.position; 
	}
	
	// Update is called once per frame
	void Update () {
		//Calculate vector representing tether 
		Vector3 curString = transform.position - Player.transform.position;  
		float difference = curString.magnitude - String.magnitude;
		if (difference < 0) {
			difference = 0;
		}
		playerAccel = difference * curString.normalized; 

	}

	void FixedUpdate(){
		Player.GetComponent<Rigidbody> ().AddForce (playerAccel); 
	}
}
