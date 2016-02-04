#pragma strict
private var doorGUI = false;
var doorprompt : UI.Text;
private var doorclose = true;
var door : Transform;

function OnTriggerEnter (door : Collider){
	if(door.tag == "Player")
	{
		doorGUI = true;
		//Debug.Log("Working");
	}
}
function Update () {
	if( doorGUI == true && Input.GetKeyDown(KeyCode.LeftShift))
	{
		changeDoor();
	}
}

function OnTriggerExit (door : Collider){
	if(door.tag == "Player")
	{
		doorGUI = false;
	}
}

function OnGUI(){
	if(doorGUI == true){
		doorprompt.text = "Press shift to open";
	}
	if(doorGUI == false){
		doorprompt.text = "";
	}
}

function changeDoor(){
	if(doorclose == true){
		door.GetComponent.<Animation>().CrossFade("d2slideopen");
		//door.audio.PlayOneShot();
		doorclose = false;
		yield WaitForSeconds(3);
		door.GetComponent.<Animation>().CrossFade("d2slideclose");
		//door.audio.PlayOneShot();
		doorclose=true;
	}
}