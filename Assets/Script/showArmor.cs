using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class showArmor : MonoBehaviour 
{
	public FPSC player;
	public Text text;

	// Use this for initialization
	void Start () 
	{
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		text.text = "" + player.armor;
	}
}
