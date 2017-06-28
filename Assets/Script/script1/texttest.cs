using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class texttest : MonoBehaviour {
	Text txt_ammo;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void textchange(){
		txt_ammo = GameObject.Find ("Canvas/Text").GetComponent<Text> ();
		txt_ammo.text ="You want to write content";
	}

}
