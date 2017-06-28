using UnityEngine;
using System.Collections;
using LitJson;

//Room场景的初始化，通过数据将场景生成出来
public class DeviceSceneInit : MonoBehaviour {
	//场景显示的参数
	float leftWidth = 50;
	float rightWidth = 50;
	float topWidth = 50;
	float bottomWidth = 50;
	float heightGap = 20;
	float widthGap = 5;

	string sceneName;
	float roomCameraSize = 50;

	//生成物体的参数
	float x;//坐标x初始值
	float y;//坐标y初始值
	float maxYInRow = 0;
	float maxWidth = 0;
	//int[] rowsHeight;//各排高
	//int[] rowsWidth;//各排宽
	float maxExtX = 0;//生成的场景最大宽
	float maxExtY = 0;//生成的场景最大高
	float maxX = 0;//视野最大宽
	float maxY = 0;//视野最大高

	float Uheight;//U高
	float Ubottom;//1U到底部的距离
	float X;//机柜的X轴坐标
	GameObject cabinet;//机柜
	GameObject device;//设备
	GameObject card;
	GameObject sceneOjbs;

	string preCabinetName;
	string preDeviceName;
	string preCardName;

	void Awake () {

	}


	// Update is called once per frame
	void Update () {

	}

	//设置场景参数
	void SetSceneArgs(){
		Camera.main.orthographicSize = roomCameraSize;
		x = leftWidth;
		y = bottomWidth;

		maxYInRow = 0;
		maxWidth = 0;

		maxExtX = 0;//生成的场景最大宽
		maxExtY = 0;//生成的场景最大高
		maxX = 0;//视野最大宽
		maxY = 0;//视野最大高
	}

	//创建场景内容
	public void CreateScene(){

		//roomObj.name = "Room";
		//设置生成物体的位置
		//加载测试数据
		//string str = SystemObject.GetComponent<TestScript>().getStr();

		//运行数据
		this.transform.GetComponent<SceneSetting>().ResetScene();
		sceneOjbs = new GameObject();
		sceneOjbs.name = "secneObjes";
		string str = this.GetComponent<SystemSetting>().GetSceneData();
		//解析预定格式的Json代码
		JsonData roomData = (JsonMapper.ToObject(str))["roomData"];
		//房间数据
		for(int i = 0; i < roomData.Count; i++){
			//创建房间节点
			//GameObject room = new GameObject();
			//room.name = "obj";
			//room.transform.parent = room.transform;
			//room.name =roomData[i]["roomInstance"].ToString();
			if(((IDictionary)roomData[i]).Contains("rowData") && roomData[i]["rowData"] != null){
				JsonData rowData = roomData[i]["rowData"];
				//排数据
				for(int j = 0; j < rowData.Count; j++){
					//创建排节点
					//GameObject row = new GameObject();
					//row.transform.parent = room.transform;
					//row.name = rowData[j]["rowInstance"].ToString();
					if(((IDictionary)rowData[j]).Contains("cabinetData") && rowData[j]["cabinetData"] != null){
						JsonData cabinetData = rowData[j]["cabinetData"];
						//机柜数据
						for(int k = 0; k < cabinetData.Count; k++){
							//生成一个机柜实例，并且返回它的size
							Vector2 CabinetSize = CreateCabinet(cabinetData[k]);

							MaxHeightInRow(CabinetSize);
							//cabinet.transform.parent = row.transform;//机柜设置在排节点下

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

										}
									}
								}
							}
						}
					}

