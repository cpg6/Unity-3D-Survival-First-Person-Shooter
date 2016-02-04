using UnityEngine;
using System.Collections;

public class weaponMaster : MonoBehaviour {
	
	public struct Weapon
	{
		public Weapon(int dmg, int coil, int bullets, bool flagCanUse, bool flagInUse)
		{
			ammo = bullets;
			damage = dmg;
			recoil = coil;
			canUse = flagCanUse;
			inUse = flagInUse;
		}
		public int damage, recoil, ammo;
		public bool canUse, inUse;
	}

	public Weapon Glock = new Weapon(25, 0, 45, true, true);
	public Weapon Shotgun = new Weapon(10, 5, 0, false, false);
	public Weapon Ak47 = new Weapon(35, 7, 0, false, false);
	public Weapon Alien = new Weapon(100, 10, 0, false, false);
	public GameObject wGlock, wShotgun, wAk47, wAlien;

	void Start () 
	{
		//wGlock = transform.Find("glock").gameObject;
		//wShotgun = transform.Find ("shotgun").gameObject;
		//wAk47 = transform.Find("ak47").gameObject;
		//wAlien = transform.Find ("alien").gameObject;
		wGlock.SetActive(true);
		wShotgun.SetActive(false);
		wAk47.SetActive(false);
		wAlien.SetActive(false);
		if(PlayerPrefs.HasKey("totalglock")){
			Glock.ammo=PlayerPrefs.GetInt("totalglock");
		}
		if(PlayerPrefs.HasKey("totalak")){
			Ak47.ammo=PlayerPrefs.GetInt("totalak");
		}
		if(PlayerPrefs.HasKey("totalshells")){
			Shotgun.ammo=PlayerPrefs.GetInt("totalshells");
		}
		if(PlayerPrefs.HasKey("totalalien")){
			Alien.ammo=PlayerPrefs.GetInt("totalalien");
		}
	}

