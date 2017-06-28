using UnityEngine;
using System.Collections;

public class AddUI : MonoBehaviour {
	private GameObject panel;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddMsgBox(string msgType) {
        if (!GameObject.Find("msgBox")) {
            switch (msgType) {
                case "DeviceMsg":
                    panel = Resources.Load("Popprefab/Device2", typeof(GameObject)) as GameObject;
                    break;
                case "CabinetMsg":
                    panel = Resources.Load("Popprefab/Device3", typeof(GameObject)) as GameObject;
                    break;
                case "PortMsg":
                    panel = Resources.Load("Popprefab/Device", typeof(GameObject)) as GameObject;
                    break;
                case "RoomMsg":
                    panel = Resources.Load("Popprefab/UWDevice", typeof(GameObject)) as GameObject;
                    break;
                case "LinkMsg":
                    panel = Resources.Load("Popprefab/Device4", typeof(GameObject)) as GameObject;
                    break;
            }
            GameObject msgBox = Instantiate(panel, transform.position, Quaternion.identity) as GameObject;
            msgBox.name = "msgBox";
            msgBox.transform.parent = gameObject.transform;
            msgBox.transform.localScale = Vector3.one;
            //如果取不到下架权限，则隐藏下架按钮。
            if (!GameObject.Find("SystemObject").GetComponent<SystemSetting>().GetEnUnload()) { 
                if (msgBox.transform.FindChild("DownDevice"))
                {
                    msgBox.transform.FindChild("DownDevice").gameObject.SetActive(false);
                }
            }
        }
    }
	


	//机柜名称和管理间名称的显隐
	public void ShowHiddenName(bool name){
		if (false.Equals (name)) {
//			transform.FindChild("CabinetName").gameObject.SetActive (false);
			for(int i=0;i<ShowCabinetName.dname.Count;i++){
				transform.Find (ShowCabinetName.dname [i]).gameObject.SetActive (false);
			}
			transform.Find("ManagementRoomName").gameObject.SetActive (false);
		} else if (true.Equals (name)) {
			for(int i=0;i<ShowCabinetName.dname.Count;i++){
				transform.Find (ShowCabinetName.dname [i]).gameObject.SetActive (true);
			}
			transform.Find("ManagementRoomName").gameObject.SetActive (true);
		}
	}


}
