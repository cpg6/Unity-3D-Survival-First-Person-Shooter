using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(CharacterController))] //whatever uses this class needs a character controller component

public class FPSC : MonoBehaviour 
{
	//playerpref flags
	public int crouchpowerup = 0;
	public int doublejpowerup = 0;
	public int triplejpowerup = 0;
	public int bunnyhoppowerup = 0;
	public int akpowerup = 0;
	public int alienpowerup =0;
	public int shotgunpowerup =0;
	//player stats
	public int curglock;
	public int totalglock;
	public int curak;
	public int totalak;
	public int curshells;
	public int totalshells;
	public int curalien;
	public int totalalien;

	public float health;
	public float armor;

	public float dmg;

	//forward/backward movement
	private float directionZ;
	//strafing << did i spell that wrong?
	private float directionX;
	// since rotation switches x and y axes, variables are named accoridingly
	private float rotationVertical;
	private float rotationHorizontal;
	public float verticalViewLimit; //without this, you'll have the ability to completely flip your char. and look upside down

	//speed
	private float speed; //rate of movement
	public float walkSpeed;
	public float runSpeed;
	//public float speedLimit;

	//gravity/jump
	public float personalGravity;
	private float vertVelocity; //vertical speed and direction
	public float jumpSpeed;
	public float bHopSpeed;
	public bool doubleJump;
	private bool doubleJumped;
	public bool tripleJump;
	private bool tripleJumped;
	public bool bunnyHopping;

	//crouch ability
	public bool crouch;
	private bool crouched;

	//application of all mvmt
	private Vector3 velManager;

	//mouse stuff
	public float mouseSensitvity;
	
	private CharacterController control;

	public bool cursor;

	// CHRIS CODE HERE================================
	int weap;
	float fireRate = .07f, fireCooldown;
	//HUD STUFF
	public Text ammotext;
	public glockScript glockScript;
	public shotgunScript shotgunScript;
	public akScript akScript;
	public alienScript alienScript;
	// ===============================================

	//new
	public GameObject glock, shotgun, ak, alien;

	// Use this for initialization
	void Start () 
	{
		control = GetComponent<CharacterController> ();

		doubleJumped = false;
		tripleJumped = false;
		crouched = false;
		bunnyHopping = false;

		crouchpowerup = PlayerPrefs.GetInt("PlayerCrouch");
		doublejpowerup = PlayerPrefs.GetInt("PlayerDoubleJump");
		triplejpowerup = PlayerPrefs.GetInt("PlayerTripleJump");
		bunnyhoppowerup = PlayerPrefs.GetInt("PlayerBunnyHop");
		alienpowerup = PlayerPrefs.GetInt("alienpowerup");
		akpowerup = PlayerPrefs.GetInt("akpowerup");
		shotgunpowerup = PlayerPrefs.GetInt("shotgunpowerup");
		curglock = PlayerPrefs.GetInt("curglock");
		curak = PlayerPrefs.GetInt("curak");
		curshells = PlayerPrefs.GetInt("curshells");
		curalien = PlayerPrefs.GetInt("curalien");

		health= PlayerPrefs.GetInt("health");
		if(health <=0){
			health =100;
		}
		armor= PlayerPrefs.GetInt("armor");

	}

