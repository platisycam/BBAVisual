using UnityEngine;
using System.Collections;

public class UnloadObj : MonoBehaviour {
	string roomid;
	string equInstance;
	string rowid;
	string fEquid;
	string equKind;
	string action;
	GameObject unloadDevice;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string GetUnloadObjInfo(){
		string str = "";
		roomid = this.GetComponent<SystemSetting>().GetRoomid();
		unloadDevice = this.GetComponent<SystemSetting>().GetUnloadDevice();
		equInstance = unloadDevice.name;
		action = "unLoad";
		equKind = unloadDevice.tag;
		if("cabinet".Equals(unloadDevice.tag)){
			rowid = unloadDevice.transform.parent.name;
			str = "unloadDeviceInfo?" + "{BEquid:" + "" + ";EquInstance:" + equInstance + ";Computerroomid:" + roomid 
				+ ";EquLabel:"+ "" + ";rowid:" + rowid + ";EquKind:" + equKind + ";Action:unLoad" + "}";
		}else if("device".Equals(unloadDevice.tag)){
			fEquid = unloadDevice.transform.parent.name;
			str = "unloadDeviceInfo?" + "{BEquid:" + "" + ";EquInstance:" + equInstance + ";Fequid:" + fEquid 
				+ ";Slot:" + "" + ";EquLabel:"+ "" + ";EquKind:" + equKind +";Action:load}";
		}else if("card".Equals(unloadDevice.tag)){
			fEquid = unloadDevice.transform.parent.name;
			str = "unloadDeviceInfo?" + "{BEquid:" + "" + ";EquInstance:" + equInstance + ";Fequid:" + fEquid 
				+ ";Slot:" + "" + ";EquLabel:"+ "" + ";EquKind:" + equKind +";Action:load}";
		}
		return str;
	}

	public void UnloadDevice(){
		Debug.Log("unloadDevice.name: " + unloadDevice.name);
		if(GameObject.Find("msgBox")){
			Destroy(GameObject.Find("msgBox"));
		}
		if(unloadDevice){
			Debug.Log("equInstance.name: " + equInstance);
			Destroy(unloadDevice);
		}
		this.GetComponent<SystemSetting>().SetUnloadDevice(null);
		if(GameObject.Find(unloadDevice.name).GetComponent<ShowCabinetName>()){
			GameObject.Find(unloadDevice.name).GetComponent<ShowCabinetName>().DestoryCabinetName();
		}

	}
}
