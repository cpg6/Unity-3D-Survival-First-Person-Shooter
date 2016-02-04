#pragma strict

var variance = 1.0;
var distance = 10.0;
var spreadFactor : float=1.0;
var effect : Transform;
var damage = 7;
var soundshotty : AudioClip;
var shottydelay : float = 0.25;
var maxshells = 180;
var totalcurshells =45;
var curclip = 15;
var maxclip = 15;
public var reload : boolean = false;
//DONT TOUCH temp OR remainder OR ANYTHING BELOW HERE
var ammoGui : UI.Text;
var temp = 0;
var remainder = 0;
GetComponent.<AudioSource>().clip = soundshotty;

function Update () {
	if(Input.GetMouseButtonDown(0))
	{
		if(curclip >=1)
		{
			for(var i=0;i<30;i++)
			{
				shootray();
				if(i >= 29)
				{
					GetComponent.<AudioSource>().Play();
					curclip --;
				}
			}
		}else{
			Reload();
		}
	}
	if(Input.GetKeyDown(KeyCode.R))
	{
		Reload();
	}
}

function shootray(){
	var hit: RaycastHit;
	var dir : Vector3 = transform.forward;
	dir.x += Random.Range(-spreadFactor, spreadFactor);
	dir.y += Random.Range(-spreadFactor, spreadFactor);
	dir.z += Random.Range(-spreadFactor, spreadFactor);
	
	if(Physics.Raycast (transform.position, dir, hit, 100))
	{
		var particleclone = Instantiate(effect,hit.point, Quaternion.LookRotation(hit.normal));
		Destroy(particleclone.gameObject,2);
		hit.transform.SendMessage("ApplyDamage", damage,SendMessageOptions.DontRequireReceiver);
	}
}


function Reload(){
	if(curclip < maxclip && totalcurshells>0)
	{
		temp = totalcurshells%maxclip;
		if(temp==0)
		{
			temp=15;
		}
		totalcurshells -= temp;
		curclip += temp;
		if(curclip > maxclip)
		{
			remainder = curclip - maxclip;
			totalcurshells += remainder;
			curclip = maxclip;
		}
		if(totalcurshells<0)
		{
			totalcurshells=0;
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
	ammoGui.text="UTS :" + curclip + "/" + totalcurshells.ToString();
}

@script RequireComponent(AudioSource);