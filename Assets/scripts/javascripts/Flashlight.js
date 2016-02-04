#pragma strict

var flashlightLightSource : Light;
var lightOn : boolean =true;
var lightDrain : float = 0.1;
var batteryLife : float = 0.0;
var maxBatteryLife : float = 2.0;

var soundTurnOn : AudioClip;
var soundTurnOff : AudioClip;

function Start(){
	batteryLife = maxBatteryLife;
	flashlightLightSource = GetComponent(Light);
}

function Update(){
	if(lightOn == true)
	{
		batteryLife -= Time.deltaTime * lightDrain;
	}
	flashlightLightSource.GetComponent.<Light>().intensity=batteryLife;
	
	if(batteryLife <= 0)
	{
		batteryLife =0;
		lightOn=false;
	}
	
	if(Input.GetKeyUp(KeyCode.F))
	{
		toggleFlashlight();
		toggleFlashlightSFX();
		
		if(lightOn==true)
		{
			lightOn=false;
		}else if(!lightOn && batteryLife >=0)
		{
			lightOn=true;
		}
	}
}

function toggleFlashlight(){
	if(lightOn)
	{
		flashlightLightSource.enabled=false;
	}else{
		flashlightLightSource.enabled=true;
	}
}

function toggleFlashlightSFX(){
	if(lightOn)
	{
		GetComponent.<AudioSource>().clip =soundTurnOn;
	}else{
		GetComponent.<AudioSource>().clip= soundTurnOff;
	}
	GetComponent.<AudioSource>().Play();
}

//@script RequireComponent(AudioSource);