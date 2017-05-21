using UnityEngine;
using System.Collections;

public class RotatePlayerMesh : MonoBehaviour {
	public Transform target;
	public float smoothTime = 0.1F;
	private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 euler = target.rotation.eulerAngles;   //get target's rotation
		Quaternion rot = Quaternion.Euler(0, euler.y, 0); //transpose values
		transform.rotation = rot;   
	}
}
