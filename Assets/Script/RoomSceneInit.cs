using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using LitJson;

//Room场景的初始化，通过数据将场景生成出来
public class RoomSceneInit : MonoBehaviour {
	//场景显示的参数
	float leftWidth;
	float rightWidth;
	float topWidth;
	float bottomWidth;
	float heightGap;
	float widthGap;

	string sceneName;
	float roomCameraSize;

	//生成物体的参数
	float x;//坐标x初始值
	float y;//坐标y初始值
	float maxYInRow;
	float maxWidth;
	//int[] rowsHeight;//各排高
	//int[] rowsWidth;//各排宽
	float maxExtX;//生成的场景最大宽
	float maxExtY;//生成的场景最大高
	float maxX;//视野最大宽
	float maxY;//视野最大高

	float Uheight;//U高
	float Ubottom;//1U到底部的距离
	float X;//机柜的X轴坐标
	GameObject cabinet;//机柜
	GameObject device;//设备
	GameObject card;
	GameObject sceneOjbs;
	GameObject row;
	RowData[] rowDatas;

	string preCabinetName;
	string preDeviceName;
	string preCardName;
	string roomid;
	string roomActivity = "false";
	Vector3[] lineEnds;

	void start () {
	}


	// Update is called once per frame
	void Update () {

	}

	//设置场景参数
	void SetSceneArgs(){
		Camera.main.orthographicSize = roomCameraSize;

		leftWidth = this.GetComponent<SceneSetting>().getLeftWidth();
		rightWidth =this.GetComponent<SceneSetting>().getRightWidth();
		topWidth = this.GetComponent<SceneSetting>().getTopWidth();
		bottomWidth = this.GetComponent<SceneSetting>().getBottomWidth();
		heightGap = this.GetComponent<SceneSetting>().getHeightGap();
		widthGap = this.GetComponent<SceneSetting>().getWidthGap();
		maxYInRow = 0;
		maxWidth = 0;
		x = leftWidth;
		y = bottomWidth;
		maxExtX = leftWidth;//生成的场景最大宽
		maxExtY = 0;//生成的场景最大高
		maxX = 0;//视野最大宽
		maxY = 0;//视野最大高

        GameObject temePrefab = (GameObject)Resources.Load("Prefab/G650-1");
        Vector2 size = temePrefab.AddComponent<BoxCollider2D>().size;
        Uheight = size.y / 44.3f;//计算1U的高度
        Ubottom = Uheight * 1.2f;//1U到底部的距离
    }

	//创建模型场景
	public void CreateScene(string modelname){
		float x = 30f;
		float y = 30f;
		Debug.Log("1");
		this.transform.GetComponent<SceneSetting>().ResetScene();
		sceneOjbs = new GameObject();
		sceneOjbs.name = "senceObjes";
		SetSceneArgs();
		Camera.main.GetComponent<ViewpointCtrl>().setScrollSpeed(0.5f);
		Camera.main.GetComponent<ViewpointCtrl>().setMinCamSize(0.5f);
		Camera.main.transform.position = new Vector3(x/2,y/2, -10f);
		Camera.main.orthographicSize = x/2f;
		this.GetComponent<SceneSetting>().setMaxCamSize(Camera.main.orthographicSize*1.2f);
		Camera.main.GetComponent<ViewpointCtrl>().setMaxCamSize(Camera.main.orthographicSize*1.2f);
		GameObject temePrefab = (GameObject)Resources.Load("Prefab/Room");
		GameObject roomObj = (GameObject)Instantiate(temePrefab, Vector3.zero, Quaternion.identity);
		roomObj.tag = "creating";
		roomObj.transform.localScale = new Vector3(x, y, 0f);
		roomObj.transform.parent = sceneOjbs.transform;
		roomObj.tag = "room";
		temePrefab = (GameObject)Resources.Load("Prefab/" + modelname);
		if(temePrefab != null){
			GameObject modelObj = (GameObject)Instantiate(temePrefab, new Vector3(x/2f, y/2f, -1f), Quaternion.identity);
			roomObj.transform.position = new Vector3(x/2, y/2, 0f); 
		}
		this.GetComponent<SceneSetting>().setMaxExtX(x);
		this.GetComponent<SceneSetting>().setMaxExtY(y);
	}

