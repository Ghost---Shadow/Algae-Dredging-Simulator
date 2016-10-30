using UnityEngine;
using System.Collections;

public class SensorScript : MonoBehaviour {

	void OnCollisionEnter(Collision other){
		Debug.Log(other.gameObject.name);
		Debug.Log(transform.parent.position+transform.position);
	}
}
