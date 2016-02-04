using UnityEngine;
using System.Collections;

public class glockScript : MonoBehaviour {

	public GameObject bullet_prefab;
	AudioSource[] sounds;
	AudioSource shoot, reload;

	weaponMaster weap;
	GameObject player;
	public int clipMax = 15, clipCurrent = 15;
	public int totalBullets;

	public GameObject glock;

	float reloadCooldown = 1f, reloadCooldownMax = 1f;

	//new
	private Vector3 posLock;

	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		weap = player.GetComponent<weaponMaster>(); 			// Get Master Script for ammo info
		totalBullets = weap.getGlockAmmo();

		sounds = GetComponents<AudioSource>();
		reload = sounds[0];
		shoot = sounds[1];
		//Phil added this for current ammo changer 
		if(PlayerPrefs.HasKey("curglock")){
			clipCurrent = PlayerPrefs.GetInt("curglock");
		}

		//new
		posLock = new Vector3 (0.2400008f, -0.323f, 0.4980001f);
	}

	void Update () 
	{
		totalBullets = weap.getGlockAmmo();//Phil Added this, glock was not being updated of ammo update
		reloadCooldown += Time.deltaTime;
		if(clipCurrent <= 0 && totalBullets > 0)
		{
			Reload ();
		}
		PlayerPrefs.SetInt("curglock",clipCurrent);
		PlayerPrefs.SetInt("totalglock",totalBullets);

		this.gameObject.transform.localPosition = posLock;
	}

	public void Shoot()
	{
		if(clipCurrent > 0 && reloadCooldown > reloadCooldownMax)
		{
			clipCurrent--;
			//Debug.Log("Current clip is" + clipCurrent);
			//Debug.Log("Total bullets are" + totalBullets);
			//Debug.Log("Struct bullets are" + weap.getGlockAmmo());
			Instantiate(bullet_prefab, transform.FindChild("muzzle").transform.position, transform.FindChild("muzzle").transform.rotation);
			shoot.Play();
			glock.GetComponent<Animation>().CrossFade("glockfire");
		}
		if(clipCurrent <= 0 ){
			glock.GetComponent<Animation>().Play("glocklock");
		}
	}

	public void Reload()
	{
		if(totalBullets > clipMax && clipCurrent == 0) // If I have more than the whole clip can hold set to max
		{
			clipCurrent = clipMax;
			totalBullets -= clipMax;
			weap.setGlockAmmo(totalBullets); //Update ammo in structure
			reload.Play();
			reloadCooldown = 0f;
			glock.GetComponent<Animation>().CrossFade("glockr2");
			//glock.GetComponent<Animation>().CrossFade("glockidle");
		}

		else if(totalBullets <= clipMax && totalBullets > 0 && clipCurrent ==0) // If i have less than maxClip but more than 0 Swap and Update ammo structure
		{
			clipCurrent = totalBullets;
			totalBullets -= clipCurrent;
			weap.setGlockAmmo(0); // ? This should be correct? Update the structure to have only the remaining loaded bullets?
			reload.Play();
			reloadCooldown = 0f;

			glock.GetComponent<Animation>().CrossFade("glockr2");
			//glock.GetComponent<Animation>().CrossFade("glockidle");
		}

		else if(totalBullets > clipMax && clipCurrent <15) //phil manual reload function
		{
			int ctemp;
			ctemp = clipMax - clipCurrent;
			totalBullets -=ctemp;
			clipCurrent = clipMax;
			weap.setGlockAmmo(totalBullets);
			reload.Play();
			reloadCooldown = 0f;
			glock.GetComponent<Animation>().CrossFade("glockr1");
			//glock.GetComponent<Animation>().CrossFade("glockidle");
		}
		else if(totalBullets <= clipMax && totalBullets > 0 && clipCurrent <15)
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
			weap.setGlockAmmo(totalBullets);
			reload.Play();
			reloadCooldown = 0f;
			glock.GetComponent<Animation>().CrossFade("glockr1");
			//glock.GetComponent<Animation>().CrossFade("glockidle");
		}
	}

}