	//创建场景内容
	public void CreateScene(){

		Debug.Log("createScene");
		//this.transform.GetComponent<SceneSetting>().ResetScene();
		sceneOjbs = new GameObject();
		sceneOjbs.name = "senceObjes";
		//运行数据
		SetSceneArgs();
		Camera.main.GetComponent<ViewpointCtrl>().setScrollSpeed(3f);
		Camera.main.GetComponent<ViewpointCtrl>().setMinCamSize(4f);
		string str = this.transform.GetComponent<SystemSetting>().GetSceneData();
		if(str != null){
			this.transform.GetComponent<SystemSetting>().SetSceneData("");
		}
		//解析预定格式的Json代码
		JsonData roomData = (JsonMapper.ToObject(str))["roomData"];
		//房间数据
		for(int i = 0; i < roomData.Count; i++){
			//创建房间节点
		    //GameObject room = new GameObject();
			//room.name = "obj";
			//room.transform.parent = room.transform;
			//room.name =roomData[i]["roomInstance"].ToString();
			if(((IDictionary)roomData[i]).Contains("roomid") && roomData[i]["roomid"] != null){
				roomid = roomData[i]["roomid"].ToString();
				this.GetComponent<SystemSetting>().SetRoomid(roomid);
				roomActivity = roomData[i]["activity"].ToString();
				if(((IDictionary)roomData[i]).Contains("roomName") && roomData[i]["roomName"] != null){
					this.GetComponent<SystemSetting>().SetRoomName(roomData[i]["roomName"].ToString());
				}
			}
			if(((IDictionary)roomData[i]).Contains("rowData") && roomData[i]["rowData"] != null){
				JsonData rowData = roomData[i]["rowData"];
				//排数据
				rowDatas = new RowData[rowData.Count];
				for(int j = 0; j < rowData.Count; j++){
					//创建排节点
					row = new GameObject();
					RowData item = new RowData();
					//挂参数脚本
					row.tag = "creating";
					row.transform.parent = sceneOjbs.transform;
					row.tag = "row";
					if(((IDictionary)rowData[j]).Contains("rowid") && rowData[j]["rowid"] != null){
						string rowName = rowData[j]["rowid"].ToString();
						row.name = rowName;
						item.id = rowName;
					}
					if(((IDictionary)rowData[j]).Contains("cabinetData") && rowData[j]["cabinetData"] != null){
						JsonData cabinetData = rowData[j]["cabinetData"];
						//机柜数据
						for(int k = 0; k < cabinetData.Count; k++){
							//生成一个机柜实例，并且返回它的size
							Vector2 CabinetSize = CreateCabinet(row, cabinetData[k]);

							MaxHeightInRow(CabinetSize);


							if(((IDictionary)cabinetData[k]).Contains("deviceData") && cabinetData[k]["deviceData"] != null){
								JsonData deviceData = cabinetData[k]["deviceData"];
								//设备数据
								for(int m = 0; m < deviceData.Count; m++){
									//生成设备
									CreateDevice(deviceData[m]);
									if(((IDictionary)deviceData[m]).Contains("cardData") && deviceData[m]["cardData"] != null){
										JsonData cardData = deviceData[m]["cardData"];
										//板卡数据
										for(int n = 0; n < cardData.Count; n++){
											CreateCard(cardData[n]);
											card = null;
										}
									}
									device = null;
								}
							}
							cabinet = null;
						}
					}
					row = null;
					//将x,y坐标存入排参数脚本里	
					item.x = x;
					item.y = y;
					y = y + maxYInRow + heightGap;
					rowDatas[j] = item;
					maxYInRow = 0;
					//每排生成结束后，需要设置最大宽度，修改x,y坐标;
					MaxWidth(x);
					//坐标点 + 前一排最高 + 高间隔
					x = leftWidth;
				}
			}
		}
		maxExtX = maxWidth + rightWidth - widthGap;
		maxExtY = y + topWidth - heightGap;
		Debug.Log("maxExtX: " + maxExtX + "maxExtY: " + maxExtY);
		if(maxExtX/4 > maxExtY/3){
			maxX = maxExtX;
			maxY = maxX*3/4;
			Camera.main.orthographicSize = maxX/2;
		}else{
			maxY = maxExtY;
			maxX = maxY*4/3;
			Camera.main.orthographicSize = maxY/2;
		}
//		Debug.Log("maxX: " + maxX + ". maxY: " + maxY + "Camera.main.orthographicSize: " + Camera.main.orthographicSize);
		//稍稍放大一下最大摄像机倍数
		Camera.main.GetComponent<ViewpointCtrl>().setMaxCamSize(Camera.main.orthographicSize*1.2f);
		//设置摄像机位置到生成的物体中间，并且将视角设置为可以收录所有物体
		Camera.main.transform.position = new Vector3(maxExtX/2, maxExtY/2, -10);
		GameObject temePrefab = (GameObject)Resources.Load("Prefab/Room");
		GameObject roomObj = (GameObject)Instantiate(temePrefab, new Vector3(maxExtX/2, maxExtY/2, 0), Quaternion.identity);
		roomObj.tag = "creating";
		roomObj.transform.localScale = new Vector3(maxX, maxY, 0f);
		roomObj.name = roomid;
		roomObj.transform.parent = sceneOjbs.transform;
		roomObj.tag = "room";
		//显示管理间名称
		GameObject.Find("Canvas").GetComponent<ShowRoomName>().DisplayManagementRoomName(roomObj, this.GetComponent<SystemSetting>().GetRoomName());
//		if("true".Equals(roomActivity)){
		roomObj.AddComponent<BoxCollider2D>();
		roomObj.AddComponent<DeviceMouseEvent>();
//		}
		this.GetComponent<SystemSetting>().SetCamOrgPos(Camera.main.transform.position);
		this.GetComponent<SceneSetting>().setMinX(0);
		this.GetComponent<SceneSetting>().setMinY(0);
		this.GetComponent<SceneSetting>().setMaxExtX(maxExtX);
		this.GetComponent<SceneSetting>().setMaxExtY(maxExtY);
//		this.GetComponent<SystemSetting>().Print();
		this.GetComponent<SystemSetting>().SetRowDatas(rowDatas);
		sceneOjbs = null;

	}