					y = y + maxYInRow + heightGap;
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
		if(maxExtX/4 > maxExtY/3){
			maxX = maxExtX;
			maxY = maxX*3/4;
			Camera.main.orthographicSize = maxX/2;
		}else{
			maxY = maxExtY;
			maxX = maxY*4/3;
			Camera.main.orthographicSize = maxY/2;
		}
		Debug.Log("maxX: " + maxX + ". maxY: " + maxY);
		//稍稍放大一下最大摄像机倍数
		Camera.main.orthographicSize = Camera.main.orthographicSize*1.2f;
		//设置摄像机位置到生成的物体中间，并且将视角设置为可以收录所有物体
		Camera.main.transform.position = new Vector3(maxExtX/2, maxExtY/2, -10);
		GameObject temePrefab = (GameObject)Resources.Load("Prefab/Room");
		GameObject roomObj = (GameObject)Instantiate(temePrefab, new Vector3(maxExtX/2, maxExtY/2, 0), Quaternion.identity);
		roomObj.transform.localScale = new Vector3(maxX, maxY, 0f);
		roomObj.AddComponent<BoxCollider2D>();
		//设备场景不需要房间响应鼠标事件
		//roomObj.AddComponent<DeviceMouseEvent>();
		this.GetComponent<SystemSetting>().SetCamOrgPos(Camera.main.transform.position);
	}

	//生成机柜，prefab为机柜预设名，name为机柜实例名
	//jd的格式为{\"cabinetPrefab\":\"\", \"cabinetInstance\":\"\",\"deviceData\":“”}
	Vector2 CreateCabinet(JsonData jd){
		string prefab = "";
		string instanceName = "";
		string id = "";
		string highLight = "";
		string activity = "";
		if(((IDictionary)jd).Contains("cabinetPrefab") && jd["cabinetPrefab"] != null){
			prefab = jd["cabinetPrefab"].ToString();
		}
		if(((IDictionary)jd).Contains("cabinetInstance") && jd["cabinetInstance"] != null){
			instanceName = jd["cabinetInstance"].ToString();
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
		GameObject temePrefab = (GameObject)Resources.Load("Prefab/" + prefab);
		if(temePrefab == null){
			temePrefab = (GameObject)Resources.Load("Prefab/" + preCabinetName);
		}

		cabinet = (GameObject)Instantiate(temePrefab, Vector3.zero, Quaternion.identity);
		cabinet.name = instanceName;
		cabinet.tag = "cabinet";
		if("" != activity && "true".Equals(activity)){
			//增加碰撞盒，检测鼠标事件
			cabinet.AddComponent<DeviceMouseEvent>();
			//增加鼠标事件脚本
			//cabinet.AddComponent<SpriteMouseEvent>();
			Debug.Log("add cabinet mouse event");

		}
		cabinet.AddComponent<BoxCollider2D>();
		Vector2 size = cabinet.GetComponent<BoxCollider2D>().size;
		Uheight = size.y / 44.3f;//计算1U的高度
		Ubottom = Uheight * 1.2f;//1U到底部的距离
		cabinet.transform.position = new Vector3(x + size.x/2, y + size.y/2, -1f);
		X = cabinet.transform.position.x;//存储机柜的X轴坐标,方便生成设备时使用
		x += (size.x + widthGap);
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
		if(((IDictionary)jd).Contains("devicePrefab") && jd["devicePrefab"] != null){
			prefab = jd["devicePrefab"].ToString();
		}
		if(((IDictionary)jd).Contains("deviceInstance") && jd["deviceInstance"] != null){
			instanceName = jd["deviceInstance"].ToString();
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
		if(((IDictionary)jd).Contains("startSlot") && jd["startSlot"] != null){
			startSlot = jd["startSlot"].ToString();
		}
		if(((IDictionary)jd).Contains("endSlot") && jd["endSlot"] != null){
			endSlot = jd["endSlot"].ToString();
		}
		GameObject tempPrefab = (GameObject)Resources.Load("Prefab/" + prefab);
		if(tempPrefab == null){
			tempPrefab = (GameObject)Resources.Load("Prefab/" + preDeviceName);
		}
		device = (GameObject)Instantiate(tempPrefab, Vector3.zero, Quaternion.identity);
		device.name = instanceName;
		device.tag = "device";
		if("" != activity && "true".Equals(activity)){
			//增加碰撞盒，检测鼠标事件
			device.AddComponent<DeviceMouseEvent>();
			AddPortMouseEvent(device.transform);
		}
		device.AddComponent<BoxCollider2D>();
		Vector2 dsize = device.GetComponent<BoxCollider2D> ().size;
		int s;
		int.TryParse (startSlot, out s);
		device.transform.position = new Vector3 (X, y + Ubottom + (s - 1) * Uheight + dsize.y * 0.5f, -2f);
		device.transform.parent = cabinet.transform;
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
		card = (GameObject)Instantiate(tempPrefab, Vector3.zero, Quaternion.identity);
		card.name = instanceName;
		card.tag = "card";
		card.AddComponent<BoxCollider2D> ();

		Vector2 csize = card.GetComponent<BoxCollider2D> ().size;
		if("" != activity && "true".Equals(activity)){
			card.AddComponent<DeviceMouseEvent>();
			AddPortMouseEvent(card.transform);
		}
		//float Cheight = csize.y;//板卡单位高度
		int s;
		int.TryParse (startSlot,out s);
		Debug.Log("device: " + device.transform.name);
		Debug.Log("Pmark" + startSlot);
		float X1 = device.transform.Find ("Pmark" + startSlot).transform.position.x;
		float Y1 = device.transform.Find ("Pmark" + startSlot).transform.position.y;//获取标识坐标
		card.transform.position = new Vector3 (X1, Y1, -3f);
		//card.transform.position = new Vector3 (X1, Y1 + (s - 1) * Cheight, 0);
		card.transform.parent = device.transform;

	}
	//设置每排最大的高度
	void MaxHeightInRow(Vector2 size){
		maxYInRow = (maxYInRow > size.y ? maxYInRow : size.y);
	}

	//设置最大宽度
	void MaxWidth(float width){
		maxWidth = (maxWidth > width ? maxWidth : width);
	}

	void AddPortMouseEvent(Transform transform){
		foreach(Transform child in transform){
			if(child.tag.Equals("port")){
				child.gameObject.AddComponent<DeviceMouseEvent>();
				child.gameObject.AddComponent<BoxCollider2D>();
			}
		}
	}
}



