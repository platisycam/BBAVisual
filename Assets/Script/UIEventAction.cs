using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class UIEventAction : MonoBehaviour{
	bool isDelete = false;
	string testPanelName = "3560";//测试用面板名字
	string testPortlName = "port1";
	Vector3 offset;//用于记录鼠标和面板的位置偏移量
	//需要将所有UI按钮注册进此List
	List<string> btnsName = new List<string>{
        "Button01",
        "Button02",
        "Button03",
        "Button04",
        "Button05"
    };

	List<string> winsName = new List<string>{
		"Win01",
		"Win02"
	};
		
	// Use this for initialization
	public void Start () {

		//为所有的UI按钮增加单击事件
		foreach(string btnName in btnsName){
			GameObject obj = GameObject.Find(btnName);
			if(obj){
				Button btn = obj.GetComponent<Button>();
				if(btn != null){
					btn.onClick.AddListener(delegate{
						this.BtnOnClick(obj);
					});
				}
			}
		}
			
		foreach(string winName in winsName){
			GameObject obj = GameObject.Find(winName);
			if(obj){
				//增加事件触发器
				EventTrigger trigger = obj.GetComponent<EventTrigger>();
				if(trigger == null){
					trigger = obj.AddComponent<EventTrigger>();
				}
				trigger.triggers = new List<EventTrigger.Entry>();

				//增加触发事件
				EventTrigger.Entry entry = new EventTrigger.Entry();
				entry.eventID = EventTriggerType.PointerDown;//添加鼠标按下事件
				//entry.callback = new EventTrigger.TriggerEvent();
				entry.callback.AddListener(delegate{
					this.WinOnSingleClick(obj);
				});
				trigger.triggers.Add(entry);

				entry = new EventTrigger.Entry();
				entry.eventID = EventTriggerType.Drag;//添加鼠标拖动事件
				//entry.callback = new EventTrigger.TriggerEvent();
				entry.callback.AddListener(delegate{
					this.WinOnDrag(obj);
				});
				trigger.triggers.Add(entry);
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	void BtnOnClick(GameObject btnObj){
		if(btnObj != null){
			switch(btnObj.name){
			case "Button01":
				if(!this.GetComponent<CreateSprite>()){
					this.gameObject.AddComponent<CreateSprite>();
				}
				isDelete = false;
				Debug.Log("btn1");
				this.GetComponent<CreateSprite>().CreatePanelClone(testPanelName);
				break;
			case "Button02":
				if(!this.GetComponent<CreateSprite>()){
					this.gameObject.AddComponent<CreateSprite>();
				}
				isDelete = false;
				this.GetComponent<CreateSprite>().CreatePortClone(testPortlName);
				break;
			case "Button03":
				if(!this.GetComponent<SavePrefab>()){
					this.gameObject.AddComponent<SavePrefab>();
				}
				isDelete = false;
				//this.GetComponent<SavePrefab>().SaveAsPrefab();
				break;
//			case "Button04":
//				if(!this.GetComponent<LayoutPort>()){
//					this.gameObject.AddComponent<LayoutPort>();
//				}
//				isDelete = false;
//				this.GetComponent<LayoutPort>().CreatePorts();
//				break;
			case "Button05":
				isDelete = !isDelete;
				break;
			default:
				Debug.Log("没有找到执行函数");
				break;
			}
		}
	}		

	public void WinOnSingleClick(GameObject obj){
		offset = obj.transform.position - Input.mousePosition;
	}

	public void WinOnDrag(GameObject obj){
		Vector3 mousPos = Input.mousePosition;
		obj.transform.position = mousPos + offset;
	}

	public bool getIsDelete(){
		return this.isDelete;
	}

	public void setIsDelete(bool b){
		this.isDelete = b;
	}
}
