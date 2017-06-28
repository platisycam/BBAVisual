using UnityEngine;
using System.Collections;

/* 用于Unity与Web的通信
 * @author wb 2016/09/14
 */
public class ComunicationAction : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	/*
	 * 接收从web的消息
	 */
	void ReceiveMsgFromWeb(string str){
		Debug.Log("ReceiveMsgFromWeb:" + str);
		//将收到的字符串解析，‘?’前的为标识符，后的为参数


		string[] strs = str.Split('?');
		//Debug.Log("strs[0]: " + strs[0] + ", strs[1]: " + strs[1] + "strs[2]" + strs[2]);
		switch(strs[0]){
        case "authority":
            this.GetComponent<SystemSetting>().SetAuthority(strs[1]);
            break;
        case "scenename":
			this.GetComponent<SystemSetting>().SetSceneData(strs[2]);
			this.GetComponent<SystemSetting>().InitScene(strs[1]);
		    break;
		case "gameObjectInfo":
			if(GameObject.Find("msgBox")){
				Debug.Log("communication:" + strs[1]);
				GameObject.Find("msgBox").GetComponent<ControlAdd>().unityModelBaseData(strs[1]);
			}
			break;
		case "portSceneData":
			this.GetComponent<SystemSetting>().SetPortSceneData(strs[1]);
			break;
		case "linkShowSceneData":
			//链路详情场景展示
			this.GetComponent<SystemSetting>().SetPortSceneData(strs[1]);
			this.GetComponent<SystemSetting>().InitScene("LinkScene");
			break;
		case "loadDeviceInfo":
			this.GetComponent<SystemSetting>().GetLoadDeviceInfo(strs[1]);
			break;
		case "loadDeviceReslut":
			if("success".Equals(strs[1])){
                this.GetComponent<SystemSetting>().SetEnLoaded(true);
                    //				this.GetComponent<SystemSetting>().resetScene();
                    //				this.GetComponent<RoomSceneInit>().CreateScene();
            }
            else if(strs[1].Equals("error")){
			this.GetComponent<SystemSetting>().SetEnLoaded(false);
			}
			break;
		case "unloadDeviceReslut":
			if("success".Equals(strs[1])){
				this.GetComponent<UnloadObj>().UnloadDevice();
			}else if(strs[1].Equals("error")){

			}
			break;
		case "getLinkedObjectInfo":
			GameObject.Find("msgBox").GetComponent<ControlAdd>().unityModelBaseData(strs[1]);
			break;
		case "drawLineData":
			this.GetComponent<SystemSetting>().SetDrawLineData(strs[1]);
			break;
		case "uStatisticsData":
			this.GetComponent<SystemSetting>().SetUStatisticsData(strs[1]);
			break;
		case "modelScene":
            this.GetComponent<SystemSetting>().SetEnUnload(false);
            this.GetComponent<SystemSetting>().ZoneToRoomChangeSence();
			this.GetComponent<SceneSetting>().ResetScene();
			this.GetComponent<RoomSceneInit>().CreateScene(strs[1]);
			break;
		default:
			break;
		}
	}

	/*
	 * 向web发送消息,/js/message.js接收
	 */
	public void SendMsgToWeb(string str){
		Debug.Log("sendMsgToWeb: " + str);
		Application.ExternalCall("ReceiveMsgFromUnity",str);
	}
}
