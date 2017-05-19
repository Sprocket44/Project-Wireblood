using UnityEngine;
using System.Collections;

public class WireEffect : MonoBehaviour {

	private LineRenderer lineRenderer; 
	public Transform origin; 
	public Transform destination; 

	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
