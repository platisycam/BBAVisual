using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShowCabinetName : MonoBehaviour {
	private GameObject cname;
	private GameObject display;
	private GameObject cabinet;
	public static List<string> dname=new List<string>();

//	float xOffset;  
//	float yOffset;  
//	RectTransform recTransform; 

//	private static bool start=false;

	// Use this for initialization
	void Start () {
//		cabinet = GameObject.Find ("FD320.1-1");
	}
		
	// Update is called once per frame
	void Update () {
		//		Vector2 player2DPosition = Camera.main.WorldToScreenPoint(transform.position);  
		//		recTransform.position = player2DPosition + new Vector2(xOffset, yOffset);  
		if(display){
			display.transform.position = Camera.main.WorldToScreenPoint(this.transform.position+new Vector3 (0,this.GetComponent<BoxCollider2D>().size.y/2+4.0f,0));

		}

	}
		


	public void DisplayCabinetName(string lable){
//		Debug.Log ("1111111111");
		cname = Resources.Load ("Popprefab/CabinetName", typeof(GameObject)) as GameObject;
		display = Instantiate (cname, transform.position, Quaternion.identity) as GameObject;
		display.transform.name = lable + "Display";
		display.transform.parent = GameObject.Find("Canvas").transform;
		display.transform.localScale = Vector3.one;

//		gameObject.transform.position = new Vector3 (0,0,0);
//		cabinet = GameObject.Find (name);
//		_cname.transform.position=cabinet.transform.position+new Vector3 (0,cabinet.transform.GetComponent<BoxCollider2D>().size.y/2,0);
		display.GetComponent<Text> ().text = lable;
//		Debug.Log ("222222222222");
	}


	public void DestoryCabinetName(){
		if(display){
			Destroy(display);
		}

	}
}
