using UnityEngine;
using System.Collections;

public class shotgunScript : MonoBehaviour {

	public GameObject bullet_prefab;	
	AudioSource[] sounds;
	AudioSource shoot, reload;

	weaponMaster weap;
	GameObject player;
	public int clipMax = 3, clipCurrent = 3;
	public int totalBullets;
	Vector3 dir;
	public GameObject shotgun;

	float reloadCooldown = 3.331f, reloadCooldownMax = 3.331f;

	//new
	private Vector3 posLock;

	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		weap = player.GetComponent<weaponMaster>(); 			// Get Master Script for ammo info
		totalBullets = weap.getShotgunAmmo();

		sounds = GetComponents<AudioSource>();
		reload = sounds[0];
		shoot = sounds[1];

		//phil add this for current ammo count 
		if(PlayerPrefs.HasKey("curshells")){				
			clipCurrent = PlayerPrefs.GetInt("curshells");
		}
		if(PlayerPrefs.HasKey("totalshells")){
			totalBullets = PlayerPrefs.GetInt("totalshells");
		}

		//this.gameObject.transform.localPosition = posLock;
		posLock = new Vector3 (0.09000088f, -0.22f, 0.3699999f);
	}
	void Update () 
	{
		totalBullets = weap.getShotgunAmmo();//Phil Added this, glock was not being updated of ammo update
		reloadCooldown += Time.deltaTime;
		if(clipCurrent <= 0 && totalBullets > 0)
		{
			Reload ();
		}
		PlayerPrefs.SetInt("curshells",clipCurrent);
		PlayerPrefs.SetInt("totalshells",totalBullets);

		this.gameObject.transform.localPosition = posLock;
	}

	public void Shoot()
	{
		if(clipCurrent > 0 && reloadCooldown > reloadCooldownMax)
		{
			clipCurrent--;
			//Debug.Log("Current clip is" + clipCurrent);
			//Debug.Log("Total bullets are" + totalBullets);
			//Debug.Log("Struct bullets are" + weap.getShotgunAmmo());
			for (int i = 0; i < 8; i++)
			{
				dir.x = Random.Range(-10, 10);
				dir.y = Random.Range(-10, 10);
				Quaternion adjusted = transform.FindChild("muzzle").transform.rotation;
				adjusted = Quaternion.RotateTowards(adjusted, transform.rotation*Quaternion.FromToRotation(transform.forward,transform.up), dir.y);
				adjusted = Quaternion.RotateTowards(adjusted, transform.rotation*Quaternion.FromToRotation(transform.forward,transform.right), dir.x);
				Instantiate(bullet_prefab, transform.FindChild("muzzle").transform.position, adjusted);
			}
			shoot.Play();
			shotgun.GetComponent<Animation>().CrossFade("shotgunfire");

		}
	}
	
	public void Reload()
	{
		if(totalBullets > clipMax && clipCurrent ==0) // If I have more than the whole clip can hold set to max
		{
			clipCurrent = clipMax;
			totalBullets -= clipMax;
			weap.setShotgunAmmo(totalBullets); //Update ammo in structure
			reload.Play();
			reloadCooldown = 0f;
			shotgun.GetComponent<Animation>().CrossFade("shotgunreload");
		}

		else if(totalBullets <= clipMax && totalBullets > 0 && clipCurrent ==0) // If i have less than maxClip but more than 0 Swap and Update ammo structure
		{
			clipCurrent = totalBullets;
			totalBullets -= clipCurrent;
			weap.setShotgunAmmo(0); // ? This should be correct? Update the structure to have only the remaining loaded bullets?
			reload.Play();
			reloadCooldown = 0f;
			shotgun.GetComponent<Animation>().CrossFade("shotgunreload");
		}

		else if(totalBullets > clipMax && clipCurrent < 3) //phil manual reload function
		{
			int ctemp;
			ctemp = clipMax - clipCurrent;
			totalBullets -= ctemp;
			clipCurrent = clipMax;
			weap.setShotgunAmmo(totalBullets);
			reload.Play();
			reloadCooldown = 0f;
			shotgun.GetComponent<Animation>().CrossFade("shotgunreload");
		}

		else if(totalBullets <= clipMax && totalBullets > 0 && clipCurrent <3)
		{
			int ctemp2;
			ctemp2= clipMax - clipCurrent;
			if(ctemp2 < totalBullets){
				clipCurrent += ctemp2;
				totalBullets -= ctemp2;
			}else{
				clipCurrent += totalBullets;
				totalBullets =0;
			}
			weap.setShotgunAmmo(totalBullets);
			reload.Play();
			reloadCooldown = 0f;
			shotgun.GetComponent<Animation>().CrossFade("shotgunreload");
			//glock.GetComponent<Animation>().CrossFade("glockr1");
			//glock.GetComponent<Animation>().CrossFade("glockidle");
		}
	}
}
