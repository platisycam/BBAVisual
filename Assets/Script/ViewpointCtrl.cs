using UnityEngine;
using System.Collections;

public class ViewpointCtrl : MonoBehaviour {
	float scrollSpeed = 0.5f;
	float dragSpeel = 0.5f;
	float maxCamSize = 12f;
	float minCamSize = 1f;
	GameObject systemObject;
	Vector3 prePoint = Vector3.zero;
	Vector3 preCamPoint = Vector3.zero;
	//Vector3 curPoint = Vector3.zero;
	// Use this for initialization
	void Start () {
		systemObject = GameObject.Find("SystemObject");

	}
	
	// Update is called once per frame
	void Update () {
		//鼠标滚轮事件
		if(Input.GetAxis("Mouse ScrollWheel") < 0){
//			if( systemObject.GetComponent<SystemSetting>()){
//				maxCamSize = systemObject.GetComponent<SystemSetting>().getMaxCamSize();
				//Debug.Log("scale: " + this.transform.localScale);
//			}
//			Debug.Log("<0, " + Camera.main.orthographicSize + ",maxCamSize: " + maxCamSize);
			if(Camera.main.orthographicSize < maxCamSize){
				Camera.main.orthographicSize += scrollSpeed;
			}
		}
		if(Input.GetAxis("Mouse ScrollWheel") > 0){
//			if( systemObject.GetComponent<SystemSetting>()){
//				maxCamSize = systemObject.GetComponent<SystemSetting>().getMaxCamSize();
				//Debug.Log("scale: " + this.transform.localScale);
//			}
			if(Camera.main.orthographicSize > minCamSize){
				Camera.main.orthographicSize -= scrollSpeed;
			}
		}
		//鼠标按键事件
		if(Input.GetMouseButtonDown(0) && !GameObject.Find("msgBox")){
			//Debug.Log("Input.mousePosition: " + Input.mousePosition);
			prePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			preCamPoint = Camera.main.transform.position;
		}

		//让链路场景内的物体随镜头距离保持不变。
		if(GameObject.Find("moveLinkObjs")){
			GameObject.Find("moveLinkObjs").transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f);
			GameObject.Find("moveLinkObjs").transform.localScale = new Vector3(Camera.main.orthographicSize/3f, Camera.main.orthographicSize/3f, 1f);	

		}


		if(Input.GetMouseButton(0) && !GameObject.Find("msgBox")){
			

			Vector3 curPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 camPosition = Camera.main.transform.position;
//			Debug.Log("room");
			Camera.main.transform.position -= (curPoint - prePoint)*dragSpeel;
			if (systemObject.GetComponent<SystemSetting> () && !GameObject.Find ("ZonePicture")) {
				if (Camera.main.transform.position.x < systemObject.GetComponent<SceneSetting> ().getMinX ()) {
					Camera.main.transform.position = new Vector3 (systemObject.GetComponent<SceneSetting> ().getMinX (), Camera.main.transform.position.y, Camera.main.transform.position.z);
				}
				if (Camera.main.transform.position.x > systemObject.GetComponent<SceneSetting> ().getMaxExtX ()) {
					Camera.main.transform.position = new Vector3 (systemObject.GetComponent<SceneSetting> ().getMaxExtX (), Camera.main.transform.position.y, Camera.main.transform.position.z);
				}
				if (Camera.main.transform.position.y < systemObject.GetComponent<SceneSetting> ().getMinY ()) {
					Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, systemObject.GetComponent<SceneSetting> ().getMinY (), Camera.main.transform.position.z);
				}
				if (Camera.main.transform.position.y > systemObject.GetComponent<SceneSetting> ().getMaxExtY ()) {
					Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, systemObject.GetComponent<SceneSetting> ().getMaxExtY (), Camera.main.transform.position.z);
				}
			}
			else if (GameObject.Find ("ZonePicture")) {
				if(Camera.main.transform.position.x<=-15.0f){
					Camera.main.transform.position = new Vector3 (-15f,Camera.main.transform.position.y,Camera.main.transform.position.z);
				}
				else if(Camera.main.transform.position.x>=15f){
					Camera.main.transform.position = new Vector3 (15f,Camera.main.transform.position.y,Camera.main.transform.position.z);
				}
				else if(Camera.main.transform.position.y<=-6.0f){
					Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x,-6.0f,Camera.main.transform.position.z);
				}
				else if(Camera.main.transform.position.y>=6.0f){
					Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x,6.0f,Camera.main.transform.position.z);
				}
			}

			prePoint = curPoint;
			preCamPoint = camPosition;
		}
	}

	public void setMaxCamSize(float maxCamSize){
		this.maxCamSize = maxCamSize;
	}

	public void setMinCamSize(float minCamSize){
		this.minCamSize = minCamSize;
	}

	public void setScrollSpeed(float scrollSpeed){
		this.scrollSpeed = scrollSpeed;
	}
}
