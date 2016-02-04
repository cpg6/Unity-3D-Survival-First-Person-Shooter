using UnityEngine;
using System.Collections;

public class RESET : MonoBehaviour {

	// Use this for initialization
	public void Start () {
		PlayerPrefs.DeleteAll();
	}
}