	//生成机柜，prefab为机柜预设名，name为机柜实例名
	//jd的格式为{\"cabinetPrefab\":\"\", \"cabinetInstance\":\"\",\"deviceData\":“”}
	Vector2 CreateCabinet(GameObject row, JsonData jd){
		string prefab = "";
		string instanceName = "";
		string id = "";
		string highLight = "";
		string activity = "";
		string lable = "";
		Vector2 size = Vector3.zero;
		if(((IDictionary)jd).Contains("cabinetPrefab")){
			if(jd["cabinetPrefab"] != null){
				prefab = jd["cabinetPrefab"].ToString();
				GameObject temePrefab = (GameObject)Resources.Load("Prefab/" + prefab);
				if(temePrefab){
					cabinet = (GameObject)Instantiate(temePrefab, Vector3.zero, Quaternion.identity);
					cabinet.tag = "creating";
					cabinet.AddComponent<BoxCollider2D>();
					cabinet.AddComponent<ShowCabinetName>();
					size = cabinet.GetComponent<BoxCollider2D>().size;
				}else{
					temePrefab = (GameObject)Resources.Load("Prefab/G650-1");
					temePrefab.AddComponent<BoxCollider2D>();
					size = temePrefab.GetComponent<BoxCollider2D>().size;
					cabinet = new GameObject();
					cabinet.tag = "creating";
				}
			}else{
				GameObject temePrefab = (GameObject)Resources.Load("Prefab/G650-1");
				temePrefab.AddComponent<BoxCollider2D>();
				size = temePrefab.GetComponent<BoxCollider2D>().size;
				cabinet = new GameObject();
				cabinet.tag = "creating";
			}
			cabinet.transform.parent = row.transform;
			cabinet.tag = "cabinet";

			cabinet.transform.position = new Vector3(x + size.x/2, y + size.y/2, -1f);
			x += (size.x + widthGap);
			X = cabinet.transform.position.x;//存储机柜的X轴坐标,方便生成设备时使用
//			Debug.Log("x: " + x);

		}else{
			cabinet = new GameObject();
			cabinet.transform.parent = row.transform;//机柜设置在排节点下
		}
		if(((IDictionary)jd).Contains("cabinetInstance") && jd["cabinetInstance"] != null){
			instanceName = jd["cabinetInstance"].ToString();
			cabinet.name = instanceName;

		}
		if(((IDictionary)jd).Contains("id") && jd["id"] != null){
			id = jd["id"].ToString();
		}
		if(((IDictionary)jd).Contains("highLight") && jd["highLight"] != null){
			highLight = jd["highLight"].ToString();
		}
		if(((IDictionary)jd).Contains("activity") && jd["activity"] != null){
			activity = jd["activity"].ToString();
		}
		if(((IDictionary)jd).Contains("cabinetLable") && jd["cabinetLable"] != null){
			lable = jd["cabinetLable"].ToString();
			cabinet.AddComponent<Properties>();
			cabinet.GetComponent<Properties>().setLable(lable);
			cabinet.GetComponent<ShowCabinetName>().DisplayCabinetName(lable);
		}
		if("" != activity && "true".Equals(activity)){
			//增加碰撞盒，检测鼠标事件
			cabinet.AddComponent<DeviceMouseEvent>();

			//增加鼠标事件脚本
			//cabinet.AddComponent<SpriteMouseEvent>();
			//Debug.Log("add cabinet mouse event");
				
		}
		return size;
		
	}
	//生成设备
	//jd的格式{\"devicePrefab\":\"\", \"deviceInstance\":\"\", \"startSlot\":\"\", \"endSlot\":\"\", \"cardData\":“”}
	void CreateDevice(JsonData jd){
		string prefab = "";
		string instanceName = "";
		string id = "";
		string highLight = "";
		string activity = "";
		string startSlot = "";
		string endSlot = "";
		JsonData portData;
		if(((IDictionary)jd).Contains("deviceInstance") && jd["deviceInstance"] != null){
			instanceName = jd["deviceInstance"].ToString();
		}
		if(((IDictionary)jd).Contains("devicePrefab") && jd["devicePrefab"] != null){
			prefab = jd["devicePrefab"].ToString();
			GameObject tempPrefab = (GameObject)Resources.Load("Prefab/" + prefab);
			if(tempPrefab == null){
				tempPrefab = (GameObject)Resources.Load("Prefab/" + preDeviceName);
			}
			Debug.Log("tempPrefab: " + prefab);
            if (tempPrefab)
            {
                device = (GameObject)Instantiate(tempPrefab, Vector3.zero, Quaternion.identity);

            }
            else {
                tempPrefab = (GameObject)Resources.Load("Prefab/nothing");
                device = (GameObject)Instantiate(tempPrefab, Vector3.zero, Quaternion.identity);
            }
            device.tag = "creating";
            device.name = instanceName;
            device.tag = "device";
            device.AddComponent<BoxCollider2D>();
            //Debug.Log("device: " + device.name);
        }
		if(((IDictionary)jd).Contains("id") && jd["id"] != null){
			id = jd["id"].ToString();
		}
		if(((IDictionary)jd).Contains("highLight") && jd["highLight"] != null){
			highLight = jd["highLight"].ToString();
		}
		if(((IDictionary)jd).Contains("activity") && jd["activity"] != null){
			activity = jd["activity"].ToString();
		}
		if(((IDictionary)jd).Contains("startSlot") && jd["startSlot"] != null && device != null)
        {
			startSlot = jd["startSlot"].ToString();
			int s;
			int.TryParse (startSlot, out s);
            Debug.Log("s: " + s);

            device.transform.position = new Vector3 (X, y + Ubottom + (s -1) * Uheight + device.GetComponent<BoxCollider2D> ().size.y * 0.5f, -2f);
			device.transform.parent = cabinet.transform;
		}
		if(((IDictionary)jd).Contains("endSlot") && jd["endSlot"] != null){
			endSlot = jd["endSlot"].ToString();
		}


		if("" != activity && "true".Equals(activity) || true){
			//增加碰撞盒，检测鼠标事件
			device.AddComponent<DeviceMouseEvent>();
		}
		if(((IDictionary)jd).Contains("portData") && jd["portData"] != null){
			portData = jd["portData"];
			AddPortMouseEvent(device.transform, portData);
		}
	}
	//生成板卡
	//jd格式{\"cardPrefab\":\"\", \"cardInstance\":\"\", \"startSlot\":\"\", \"endSlot\":\"\"}
	void CreateCard(JsonData jd){
		string prefab = "";
		string instanceName = "";
		string id = "";
		string highLight = "";
		string activity = "";
		string startSlot = "";
		string endSlot = "";
		JsonData portData;
		if(((IDictionary)jd).Contains("cardPrefab") && jd["cardPrefab"] != null){
			prefab = jd["cardPrefab"].ToString();
		}
		if(((IDictionary)jd).Contains("cardInstance") && jd["cardInstance"] != null){
			instanceName = jd["cardInstance"].ToString();
		}
		if(((IDictionary)jd).Contains("Id") && jd["Id"] != null){
			id = jd["Id"].ToString();
		}
		if(((IDictionary)jd).Contains("highLight") && jd["highLight"] != null){
			highLight = jd["highLight"].ToString();
		}
		if(((IDictionary)jd).Contains("activity") && jd["activity"] != null){
			activity = jd["activity"].ToString();
		}
		if(((IDictionary)jd).Contains("startSlot") && jd["startSlot"] != null){
			startSlot = jd["startSlot"].ToString();
		}
		if(((IDictionary)jd).Contains("endSlot") && jd["endSlot"] != null){
			endSlot = jd["endSlot"].ToString();
		}

		GameObject tempPrefab = (GameObject)Resources.Load("Prefab/" + prefab);
		if(tempPrefab == null){
			tempPrefab = (GameObject)Resources.Load("Prefab/" + preCardName);
		}
		//float Cheight = csize.y;//板卡单位高度
//		Debug.Log("device: " + device.transform.name);
//		Debug.Log("Pmark" + startSlot);
		if(device.transform.Find ("Pmark" + startSlot) && tempPrefab){
			float X1 = device.transform.Find ("Pmark" + startSlot).transform.position.x;
			float Y1 = device.transform.Find ("Pmark" + startSlot).transform.position.y;//获取标识坐标

			card = (GameObject)Instantiate(tempPrefab, Vector3.zero, Quaternion.identity);
			card.tag = "creating";
			card.name = instanceName;
			card.tag  = "card";
			card.AddComponent<BoxCollider2D> ();

			Vector2 csize = card.GetComponent<BoxCollider2D> ().size;
			if("" != activity && "true".Equals(activity) || true){
				card.AddComponent<DeviceMouseEvent>();
			}
			if(((IDictionary)jd).Contains("portData") && jd["portData"] != null){
				portData = jd["portData"];
				AddPortMouseEvent(card.transform, portData);
			}
			card.transform.position = new Vector3 (X1, Y1, -3f);
			//card.transform.position = new Vector3 (X1, Y1 + (s - 1) * Cheight, 0);
			card.transform.parent = device.transform;
		}
	}
	//设置每排最大的高度
	void MaxHeightInRow(Vector2 size){
		maxYInRow = (maxYInRow > size.y ? maxYInRow : size.y);
	}

