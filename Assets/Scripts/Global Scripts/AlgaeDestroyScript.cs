using UnityEngine;
using System.Collections;

public class AlgaeDestroyScript : MonoBehaviour {

	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag.Equals("ALGAE_TAG"))
			Destroy(other.gameObject);
	}
}
