#pragma strict

public var fieldOfViewAngel : float = 110f;
public var playerInSight : boolean;
public var personalLastSighting : Vector3;

private var nav : NavMeshAgent;
private var col : SphereCollider;
//private var : Animator;
private var lastPlayerSighting : LastPlayerSighting;
private var player : GameObject;
private var playerHealth : int;
private var previousSighting : Vector3;
//private var hash : HashIDs;
//private var 
function Awake () {
	nav = GetComponent(NavMeshAgent);
	col = GetComponent(SphereCollider);
	lastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent(LastPlayerSighting);
	player = GameObject.FindGameObjectWithTag("Player");
	playerHealth = player.GetComponent(playerManager).health;
	
	personalLastSighting = lastPlayerSighting.resetPosition;
	previousSighting = lastPlayerSighting.resetPosition;
}

function Update () {
	if(lastPlayerSighting.position != previousSighting)
	{
		personalLastSighting = lastPlayerSighting.position;
	}
	
	previousSighting = lastPlayerSighting.position;
	
	//if(playerHealth.health > 0f)*/
}

function OnTriggerStay (other : Collider)
{
	//checks to see if player hits sphere collider that represents field of view
	if(other.gameObject == player)
	{
		playerInSight = false;
		
		//creates vector from enemy to player and stores angle from that and spiders forward vector 
		var direction : Vector3 = other.transform.position - this.transform.position;
		var angle : float = Vector3.Angle(direction, transform.forward);
		
		//
		if(angle < fieldOfViewAngel * 0.5f)
		{
			var hit : RaycastHit;
			
			//check to see if something is between spider and player
			if(Physics.Raycast(transform.position + transform.up, direction.normalized, hit, col.radius))
			{
				//hits player
				Debug.Log("help");
				if(hit.collider.gameObject==player)
				{
					playerInSight =true;
					//lastPlayerSighting.position = player.transform.position;	
				}
			}
		}	
		//the player is doing any action can the spider hear the player
		var playerWalk : boolean = player.GetComponent(playerManager).playerWalk;
		var playerjump : boolean = player.GetComponent(playerManager).playerJump;
		var playershoot1 : boolean = player.GetComponent(playerManager).playerShootR;
		var playerReload1 : boolean = player.GetComponent(playerManager).playerReloadR;
		var playershoot2 : boolean = player.GetComponent(playerManager).playerShootS;
		var playerReload2 : boolean = player.GetComponent(playerManager).playerReloadS;
		if( playerWalk == true || playerjump == true || playershoot1== true || playerReload1 ==true || playershoot2== true || playerReload2 ==true)
		{
			//check to see if the player is within hearing range
			if(CalculatePathLength(player.transform.position) <= col.radius)
			{
				//just set personalLastSighting
				personalLastSighting = player.transform.position;
			}
		}	
	}
}

function OnTriggerExit( other : Collider)
{
	//if player leaves the view of sight collider
	if(other.gameObject == player)
	{
		//player is no longer in sight;
		playerInSight = false;
	}
}

function CalculatePathLength(targetPosition : Vector3)
{
	//creates path and set it based on targets position
	var path : NavMeshPath = new NavMeshPath();
	if(nav.enabled)
	{
		nav.CalculatePath(targetPosition, path);
	}
		//creates an array of points which is the length of the number of corners in the path +2
	var allWayPoints : Vector3[] = new Vector3[path.corners.Length +2];
		
		//first waypoint is position of spider
	allWayPoints[0] = transform.position;
		//last way point is the target position
	allWayPoints[allWayPoints.Length -1] = targetPosition;
		
	for(var i=0; i<path.corners.Length;i++)
	{
		allWayPoints[i+1] = path.corners[i];
	}
		
	//create a float to store path length 
	var pathLength : float = 0;
		
	//increment path length by distance between two Waypoints
	for(var j=0; j < allWayPoints.Length -1; j++)
	{
		pathLength += Vector3.Distance(allWayPoints[j], allWayPoints[j+1]);
	}
	
	return pathLength;
}