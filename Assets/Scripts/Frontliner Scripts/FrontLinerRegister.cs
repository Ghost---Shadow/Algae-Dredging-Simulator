using UnityEngine;
using System.Collections;

public class FrontLinerRegister : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void requestWaypoints(FrontLinerScript bot){
		Vector3[] wps = new Vector3[3];
		wps[0] = new Vector3(8,0,-8);
		wps[1] = new Vector3(8,0,8);
		wps[2] = new Vector3(-8,0,8);

		bot.setWaypoints(wps);
	}
}