	// Update is called once per frame
	void Update () 
	{

		curglock = PlayerPrefs.GetInt("curglock");
		totalglock = glockScript.totalBullets;

		curak = PlayerPrefs.GetInt("curak");
		totalak = akScript.totalBullets;

		curshells = PlayerPrefs.GetInt("curshells");
		totalshells = shotgunScript.totalBullets;

		curalien = PlayerPrefs.GetInt("curalien");
		totalalien = alienScript.totalBullets;
		// HUD STUFF
		weap = GetComponent<weaponMaster>().checkWeapon();
		if(weap == 1)
		{
			ammotext.text = "" + glockScript.clipCurrent + "/" + glockScript.totalBullets;
		}
		else if(weap == 2)
		{
			ammotext.text = "" + shotgunScript.clipCurrent + "/" + shotgunScript.totalBullets;
		}
		else if(weap == 3)
		{
			ammotext.text = "" + akScript.clipCurrent + "/" + akScript.totalBullets;
		}
		else if (weap == 4)
		{
			ammotext.text = "" + alienScript.clipCurrent + "/" + alienScript.totalBullets;
		}
		//Check player prefs
		if(PlayerPrefs.HasKey("PlayerCrouch")){
			if(crouchpowerup == 1){
				crouch= true;
			}else{
				crouch = false;
			}
		}
		if(PlayerPrefs.HasKey("PlayerDoubleJump")){
			if(doublejpowerup == 1){
				doubleJump = true;
			}else{
				doubleJump = false;
			}
		}
		if(PlayerPrefs.HasKey("PlayerTripleJump")){
			if(triplejpowerup == 1){
				tripleJump= true;
			}else{
				tripleJump = false;
			}
		}
		if(PlayerPrefs.HasKey("PlayerBunnyHop")){
			if(bunnyhoppowerup == 1){
				bunnyHopping = true;
			}else{
				bunnyHopping = false;
			}
		}
		if(PlayerPrefs.HasKey("alienpowerup")){
			if(alienpowerup == 1){
				GetComponent<weaponMaster>().setAlienUse();
			}
		}
		if(PlayerPrefs.HasKey("akpowerup")){
			if(akpowerup == 1){
				GetComponent<weaponMaster>().setAkUse();
			}
		}
		if(PlayerPrefs.HasKey("shotgunpowerup")){
			if(shotgunpowerup ==1){
				GetComponent<weaponMaster>().setShotgunUse();
			}
		}
		//ROTATION FOR PLAYER AND MAIN CAMERA
		// For Horizontal movement, treated as y axis
		rotationHorizontal = Input.GetAxis ("Mouse X") * mouseSensitvity;
		transform.Rotate (0, rotationHorizontal, 0);
		// For vertical movement, treated as x axis
		rotationVertical -= Input.GetAxis ("Mouse Y") * mouseSensitvity; //moving the minus sign changes the whole equation
		rotationVertical = Mathf.Clamp (rotationVertical, -verticalViewLimit, verticalViewLimit);
		Camera.main.transform.localRotation = Quaternion.Euler (rotationVertical, 0, 0);


		// GRAVITY/JUMPING
		vertVelocity += Physics.gravity.y * personalGravity * Time.deltaTime;

		if (Input.GetButtonDown("Jump") && (control.isGrounded) && !bunnyHopping)
		{
			vertVelocity = jumpSpeed; //<< The action of jumping
		}

		if (Input.GetButton ("Jump") && (control.isGrounded) && bunnyHopping)
			vertVelocity = bHopSpeed;
		
		if (Input.GetButtonDown ("Jump") && (!control.isGrounded) && !doubleJumped && doubleJump && !bunnyHopping)
		{
			vertVelocity = jumpSpeed; //<< The action of double jumping
			doubleJumped = true;
			Debug.Log ("Double Jump");
			return; //If i don't do this, it will try to do both double jump and triple jump at the same time
		}

		if (Input.GetButtonDown ("Jump") && (!control.isGrounded) && doubleJumped && !tripleJumped && tripleJump && !bunnyHopping)
		{
			vertVelocity = jumpSpeed; //<< The action of triple jumping
			tripleJumped = true;
			Debug.Log ("Triple Jump");
		}

		if (control.isGrounded)
		{
			doubleJumped = false;
			tripleJumped = false;
		}


		//PLAYER MOVEMENT/VELOCITY MANAGER
		velManager = new Vector3 (directionX, vertVelocity, directionZ); // this vector is applied to the player creating mvmt
		velManager = transform.rotation * velManager; //how the hell am i multiplying a vector by a quat?!?!?!?!?
		//variables equal whatever number the right side of this operation returns (ex. -1 or backwards if down key/S, 1 is forward if up key/w, 0 if neither)
		directionZ = Input.GetAxis ("Vertical") * speed;
		directionX = Input.GetAxis ("Horizontal") * speed;
		//alternate version: directionZ = Input.GetAxis ("Vertical") * speed * Time.deltaTime;
		//control.SimpleMove ( velManager );
		//Simple move ignores changes to y, handles gravity
		control.Move (velManager * Time.deltaTime); //runs over time, not over frame, DOES NOT deal with gravity however


		//OTHER ABILITIES
		//CROUCHING
		if (Input.GetKeyDown (KeyCode.Return))
		{
			if (crouch)
			{
				crouched = !crouched;
			}
		}

		if (crouched)
			control.height = 1;
		if (!crouched)
			control.height = 2;

		//RUNNING
		if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
		{
			speed = runSpeed;
		}
		else
			speed = walkSpeed;


		// MOUSE CURSOR
		if (cursor == false)
			Cursor.visible = false;
		else
			Cursor.visible = true;


		fireCooldown += Time.deltaTime;
		weap = GetComponent<weaponMaster>().checkWeapon();
		if(Input.GetButtonDown("Fire1") && weap == 1)
		{
			glock.GetComponent<glockScript>().Shoot();
		}
		else if(Input.GetButtonDown("Fire1") && weap == 2)
		{
			shotgun.GetComponent<shotgunScript>().Shoot();
		}
		else if(Input.GetButton("Fire1") && weap == 3 && fireCooldown >= fireRate)
		{
			ak.GetComponent<akScript>().Shoot();
			fireCooldown = 0f;
		}
		else if (Input.GetButtonDown("Fire1") && weap == 4)
		{
			alien.GetComponent<alienScript>().Shoot();
		}

		//health
		if (health <= 0 && armor <= 0)
			Application.LoadLevel ("gameover");

		//reload button
		weap=GetComponent<weaponMaster>().checkWeapon();
		if(Input.GetKeyUp(KeyCode.R) && weap ==1)
		{
			glock.GetComponent<glockScript>().Reload();
		}
		else if(Input.GetKeyUp(KeyCode.R) && weap ==2)
		{
			shotgun.GetComponent<shotgunScript>().Reload();
		}
		else if(Input.GetKeyUp(KeyCode.R) && weap ==3 && fireCooldown >= fireRate)
		{
			ak.GetComponent<akScript>().Reload();
		}
	}

