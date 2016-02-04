using UnityEngine;
using System.Collections;

public class akScript : MonoBehaviour {

	public GameObject bullet_prefab;
	AudioSource[] sounds;
	AudioSource shoot, reload;

	weaponMaster weap;
	GameObject player;
	public int clipMax = 30, clipCurrent = 30;
	public int totalBullets;

	public GameObject ak;

	float reloadCooldown = 3f, reloadCooldownMax = 3f;

	//new
	private Vector3 posLock;

	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		weap = player.GetComponent<weaponMaster>(); 			// Get Master Script for ammo info
		totalBullets = weap.getAkAmmo();

		sounds = GetComponents<AudioSource>();
		reload = sounds[0];
		shoot = sounds[1];
		//phil add this for current ammo count 
		if(PlayerPrefs.HasKey("curak")){				
			clipCurrent = PlayerPrefs.GetInt("curak");
		}
		if(PlayerPrefs.HasKey("totalak")){
			totalBullets = PlayerPrefs.GetInt("totalak");
		}

		posLock = new Vector3 (0.05100221f, -0.8060001f, 0.604f);
	}
	void Update () 
	{
		totalBullets=weap.getAkAmmo();//phil added this
		reloadCooldown += Time.deltaTime;
		if(clipCurrent <= 0 && totalBullets > 0)
		{
			Reload ();
		}
		PlayerPrefs.SetInt("curak",clipCurrent);
		PlayerPrefs.SetInt("totalak",totalBullets);

		this.gameObject.transform.localPosition = posLock;
	}
	
	public void Shoot()
	{
		if(clipCurrent > 0 && reloadCooldown > reloadCooldownMax)
		{
			clipCurrent--;
			//Debug.Log("Current clip is" + clipCurrent);
			//Debug.Log("Total bullets are" + totalBullets);
			//Debug.Log("Struct bullets are" + weap.getAkAmmo());
			Instantiate(bullet_prefab, transform.FindChild("muzzle").transform.position, transform.FindChild("muzzle").transform.rotation);
			shoot.Play();
			ak.GetComponent<Animation>().CrossFade("akfire");
		}
	}
	
	public void Reload()
	{
		if(totalBullets > clipMax && clipCurrent == 0) // If I have more than the whole clip can hold set to max
		{
			clipCurrent = clipMax;
			totalBullets -= clipMax;
			weap.setAkAmmo(totalBullets); //Update ammo in structure
			reload.Play();
			reloadCooldown = 0f;
			ak.GetComponent<Animation>().CrossFade("akr1");
		}
		
		else if(totalBullets <= clipMax && totalBullets > 0 && clipCurrent ==0) // If i have less than maxClip but more than 0 Swap and Update ammo structure
		{
			clipCurrent = totalBullets;
			totalBullets -= clipCurrent;
			weap.setAkAmmo(0); // ? This should be correct? Update the structure to have only the remaining loaded bullets?
			reload.Play();
			reloadCooldown = 0f;
			ak.GetComponent<Animation>().CrossFade("akr1");
		}

		else if(totalBullets > clipMax && clipCurrent < 30){
			int ctemp;
			ctemp = clipMax - clipCurrent;
			totalBullets -= ctemp;
			clipCurrent = clipMax;
			weap.setAkAmmo(totalBullets);
			reload.Play();
			reloadCooldown = 0f;
			ak.GetComponent<Animation>().CrossFade("akr1");
		}

		else if(totalBullets <= clipMax && totalBullets > 0 && clipCurrent <30)
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
			weap.setAkAmmo(totalBullets);
			reload.Play();
			reloadCooldown = 0f;
			ak.GetComponent<Animation>().CrossFade("akr1");
		}
	}
}
