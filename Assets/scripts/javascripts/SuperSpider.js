#pragma strict

var wayPoints : Transform[];
var currentWayPoint :int;
var loop : boolean =true;
var fieldOfViewAngle =180;
var Health : int = 100;
var damage : int =20;
var Player : GameObject;
var pp : Transform;
var MoveSpeed : int =0;
var col : SphereCollider;
var hitbox : CapsuleCollider;
var playerHealth : int;
var OnPatrol : boolean =false;
var Chase : boolean =false;
var attack : boolean =false;
var playerInSight : boolean =false;
var playerInHear : boolean =false;
var inAttackRange : boolean = false;
var attackedplayer : boolean =false;
var spiderAttacked : boolean =false;
var pjump : boolean = false;
var pwalk : boolean =false;
var pshoot1 : boolean =false;
var pshoot2 : boolean =false;
var preload1 : boolean =false;
var preload2 : boolean =false;
var check : boolean =false;
var MinDist : int = 5;
var MaxDist : int = 10;

function Awake(){
	Health =250;
	Player = GameObject.FindGameObjectWithTag("Player");
	playerHealth=Player.GetComponent(playerManager).health;
	pjump=Player.GetComponent(playerManager).playerJump;
	pwalk=Player.GetComponent(playerManager).playerWalk;
	pshoot1=Player.GetComponent(playerManager).playerShootR;
	pshoot2=Player.GetComponent(playerManager).playerShootS;
	preload1=Player.GetComponent(playerManager).playerReloadR;
	preload2=Player.GetComponent(playerManager).playerReloadS;
	OnPatrol=true;
	loop=true;
}


function Update () {
	GetPlayerStatus();
	CheckHealth();
	if(playerHealth> 0)
	{
		if(playerInSight ==false && OnPatrol==true && Chase ==false){
			patrol();
		}
		if(inAttackRange ==true && attackedplayer==false){
			attackPlayer();
			delayNextAttack();
		}
		else if(playerInSight== true || playerInHear == true && inAttackRange==false){
			chase();
		}
	}else{
		StopPatrol();
	}
}

function delayNextAttack(){
	if(attackedplayer==true){
		yield WaitForSeconds(3);
		attackedplayer=false;
	}
}

function CheckHealth(){
	if(Health <=0)
	{
		Destroy(gameObject);
	}
}

function GetPlayerStatus(){
		playerHealth=Player.GetComponent(playerManager).health;
		pjump=Player.GetComponent(playerManager).playerJump;
		pwalk=Player.GetComponent(playerManager).playerWalk;
		pshoot1=Player.GetComponent(playerManager).playerShootR;
		pshoot2=Player.GetComponent(playerManager).playerShootS;
		preload1=Player.GetComponent(playerManager).playerReloadR;
		preload2=Player.GetComponent(playerManager).playerReloadS;
}

function patrol(){
	if(OnPatrol == true){
		if(playerInSight == true)
		{
			OnPatrol=false;
		}
		if(currentWayPoint < wayPoints.Length){
			var targetwaypoint : Vector3=wayPoints[currentWayPoint].position;
			var moveDirection : Vector3=targetwaypoint - transform.position;
			transform.LookAt(targetwaypoint);
			if(moveDirection.magnitude <1 ){
				currentWayPoint++;
			}else{
			MoveSpeed=20;
			}
		}else{
			if(loop ==true){
				currentWayPoint = 0;
			}else{
				MoveSpeed=0;
			}
		}
		transform.Translate(MoveSpeed*Vector3.forward*Time.deltaTime);
	}else{
		playerInHear=true;
		playerInSight=true;
	}
}

function chase(){
	OnPatrol=false;
	loop=false;
	MoveSpeed=10;
	transform.LookAt(pp);
	if(Vector3.Distance(transform.position, pp.position)>= MinDist)
	{
		transform.Translate(MoveSpeed*Vector3.forward*Time.deltaTime);
		
		if(Vector3.Distance(transform.position, pp.position) <= 3)
		{
			MoveSpeed = 0;
			inAttackRange=true;
		}else{
			inAttackRange=false;
		}
	}
}

function StopPatrol(){
	OnPatrol =false;
}

function ApplyDamage(damage :int){
	Health -=damage;
	playerInSight=true;
	spiderAttacked =true;
}

function attackPlayer(){
	Chase=false;
	if(attackedplayer==false ){
		Player.SendMessage("ApplyDamage", damage,SendMessageOptions.DontRequireReceiver);
	}
	attackedplayer=true;
}

function OnTriggerStay(other : Collider){
	if(other.gameObject==Player)
	{
		loop=false;
		OnPatrol=false;
		var direction : Vector3 = other.transform.position - this.transform.position;
		var angle : float = Vector3.Angle(direction, transform.forward);
		
		if(angle < fieldOfViewAngle * 0.5f)
		{
			playerInSight=true;
		}
		
		if(pjump == true || pwalk == true || pshoot1 == true || pshoot2 == true || preload1 == true || preload2 == true)
		{
			playerInHear=true;
		}
	}
		
}

function OnTriggerExit(other : Collider){
		if(spiderAttacked ==false){
			playerInSight=false;
		}else{
			playerInSight=true;
		}
		check=false;
		playerInHear=false;
		yield WaitForSeconds(1);
			OnPatrol=true;
			loop=true;
			Chase=false;
}
