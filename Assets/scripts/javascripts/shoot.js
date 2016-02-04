#pragma strict

var bullet : Rigidbody;
var speed = 10;

function Update () {
	if(Input.GetButton("Fire1")){
		var clone = Instantiate(bullet, transform.position, transform.rotation);
		clone.velocity = transform.TransformDirection(Vector3 (0,0, speed));
		Destroy(clone.gameObject, 5);
	}
}