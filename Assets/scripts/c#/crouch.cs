using UnityEngine;
using System.Collections;

public class crouch : MonoBehaviour {
	public FPSC player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag=="Player"){
			Destroy(other.gameObject);
		}
	}
}

