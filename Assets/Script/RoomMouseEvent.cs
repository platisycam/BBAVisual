using UnityEngine;
using System.Collections;

public class RoomMouseEvent : MonoBehaviour {
	float scrollSpeed = 3f;
	float dragSpeel = 0.5f;
	float maxCamSzie = 0f;
	Vector3 prePoint = Vector3.zero;
	Vector3 preCamPoint = Vector3.zero;
	//Vector3 curPoint = Vector3.zero;
	// Use this for initialization
	void Start () {
		maxCamSzie = GameObject.Find("SystemObject").GetComponent<SceneSetting>().getMaxCamSize();
		Debug.Log("scale: " + this.transform.localScale);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("Mouse ScrollWheel") < 0){
			Debug.Log("<0");
			if(Camera.main.orthographicSize < maxCamSzie){
				Camera.main.orthographicSize += scrollSpeed;
			}
		}
		if(Input.GetAxis("Mouse ScrollWheel") > 0){
			Debug.Log(">0");
			if(Camera.main.orthographicSize > 10f){
				Camera.main.orthographicSize -= scrollSpeed;
			}

		}
	}

	void OnMouseDrag(){
		Debug.Log("Camera.main.transform.position:" + Camera.main.transform.position);
		Vector3 curPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 camPosition = Camera.main.transform.position;
		GameObject SystemObject = GameObject.Find("SystemObject");

		Camera.main.transform.position -= (curPoint - prePoint)*dragSpeel;
		if(Camera.main.transform.position.x < SystemObject.GetComponent<SceneSetting>().getMinX()){
			Camera.main.transform.position = new Vector3(SystemObject.GetComponent<SceneSetting>().getMinX(), Camera.main.transform.position.y, Camera.main.transform.position.z);
		}
		if(Camera.main.transform.position.x > SystemObject.GetComponent<SceneSetting>().getMaxExtX()){
			Camera.main.transform.position = new Vector3(SystemObject.GetComponent<SceneSetting>().getMaxExtX(), Camera.main.transform.position.y, Camera.main.transform.position.z);
		}
		if(Camera.main.transform.position.y < SystemObject.GetComponent<SceneSetting>().getMinY()){
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, SystemObject.GetComponent<SceneSetting>().getMinY(), Camera.main.transform.position.z);
		}
		if(Camera.main.transform.position.y > SystemObject.GetComponent<SceneSetting>().getMaxExtY()){
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, SystemObject.GetComponent<SceneSetting>().getMaxExtY(), Camera.main.transform.position.z);
		}
		prePoint = curPoint;
		preCamPoint = camPosition;
	}

	void OnMouseDown(){
		Debug.Log("Input.mousePosition: " + Input.mousePosition);
		prePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		preCamPoint = Camera.main.transform.position;
	}

	public void SetPrePoint(Vector3 prePoint){
		this.prePoint = prePoint;
	}

	public Vector3 GetPrePoint(){
		return this.prePoint;
	}
}
