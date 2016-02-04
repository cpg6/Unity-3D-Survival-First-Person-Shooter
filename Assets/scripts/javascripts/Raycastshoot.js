#pragma strict

var effect : Transform;
var damage =50;
var soundrifle : AudioClip;
var maxbullets = 300;
var totalcurbullets =210;
var curclip = 30;
var maxclip =30;
public var reload : boolean = false;
//DONT TOUCH temp OR remainder OR ANYTHING BELOW HERE
var ammoGui : UI.Text;
var temp = 0;
var remainder = 0;
GetComponent.<AudioSource>().clip= soundrifle;
var targetHP : int;
//var target : GameObject;

function Update () {
	var hit: RaycastHit;
	var ray: Ray = Camera.main.ScreenPointToRay(Vector3(Screen.width * 0.5,Screen.height * 0.5, 0));
	
	 if(Input.GetKeyDown(KeyCode.R) && curclip<30){
	 	Reload();
	 }
	 if(Input.GetMouseButtonDown(0))
	 {
	 	if(Physics.Raycast(ray, hit, 100))
	 	{
	 		if(curclip >=1){
	 			var particleclone = Instantiate(effect,hit.point, Quaternion.LookRotation(hit.normal));
	 			Destroy(particleclone.gameObject,2);
	 			if(hit.transform.tag=="Enemy")
	 			{
	 				hit.transform.SendMessage("ApplyDamage", damage,SendMessageOptions.DontRequireReceiver);
	 			}
	 			curclip--;
	 			GetComponent.<AudioSource>().Play();
	 		}else{
	 			Reload();
	 		}
	 	}
	 } 
}

function Reload(){
	if(curclip < maxclip && totalcurbullets>0)
	{
		temp = totalcurbullets%maxclip;
		if(temp==0)
		{
			temp=30;
		}
		totalcurbullets -= temp;
		curclip += temp;
		if(curclip > maxclip)
		{
			remainder = curclip - maxclip;
			totalcurbullets += remainder;
			curclip = maxclip;
		}
		if(totalcurbullets < 0)
		{
			totalcurbullets=0;
		}
		reload=true;
	}
	if(reload==true)
	{
		yield WaitForSeconds(.25);
		reload=false;
	}
}

function OnGUI(){
	ammoGui.text="ACR :" + curclip + "/" + totalcurbullets.ToString();
}

@script RequireComponent(AudioSource);