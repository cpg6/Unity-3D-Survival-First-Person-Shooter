#pragma strict
var curweapon =0;
var maxWeapons =2;

var w1 : GameObject;
var w2 : GameObject;

function Update () {
	if(Input.GetKeyDown(KeyCode.E))
	{
		SwapWeapon();
	}
}

function SwapWeapon(){
	if(w1.activeInHierarchy == true)
	{
		w1.SetActive(false);
		w2.SetActive(true);
	}else{
		w1.SetActive(true);
		w2.SetActive(false);
	}
}