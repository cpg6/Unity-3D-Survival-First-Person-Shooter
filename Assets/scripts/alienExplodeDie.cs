using UnityEngine;
using System.Collections;

public class alienExplodeDie : MonoBehaviour {

	float lifetime;
	
	void Update () 
	{
		lifetime += Time.deltaTime;
		if(lifetime > 2)
			Destroy(gameObject);
	}
}
