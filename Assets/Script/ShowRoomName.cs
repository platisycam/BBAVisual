using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowRoomName : MonoBehaviour {

	private GameObject rname;
	private GameObject _rname;
	private GameObject target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(target){
			_rname.transform.position = Camera.main.WorldToScreenPoint (target.transform.position + new Vector3 (0,target.transform.localScale.y/2-5.0f,0));
		}
	}



	public void DisplayManagementRoomName(GameObject room, string roomname){
		target = room;
		rname = Resources.Load ("Popprefab/ManagementRoomName",typeof(GameObject)) as GameObject;
		_rname = Instantiate (rname, transform.position, Quaternion.identity) as GameObject;
		_rname.transform.name="ManagementRoomName";
		_rname.transform.parent = gameObject.transform;
		_rname.transform.localScale = Vector3.one;
		_rname.GetComponent<Text> ().text = roomname;

	}

	public void DestoryManagementRoomName(){
		if (GameObject.Find ("ManagementRoomName")) {
			Destroy(GameObject.Find("ManagementRoomName"));
		}
	}
}
