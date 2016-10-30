using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {
	private FrontLinerRegister registry;
	public Material[] materials;
	public int x,y;
	void Start () {
		registry = GameObject.Find("FrontlinerContainer").GetComponent<FrontLinerRegister>();
		x = ((int)transform.position.x + (registry.sizeX-1))/2;
        y = ((int)transform.position.z + (registry.sizeY-1))/2;

		//registry.map[x,y] = FrontLinerRegister.BLOCKED;
	}

	void Update(){
		if(registry.map[x,y] == FrontLinerRegister.BLOCKED){
			GetComponent<Renderer>().material = materials[1];
		} else {
			GetComponent<Renderer>().material = materials[0];
		}
	}

	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag.Equals("FRONTLINER_TAG")){
			registry.map[x,y] = FrontLinerRegister.BLOCKED;
			other.gameObject.GetComponent<FrontLinerScript>().requestNewWaypoints();
		}
	}
	
}