	void OnTriggerEnter(Collider col)
	{
		//Debug.Log("is this working?");
		if (col.gameObject.tag == "Projectile")
		{
			if (armor > 0){
				armor -= dmg;
			}else{
				health -= dmg;
			}
			PlayerPrefs.SetInt("health",(int)health);
			PlayerPrefs.SetInt("armor",(int)armor);
		}
		if (col.gameObject.tag == "spear")
		{
			if (armor > 0){
				armor -= dmg * 1.5f;
			}else{
				health -= dmg * 1.5f;
			}
			PlayerPrefs.SetInt("health",(int)health);
			PlayerPrefs.SetInt("armor",(int)armor);
		}
		if (col.gameObject.tag == "Enemy")
		{
			if (armor > 0){
				armor -= dmg;
			}else{
				health -= dmg;
			}
			PlayerPrefs.SetInt("health",(int)health);
			PlayerPrefs.SetInt("armor",(int)armor);
		}
		if (col.gameObject.tag == "Crouch")
		{
			Destroy(col.gameObject);
			crouchAllow();
			PlayerPrefs.SetInt("PlayerCrouch", 1);
			crouchpowerup =PlayerPrefs.GetInt("PlayerCrouch");
		}
		if (col.gameObject.tag == "DoubleJump")
		{
			Destroy(col.gameObject);
			doubleJumpAllow();
			PlayerPrefs.SetInt("PlayerDoubleJump", 1);
			doublejpowerup = PlayerPrefs.GetInt("PlayerDoubleJump");
		}
		if (col.gameObject.tag == "TripleJump")
		{
			Destroy(col.gameObject);
			tripleJumpAllow();
			PlayerPrefs.SetInt("PlayerTripleJump", 1);
			triplejpowerup = PlayerPrefs.GetInt("PlayerTripleJump");
		}
		if (col.gameObject.tag == "BunnyHop")
		{
			Destroy(col.gameObject);
			bunnyHop();
			PlayerPrefs.SetInt("PlayerBunnyHop", 1);
			bunnyhoppowerup = PlayerPrefs.GetInt("PlayerBunnyHop");
		}
		if ( col.gameObject.tag == "RESET")
		{
			Destroy(col.gameObject);
			ResetPrefs();	
		}
		if (col.gameObject.tag == "health")
		{
			if(health <100){
					Destroy(col.gameObject);
					health +=25;
					if(health >100){
						health =100;
					}
			}
			PlayerPrefs.SetInt("health",(int)health);
		}
		if (col.gameObject.tag =="armor")
		{
			if(armor <200){
				Destroy(col.gameObject);
				armor =200;
			}
				PlayerPrefs.SetInt("armor",(int)armor);
		}
		if (col.gameObject.tag == "glock")
		{
			//ADD AMMO HERE
			Destroy(col.gameObject);
			GetComponent<weaponMaster>().addGlockAmmo();
			totalglock = PlayerPrefs.GetInt("totalglock");
		}
		if(col.gameObject.tag == "ak")
		{
			Destroy(col.gameObject);
			PlayerPrefs.SetInt("akpowerup", 1);
			akpowerup = PlayerPrefs.GetInt("akpowerup");
			GetComponent<weaponMaster>().addAkAmmo();
			GetComponent<weaponMaster>().setAkUse();
			totalak = PlayerPrefs.GetInt("totalak");
			//set active here
		}
		if(col.gameObject.tag == "alien")
		{
			Destroy(col.gameObject);
			PlayerPrefs.SetInt("alienpowerup", 1);
			alienpowerup = PlayerPrefs.GetInt("alienpowerup");
			GetComponent<weaponMaster>().addAlienAmmo();
			GetComponent<weaponMaster>().setAlienUse();
			totalalien = PlayerPrefs.GetInt("totalalien");
			//set active here
		}
		if(col.gameObject.tag == "shotgun")
		{
			Destroy(col.gameObject);
			PlayerPrefs.SetInt("shotgunpowerup", 1);
			shotgunpowerup = PlayerPrefs.GetInt("shotgunpowerup");
			GetComponent<weaponMaster>().addShells();
			GetComponent<weaponMaster>().setShotgunUse();
			totalshells = PlayerPrefs.GetInt("totalshells");
			//set active here
		}
	}

	/*void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "Projectile")
		{
			if (armor > 0){
				armor -= dmg;
			}else{
				health -= dmg;
			}
			PlayerPrefs.SetInt("health",(int)health);
			PlayerPrefs.SetInt("armor",(int)armor);
		}
		if (col.gameObject.tag == "Enemy")
		{
			if (armor > 0){
				armor -= dmg;
			}else{
				health -= dmg;
			}
			PlayerPrefs.SetInt("health",(int)health);
			PlayerPrefs.SetInt("armor",(int)armor);
		}
	}*/

	//FUNCTIONS TO BE CALLED BY BUTTONS
	public void bunnyHop ()
	{
		bunnyHopping = !bunnyHopping;
	}
	public void doubleJumpAllow()
	{
		doubleJump = !doubleJump;
	}
	public void tripleJumpAllow()
	{
		tripleJump = !tripleJump;
	}
	public void crouchAllow()
	{
		crouch = !crouch;
	}
	public void showCursor()
	{
		cursor = !cursor;
	}
	public void ResetPrefs()
	{
		PlayerPrefs.DeleteAll();
	}
}
