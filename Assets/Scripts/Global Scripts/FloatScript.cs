using UnityEngine;
using System.Collections;

public class FloatScript : MonoBehaviour {	
	public float springConstant = .5f;

	private Rigidbody rb;

	void Start(){
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate () {		
		float y = transform.position.y;
		rb.AddForce(-Vector3.up*y*springConstant*rb.mass);
	}
}
