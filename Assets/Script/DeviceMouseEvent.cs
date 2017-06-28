using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DeviceMouseEvent : MonoBehaviour {
	Vector3 prePoint = Vector3.zero;
	Vector3 preCamPoint = Vector3.zero;

	GameObject systemObject;
	GameObject canvas;
	// Use this for initialization
	void Start () {
		systemObject = GameObject.Find("SystemObject");
		canvas = GameObject.Find("Canvas");
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown(){
		if(!GameObject.Find("msgBox")){
			systemObject.GetComponent<SystemSetting>().MousedownTime();
		}
	}

	void OnMouseUp(){
		if(systemObject.GetComponent<SystemSetting>().IsMouseSingleClick()){
			MouseSingleDown();
		}
	}

	//单击
	void MouseSingleDown(){

        systemObject.GetComponent<SystemSetting>().MouseSingleDown(this.gameObject);


    }

}
