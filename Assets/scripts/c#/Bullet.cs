using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	public float existenceTime;
	public Rigidbody bulletPhysics;
	public float shootSpeed;

	// Use this for initialization
	void Start () 
	{
		//transform.rotation = new Quaternion (transform.rotation.x, -transform.rotation.y, transform.rotation.z, transform.rotation.w);
		bulletPhysics = GetComponent<Rigidbody> ();
		Destroy (gameObject, existenceTime);
		bulletPhysics.velocity = transform.TransformDirection(new Vector3(0,0, shootSpeed));
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
