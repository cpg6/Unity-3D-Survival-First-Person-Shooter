#pragma strict
public var health : float = 0f;
var acr : GameObject;
var uts : GameObject;
public var playerWalk : boolean = false;
public var playerJump : boolean = false;
public var playerReloadR : boolean =false;
public var playerReloadS : boolean =false;
public var playerShootR : boolean = false;
public var playerShootS : boolean = false;
var HealthGUI : UI.Text;


function Awake(){
	health=100f;
}

function ApplyDamage(damage :int){
	health -= damage;
	yield WaitForSeconds(3);
}

function Update () {
	checkHealth();
	checkMoveState();
	checkWeaponState();
}

function checkHealth(){
	if(health <=0)
	{
		Application.LoadLevel(Application.loadedLevel);
	}
}

function OnTriggerEnter(other: Collider){
	if(other.tag=="shells")
	{
		uts.GetComponent(shootbuckshot).totalcurshells +=30;
		Destroy(other.gameObject);
	}else if(other.tag=="bullets"){
		acr.GetComponent(Raycastshoot).totalcurbullets +=90;
		Destroy(other.gameObject);
	}else if(other.tag=="Health"){
		if(health <=100){
			health +=50;
			if(health >100){
				health =100;
			}
			Destroy(other.gameObject);
		}
	}else if(other.tag=="Exit"){
		Application.LoadLevel(0);
	}else if(other.tag=="Death"){
		health =0;
	}
}

function checkMoveState(){
	if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S))
	{
		playerWalk=true;
	}
	if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.S)){
		playerWalk=false;
	}
	if(Input.GetKeyDown(KeyCode.Space))
	{
		playerJump=true;
	}
	if(Input.GetKeyUp(KeyCode.Space))
	{
		playerJump=false;
	}
}

function checkWeaponState(){
	if(uts.activeInHierarchy)
	{
		if(Input.GetMouseButtonDown(0))
		{
			playerShootS=true;
		}
		if(Input.GetMouseButtonUp(0))
		{
			playerShootS=false;
		}
		playerReloadS=uts.GetComponent(shootbuckshot).reload;
		playerShootR=false;
		playerReloadR=false;
	}
	if(acr.activeInHierarchy)
	{
		if(Input.GetMouseButtonDown(0))
		{
			playerShootR=true;
		}
		if(Input.GetMouseButtonUp(0))
		{
			playerShootR=false;
		}
		playerReloadR=acr.GetComponent(Raycastshoot).reload;
		playerShootS=false;
		playerReloadS=false;
	}
}

function OnGUI(){
	HealthGUI.text="Health :"+health;
}