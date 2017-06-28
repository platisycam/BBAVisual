using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShowDeviceInformation : MonoBehaviour {

//	string ss1="[{\"value\":\"交换机实例1\",\"key\":\"设备名称\"},{\"value\":\"3650-48PoE+-4x10G\",\"key\":\"设备型号\"},{\"value\":\"(32,32)\",\"key\":\"所在U位\"},{\"value\":\"思科\",\"key\":\"品牌\"},{\"value\":\"普通交换机\",\"key\":\"kind\"},{\"value\":\"交换机\",\"key\":\"type\"},{\"value\":\"24\",\"key\":\"铜口数\"},{\"value\":\"0\",\"key\":\"光口数\"},{\"value\":\"FD320.1-1\",\"key\":\"所在机柜名称\"},{\"value\":\"FD320.1-1\",\"key\":\"所在操作间名称\"}]";
//	List<GameObject> ga=new List<GameObject>();
	GameObject Dec;
	GameObject target;
    GameObject namePanel;
	GameObject deviceInformation;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(deviceInformation && target != null){
			FollowUpDevice();
		}
	}

	//创建上架设备的信息板
	public void CreateDeviceInformation(GameObject target){
		if (!deviceInformation) {
			Debug.Log("create obj");
			Dec = Resources.Load ("Popprefab/UpDeviceInformation", typeof(GameObject)) as GameObject;
			deviceInformation = Instantiate (Dec, transform.position, Quaternion.identity) as GameObject;
			deviceInformation.SetActive(false);
			deviceInformation.transform.name = "DeviceInformation";
			deviceInformation.transform.parent = gameObject.transform;
			deviceInformation.transform.localScale = Vector3.one;
			this.target = target;
		}
	}

	//显示上架设备的信息板
	public void DisplayDeviceInformation(string str){
		if (deviceInformation) {
			deviceInformation.transform.Find ("namePanel").GetComponent<Text> ().text = str;
		}
	}

	//销毁信息板
	public void DestoryDeviceInformation(){
		if (deviceInformation != null) {
			Destroy (GameObject.Find ("DeviceInformation"));
			deviceInformation = null;
			target = null;
		}
	}

	// 跟随上架设备
	void FollowUpDevice(){
		if (target != null) {
			deviceInformation.transform.position = Camera.main.WorldToScreenPoint (target.transform.position + new Vector3(-deviceInformation.GetComponent<BoxCollider2D>().size.x/2-target.GetComponent<BoxCollider2D>().size.x/2-2f,
				deviceInformation.GetComponent<BoxCollider2D>().size.y/2+target.GetComponent<BoxCollider2D>().size.y/2+2f,0f));
		}
	}

	public void EnDisplay(){
		if(deviceInformation != null){
			deviceInformation.SetActive(true);
		}
	}

	public void UnDisplay(){
		if(deviceInformation != null){
			deviceInformation.SetActive(false);
		}
	}
}
