#pragma strict

function Start () {
//Application.LoadLevel(0);
}

function Update () {

}

function OnTriggerEnter(collider : Collider){
		if(collider.tag == "Player"){
			Application.LoadLevel(0);
		}
}