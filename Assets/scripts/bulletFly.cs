using UnityEngine;
using System.Collections;

public class bulletFly : MonoBehaviour {

	public Rigidbody bullet;
	GameObject player, explode;
	public int glockBulletSpeed, shotgunBulletSpeed, akBulletSpeed, alienBulletSpeed;
	public int glockBulletDmg, shotgunBulletDmg, akBulletDmg, alienBulletDmg;
	weaponMaster weap;

	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		weap = player.GetComponent<weaponMaster>();

		explode = Resources.Load("alienexplode", typeof(GameObject)) as GameObject;

		if(weap.checkWeapon() == 1)
		{
			bullet.velocity = transform.forward * glockBulletSpeed;
		}
		else if(weap.checkWeapon() == 2)
		{
			bullet.velocity = transform.forward * shotgunBulletSpeed;
		}
		else if(weap.checkWeapon() == 3)
		{
			bullet.velocity = transform.forward * akBulletSpeed;
		}
		else if(weap.checkWeapon() == 4)
		{
			bullet.velocity = transform.forward * alienBulletSpeed;
		}
	}

	void Update () 
	{

	}

	void OnCollisionEnter(Collision col)
	{
		if(weap.checkWeapon() == 4)
			Instantiate (explode, col.transform.position, col.transform.rotation);
		Destroy(gameObject);
	}
}