	//设置最大宽度
	void MaxWidth(float width){
		maxWidth = (maxWidth > width ? maxWidth : width);
	}

	void AddPortMouseEvent(Transform transform, JsonData portData){
		for(int i = 0; i < portData.Count; i ++){
			string portno = portData[i]["portNo"].ToString();
			Transform port = transform.Find(portno);
			if(port != null){
				port.gameObject.AddComponent<DeviceMouseEvent>();
				port.gameObject.AddComponent<Properties>();
				port.gameObject.AddComponent<BoxCollider2D>();
				if(((IDictionary)portData[i]).Contains("portType") && portData[i]["portType"] != null){
					string type = portData[i]["portType"].ToString();
					port.gameObject.GetComponent<Properties>().setType(type);
				}
				if(((IDictionary)portData[i]).Contains("portStatus") && portData[i]["portStatus"] != null){
					string status = portData[i]["portStatus"].ToString();
					port.gameObject.GetComponent<Properties>().setStatus(status);
				}
				if(((IDictionary)portData[i]).Contains("portid") && portData[i]["portid"] != null){
					string portid = portData[i]["portid"].ToString();
					port.gameObject.GetComponent<Properties>().setid(portid);
				}			
			}
		}
	}

	//显示管理间名称的接口
	//void DisplayRoomNamePort(){
	//	if (transform.GetComponent<SystemSetting> ().GetRoomName () == null) {
	//		GameObject.Find ("Canvas").GetComponent<ShowRoomName> ().enabled = false;
	//	}
	//	else{
	//        GameObject.Find ("Canvas").GetComponent<ShowRoomName> ().enabled = true;
	//		GameObject.Find ("Canvas").SendMessage ("DisplayManagementRoomName",transform.GetComponent<SystemSetting> ().GetRoomName ());
	//	}
	//}
}