	void Update () 
	{
		if(Input.GetKey("1")) 		// glock selection
		{
			if(Glock.inUse == true)									// if glock in use
			{
				Glock.inUse = true;									// do nothing
			}
			else if(Shotgun.inUse == true && /*Glock.ammo > 0 &&*/ Glock.canUse == true)		// if shotgun in use
			{
				Shotgun.inUse = false;			
				wShotgun.SetActive(false);							// disable it
				Glock.inUse = true;
				wGlock.SetActive(true);
			}
			else if(Ak47.inUse == true && /*Glock.ammo > 0 &&*/ Glock.canUse == true)			// if ak in use
			{
				Ak47.inUse = false;
				wAk47.SetActive(false);								// disable it
				Glock.inUse = true;
				wGlock.SetActive(true);
			}
			else if(Alien.inUse == true && /*Glock.ammo > 0 &&*/ Glock.canUse == true)			// if alien in use
			{
				Alien.inUse = false;
				wAlien.SetActive(false);							// disable it
				Glock.inUse = true;
				wGlock.SetActive(true);
			}

		}

		if(Input.GetKey("2"))		//Shotgun selection
		{
			if(Glock.inUse == true && /*Shotgun.ammo > 0 &&*/ Shotgun.canUse == true)				// if glock in use
			{
				Glock.inUse = false;
				wGlock.SetActive(false);							// disable it
				Shotgun.inUse= true;
				wShotgun.SetActive(true);

			}
			else if(Shotgun.inUse == true)							// if shotgun in use
			{
				Shotgun.inUse = true;								// do nothing
			}
			else if(Ak47.inUse == true && /*Shotgun.ammo > 0 &&*/ Shotgun.canUse == true)			// if ak in use
			{
				Ak47.inUse = false;
				wAk47.SetActive(false);								// disable it
				Shotgun.inUse = true;
				wShotgun.SetActive(true);
			}
			else if(Alien.inUse == true && /*Shotgun.ammo > 0 &&*/ Shotgun.canUse == true)		// if alien in use
			{
				Alien.inUse = false;
				wAlien.SetActive(false);							// disable it
				Shotgun.inUse = true;
				wShotgun.SetActive(true);
			}
		}

		if(Input.GetKey("3"))		// Ak47 Selection
		{
			if(Glock.inUse == true && /*Ak47.ammo > 0 &&*/ Ak47.canUse == true)				// if glock in use
			{
				Glock.inUse = false;
				wGlock.SetActive(false);							// disable it
				Ak47.inUse= true;
				wAk47.SetActive(true);
				
			}
			else if(Shotgun.inUse == true && /*Ak47.ammo > 0 &&*/ Ak47.canUse == true)			// if shotgun in use
			{
				Shotgun.inUse = false;								// disable it
				wShotgun.SetActive(false);
				Ak47.inUse = true;
				wAk47.SetActive(true);
			}
			else if(Ak47.inUse == true)								// if ak in use
			{
				Ak47.inUse = true;									// do nothing
			}
			else if(Alien.inUse == true &&/* Ak47.ammo > 0 &&*/ Ak47.canUse == true)			// if alien in use
			{
				Alien.inUse = false;
				wAlien.SetActive(false);							// disable it
				Ak47.inUse = true;
				wAk47.SetActive(true);
			}
		}

		if(Input.GetKey("4"))		//Section 9 alien weapon Selection
		{
			if(Glock.inUse == true && /*Alien.ammo > 0 && */Alien.canUse == true)				// if glock in use
			{
				Glock.inUse = false;
				wGlock.SetActive(false);							// disable it
				Alien.inUse= true;
				wAlien.SetActive(true);
				
			}
			else if(Shotgun.inUse == true && /*Alien.ammo > 0 && */Alien.canUse == true)		// if shotgun in use
			{
				Shotgun.inUse = false;								// do nothing
				wShotgun.SetActive(false);
				Alien.inUse = true;
				wAlien.SetActive(true);
			}
			else if(Ak47.inUse == true && /*Alien.ammo > 0 && */Alien.canUse == true)			// if ak in use
			{
				Ak47.inUse = false;
				wAk47.SetActive(false);								// disable it
				Alien.inUse = true;
				wAlien.SetActive(true);
			}
			else if(Alien.inUse == true)							// if alien in use
			{
				Alien.inUse = true;									// do nothing
			}
		}


	} // End Update

	public int checkWeapon()
	{
		if(Glock.inUse == true)
			return 1;
		else if (Shotgun.inUse == true)
			return 2;
		else if (Ak47.inUse == true)
			return 3;
		else if (Alien.inUse == true)
			return 4;
		else 
			return 1;
	}
	 // Get and Sets for weapon sub scripts
	public int getGlockAmmo()
	{
		return Glock.ammo;
	}

	public void setGlockAmmo(int x)
	{
		Glock.ammo = x;
	}

	public int getShotgunAmmo()
	{
		return Shotgun.ammo;
	}

	public void setShotgunAmmo(int x)
	{
		Shotgun.ammo = x;
	}

	public int getAkAmmo()
	{
		return Ak47.ammo;
	}

	public void setAkAmmo(int x)
	{
		Ak47.ammo = x;
	}

	public int getAlienAmmo()
	{
		return Alien.ammo;
	}

	public void setAlienAmmo(int x)
	{
		Alien.ammo = x;
	}

	public void addGlockAmmo()
	{
		Glock.ammo += 30;
	}

	public void addShells()
	{
		Shotgun.ammo += 6;
	}

	public void addAkAmmo()
	{
		Ak47.ammo += 60;
		//Debug.Log ("you are adding ak ammo somewhere");
	}

	public void addAlienAmmo()
	{
		Alien.ammo+= 1;
	}

	public void setShotgunUse()
	{
		Shotgun.canUse = true;
	}

	public int getShotgunUse()
	{
		if(Shotgun.canUse == true)
			return 1;
		else 
			return 0;
	}

	public void setAkUse()
	{
		Ak47.canUse = true;
	}

	public void setAlienUse()
	{
		Alien.canUse = true;
	}

}
