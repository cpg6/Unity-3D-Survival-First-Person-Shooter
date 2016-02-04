using UnityEngine;
using System.Collections;

public class alienScript : MonoBehaviour {

	public GameObject bullet_prefab;
	AudioSource[] sounds;
	AudioSource shoot, reload;

	weaponMaster weap;
	GameObject player;
	public int clipMax = 1, clipCurrent = 1;
	public int totalBullets;

	public GameObject alien;
	float reloadCooldown = 6.245f, reloadCooldownMax = 6.245f;

	//new
	private Vector3 posLock;

	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		weap = player.GetComponent<weaponMaster>(); 			// Get Master Script for ammo info
		totalBullets = weap.getAlienAmmo();

		sounds = GetComponents<AudioSource>();
		reload = sounds[0];
		shoot = sounds[1];

		//phil add this for current ammo count 
		if(PlayerPrefs.HasKey("curalien")){				
			clipCurrent = PlayerPrefs.GetInt("curalien");
		}
		if(PlayerPrefs.HasKey("totalalien")){
			totalBullets = PlayerPrefs.GetInt("totalalien");
		}

		posLock = new Vector3 (-0.05254519f, -0.5188586f, 0.5694532f);
	}
	void Update () 
	{
		totalBullets = weap.getAlienAmmo();//Phil Added this, glock was not being updated of ammo update
		reloadCooldown += Time.deltaTime;
		if(clipCurrent <= 0 && totalBullets > 0)
		{
			Reload ();
		}

		PlayerPrefs.SetInt("curalien",clipCurrent);
		PlayerPrefs.SetInt("totalalien",totalBullets);

		this.gameObject.transform.localPosition = posLock;
	}
	
	public void Shoot()
	{
		if(clipCurrent > 0 && reloadCooldown > reloadCooldownMax)
		{
			clipCurrent--;
			//Debug.Log("Current clip is" + clipCurrent);
			//Debug.Log("Total bullets are" + totalBullets);
			//Debug.Log("Struct bullets are" + weap.getAlienAmmo());
			Instantiate(bullet_prefab, transform.FindChild("muzzle").transform.position, transform.FindChild("muzzle").transform.rotation);
			shoot.Play();
			alien.GetComponent<Animation>().CrossFade("alienfire");
		}
	}
	
	public void Reload()
	{
		if(totalBullets > clipMax) // If I have more than the whole clip can hold set to max
		{
			clipCurrent = clipMax;
			totalBullets -= clipMax;
			weap.setAlienAmmo(totalBullets); //Update ammo in structure
			reload.Play();
			reloadCooldown = 0f;
			alien.GetComponent<Animation>().CrossFade("alienr");
		}
	
		else if(totalBullets <= clipMax && totalBullets > 0) // If i have less than maxClip but more than 0 Swap and Update ammo structure
		{
			clipCurrent = totalBullets;
			totalBullets -= clipCurrent;
			weap.setAlienAmmo(0); // ? This should be correct? Update the structure to have only the remaining loaded bullets?
			reload.Play();
			reloadCooldown = 0f;
			alien.GetComponent<Animation>().CrossFade("alienr");
		}
	}
}
