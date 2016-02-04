using UnityEngine;
using System.Collections;

public class enemyGun : MonoBehaviour 
{
	public Rigidbody bullet;
	public Vector3 offset;
	public Quaternion rotOffset;
	private bool shooting = true;
	public float waitToShoot;

	public GameObject enemy;
	public enemy enemyScript;

	// Use this for initialization
	void Start () 
	{
		enemy = transform.parent.parent.gameObject;
		enemyScript = enemy.GetComponent<enemy> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!enemyScript.patrolState && enemyScript.chaseState)
		{
			if(shooting)
			{
				Rigidbody instance = Instantiate (bullet, transform.position + offset, transform.rotation) as Rigidbody;
				shooting = false;
				StartCoroutine("shoot");
			}
		}
	}

	IEnumerator shoot()
	{
		yield return new WaitForSeconds(waitToShoot);
		shooting = true;
		yield break;
	}
}
