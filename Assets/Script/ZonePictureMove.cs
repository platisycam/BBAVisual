using UnityEngine;
using System.Collections;

public class ZonePictureMove : MonoBehaviour {
	float scrollSpeed = 0.5f;
	float dragSpeel = 0.5f;
//	float maxCamSize = 0f;
//	float minCamSize = 4f;
	Vector3 prePoint = Vector3.zero;
	Vector3 preCamPoint = Vector3.zero;
	//Vector3 curPoint = Vector3.zero;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		//鼠标滚轮事件
		if(Input.GetAxis("Mouse ScrollWheel") < 0){
//			if( GameObject.Find("SystemObject").GetComponent<SystemSetting>()){
//				maxCamSize = GameObject.Find("SystemObject").GetComponent<SystemSetting>().getMaxCamSize();
//				//Debug.Log("scale: " + this.transform.localScale);
//			}
			//			Debug.Log("<0, " + Camera.main.orthographicSize + ",maxCamSize: " + maxCamSize);
			if(Camera.main.orthographicSize < 14.0f){
				Camera.main.orthographicSize += scrollSpeed;
			}
		}
		if(Input.GetAxis("Mouse ScrollWheel") > 0){
//			if( GameObject.Find("SystemObject").GetComponent<SystemSetting>()){
//				maxCamSize = GameObject.Find("SystemObject").GetComponent<SystemSetting>().getMaxCamSize();
//				//Debug.Log("scale: " + this.transform.localScale);
//			}
			if(Camera.main.orthographicSize > 2.0f){
				Camera.main.orthographicSize -= scrollSpeed;
			}
		}
		//鼠标按键事件
		if(Input.GetMouseButtonDown(0)){
			//Debug.Log("Input.mousePosition: " + Input.mousePosition);
			prePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			preCamPoint = Camera.main.transform.position;
		}

//		//让链路场景内的物体随镜头距离保持不变。
//		if(GameObject.Find("linkObjs")){		
//			GameObject.Find("linkObjs").transform.localScale = new Vector3(Camera.main.orthographicSize/3f, Camera.main.orthographicSize/3f, 1f);	
//			GameObject.Find("linkObjs").transform.position = new Vector3(Camera.main.transform.position.x  - GameObject.Find("SystemObject").GetComponent<SystemSetting>().getLinkSceneOffset()/ GameObject.Find("linkObjs").transform.localScale.x, Camera.main.transform.position.y, 0f);
//			Debug.Log("camera.postion: " + Camera.main.transform.position
//				+ ", (\"linkObjs\").position: " + GameObject.Find("linkObjs").transform.position
//				+ ", (\"linkObjs\").scale: " + GameObject.Find("linkObjs").transform.localScale
//			);
//		}


		if(Input.GetMouseButton(0) ){
			//Debug.Log("mousedown");
			//Debug.Log("Camera.main.transform.position:" + Camera.main.transform.position);
			Vector3 curPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 camPosition = Camera.main.transform.position;
//			GameObject SystemObject = GameObject.Find("SystemObject");
			Camera.main.transform.position -= (curPoint - prePoint)*dragSpeel;
			if(Camera.main.transform.position.x<=-11.0f){
				Camera.main.transform.position = new Vector3 (-11f,Camera.main.transform.position.y,Camera.main.transform.position.z);
			}
			else if(Camera.main.transform.position.x>=11f){
				Camera.main.transform.position = new Vector3 (11f,Camera.main.transform.position.y,Camera.main.transform.position.z);
			}
			else if(Camera.main.transform.position.y<=-6.0f){
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x,-6.0f,Camera.main.transform.position.z);
			}
			else if(Camera.main.transform.position.y>=6.0f){
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x,6.0f,Camera.main.transform.position.z);
			}


//			if(SystemObject.GetComponent<SystemSetting>()){
//				if(Camera.main.transform.position.x < SystemObject.GetComponent<SystemSetting>().getMinX()){
//					Camera.main.transform.position = new Vector3(SystemObject.GetComponent<SystemSetting>().getMinX(), Camera.main.transform.position.y, Camera.main.transform.position.z);
//				}
//				if(Camera.main.transform.position.x > SystemObject.GetComponent<SystemSetting>().getMaxExtX()){
//					Camera.main.transform.position = new Vector3(SystemObject.GetComponent<SystemSetting>().getMaxExtX(), Camera.main.transform.position.y, Camera.main.transform.position.z);
//				}
//				if(Camera.main.transform.position.y < SystemObject.GetComponent<SystemSetting>().getMinY()){
//					Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, SystemObject.GetComponent<SystemSetting>().getMinY(), Camera.main.transform.position.z);
//				}
//				if(Camera.main.transform.position.y > SystemObject.GetComponent<SystemSetting>().getMaxExtY()){
//					Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, SystemObject.GetComponent<SystemSetting>().getMaxExtY(), Camera.main.transform.position.z);
//				}
//			}
			prePoint = curPoint;
			preCamPoint = camPosition;
		}
	}

}
