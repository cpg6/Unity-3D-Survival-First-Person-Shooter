using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour 
{
	public float patrol, chase, patrolPause, chasePause;
	public Transform[] waypoints;
	public Transform player;

	public bool patrolState;
	public bool chaseState;

	public int currentWaypoint;

	//private Vector3 playerOffset;

	public FPSC playerScript;
	public float health;
	public float dmg;
	private int weap;

	public weaponMaster weaponScript;
	//public Vector3 normal;

	//public Rigidbody bullet;
	//public Vector3 offset;
	//public Quaternion rotOffset;
	//private bool shooting = true;
	//public float waitToShoot;

	void Start()
	{
//		transform.position = waypoints [0].position;
//		currentWaypoint = 0;
		patrolState = true;

		//offset = new Vector3 (0, 0, 0);

		//StartCoroutine ("Patrol");
	}

	void Update() //will be patrol state
	{
		weap = weaponScript.checkWeapon();
		//playerOffset = transform.position - player.position;
		//normal = transform.TransformDirection(GetComponent<MeshFilter>().mesh.normals[0]);
		/*
		//if i am at a waypoint, increase currentWaypoint so I move to the next
		if (transform.position == waypoints[currentWaypoint].position)
		    currentWaypoint++;
		//if i am at the last waypoint, reset currentWaypoint so I go back to the first
		if (currentWaypoint >= waypoints.Length)
			currentWaypoint = 0;
		if (patrolState && !chaseState)
			transform.position = Vector3.MoveTowards (transform.position, waypoints [currentWaypoint].position, patrol * Time.deltaTime);
		*/
		if(!patrolState && chaseState)
		{
			//transform.position = Vector3.MoveTowards (transform.position, normal, chase * Time.deltaTime);
			//transform.position += transform.forward * chase * Time.deltaTime;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position), chase * Time.deltaTime);
			transform.position = Vector3.MoveTowards(transform.position, player.position, chase *Time.deltaTime);
		//	if(shooting)
		//	{
		//		Rigidbody instance = Instantiate (bullet, transform.position + offset, transform.rotation) as Rigidbody;
		//		shooting = false;
		//		StartCoroutine("shoot");
		//	}
		}

		/*
		Vector3 direction = transform.position - player.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		//to get player's actual facing direction
		Quaternion updatedRot = Quaternion.AngleAxis(angle + 90, Vector3.forward); //this is the new rotation cooridinates based on the player
		transform.rotation = Quaternion.Slerp(transform.rotation, updatedRot, Time.deltaTime * 10);
		transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x +2, player.position.y +2, player.position.z), Time.deltaTime * chase)
		*/
		if(health <= 0)
			Destroy(gameObject);
	}

	void OnCollisionEnter(Collision col)
	{
		//weapon master
		if (col.gameObject.tag == "bullet2")
		{
			if(weap == 1)
				health -= weaponScript.Glock.damage;
			else if(weap == 2)
				health -= weaponScript.Shotgun.damage;
			else if(weap == 3)
				health -= weaponScript.Ak47.damage;

		}
		else if (col.gameObject.tag == "Alienbullet")
		{
			health -= weaponScript.Alien.damage;
		}
	}

	void OnTriggerStay(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{	
			patrolState = false;
///			Debug.Log ("Chase State");
			chaseState = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			patrolState = true;
			chaseState = false;
		}
	}
	
}
