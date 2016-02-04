#pragma strict

function Start () {

}

function Update () {

}

function OnTriggerEnter(collider : Collider){
		if(collider.tag == "Player"){
			Application.LoadLevel(29);
		}
}