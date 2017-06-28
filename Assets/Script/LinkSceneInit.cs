using UnityEngine;
using System.Collections;
using LitJson;
/*
	链路展示；
 */
public class LinkSceneInit : MonoBehaviour {
	//生成物体的参数
	float x;//坐标x初始值
	float y;//坐标y初始值
	float maxYInRow = 0;
	float maxWidth = 0;
	float maxExtX = 0;//生成的场景最大宽
	float maxExtY = 0;//生成的场景最大高
	float maxX = 0;//视野最大宽
	float maxY = 0;//视野最大高
	//基本参数
	float leftWidth = 1;
	float rightWidth = 1;
	float topWidth = 1;
	float bottomWidth = 1;
	float heightGap = 0;
	float widthGap = 0;
	string prefabName = "node";
	GameObject SystemObject = null;
	GameObject scaleLinkObjs;
	GameObject moveLinkObjs;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//设置场景参数
	void SetSceneArgs(){
		x = leftWidth;
		y = 0;

		maxYInRow = 0;
		maxWidth = 0;

		maxExtX = 0;//生成的场景最大宽
		maxExtY = 0;//生成的场景最大高
		maxX = 0;//视野最大宽
		maxY = 0;//视野最大高
		GameObject msgBox = GameObject.Find("msgBox");
		if(msgBox != null){
			Destroy(msgBox);
		}
	}

	public void CreateScene(){
		CloseLinkScene();
		if(GameObject.Find("moveLinkObjs") == null){
			scaleLinkObjs = new GameObject();
			moveLinkObjs = new GameObject();
		}
		SetSceneArgs();
		scaleLinkObjs.name = "scaleLinkObjs";
		moveLinkObjs.name = "moveLinkObjs";
		string str = GameObject.Find("SystemObject").GetComponent<SystemSetting>().GetPortSceneData();
		if(str != null){
			//this.transform.GetComponent<SystemSetting>().SetSceneData(null);
            this.transform.GetComponent<SystemSetting>().SetPortSceneData(null);
            Debug.Log("linkdata: " + str);
		}
		JsonData jd = (JsonMapper.ToObject(str))["linkData"];
		for (int i = 0; i < jd.Count; i++) {
			Vector2 NodeSize = CreateNode(jd[i]);
 		}

		maxExtX = x + rightWidth - widthGap;
		maxExtY = y + topWidth - heightGap;
		moveLinkObjs.transform.position = new Vector3(maxExtX/2f, 0f, 0f);
			
		GameObject temePrefab = (GameObject)Resources.Load("Prefab/Room");
		GameObject linkPanel = (GameObject)Instantiate(temePrefab, new Vector3(maxExtX/2f, 0f, -5f), Quaternion.identity);
		linkPanel.AddComponent<BoxCollider2D>();
		linkPanel.name = "linkPanel";
		linkPanel.transform.localScale = new Vector3(maxExtX, maxExtY, 0f);
		CreateCloseIcon(linkPanel);
		linkPanel.transform.parent = scaleLinkObjs.transform;
		scaleLinkObjs.transform.parent = moveLinkObjs.transform;
		moveLinkObjs.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f);


	}
	//生成节点
	//jd格式：{\"NodePrefab\":\"\", \"NodeInstance\":\"\",\"NodeType\":\"\",\"Id\":\"\",\"Length\":\"\",\"HighLight\":\"\",\"Activity\":\"\"}
	Vector2 CreateNode(JsonData jd){
		string prefab = "";
		string name = "";
		string id = "";
		string highLight = "";
		string activity = "";
		string length = "";
		string nodeType = "";
		if(((IDictionary)jd).Contains("nodePrefab") && jd["nodePrefab"] != null){
			prefab = jd["nodePrefab"].ToString();
		}
		if(((IDictionary)jd).Contains("nodeInstance") && jd["nodeInstance"] != null){
			name = jd["nodeInstance"].ToString();
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
		if(((IDictionary)jd).Contains("length") && jd["length"] != null){
			length = jd["length"].ToString();
		}
		if(((IDictionary)jd).Contains("nodeType") && jd["nodeType"] != null){
			nodeType = jd["nodeType"].ToString();
		}	
		GameObject nodePrefab = (GameObject)Resources.Load("Prefab/" + prefab);
		if(nodePrefab == null){
			nodePrefab = (GameObject)Resources.Load("Prefab/" + prefabName);
		}
		GameObject node = (GameObject)Instantiate(nodePrefab, Vector3.zero, Quaternion.identity);
		Debug.Log("nodeType: " + nodeType);
		if("switch".Equals(nodeType)){
			node.tag = "switch";
		}
		if("server".Equals(nodeType)){
			node.tag = "server";
		}
		if("line".Equals(nodeType)){
			node.tag = "line";
		}
		node.name = name;
		//增加碰撞盒，检测鼠标事件
		node.AddComponent<BoxCollider2D>();


		Vector2 size = node.GetComponent<BoxCollider2D>().size;
		node.AddComponent<DeviceMouseEvent>();

		node.transform.position = new Vector3(x + size.x/2, y, -6f);
		node.transform.parent = scaleLinkObjs.transform;
		x += (size.x + widthGap);
		return size;
	}


	//设置最大宽度
	//void MaxWidth(float width){
	//	maxWidth = (maxWidth > width ? maxWidth : width);
	//}

	void CreateCloseIcon(GameObject linkPanel){
		if(GameObject.Find("senceObjes")){
			GameObject prefab = (GameObject)Resources.Load("Prefab/close");
			if(prefab != null){
				GameObject closeIcon = (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity);
				closeIcon.name = "linkSceneCloseIcom";
				closeIcon.AddComponent<BoxCollider2D>();
				Debug.Log("closeIconSize: "  + closeIcon.transform.GetComponent<BoxCollider2D>().size);
				closeIcon.AddComponent<DeviceMouseEvent>();
				Vector3 objsSize = new Vector2(linkPanel.transform.GetComponent<BoxCollider2D>().size.x * linkPanel.transform.localScale.x, 
					linkPanel.transform.GetComponent<BoxCollider2D>().size.y * linkPanel.transform.localScale.y);
				Vector3 closeIconSize = closeIcon.transform.GetComponent<BoxCollider2D>().size;
				Debug.Log("closeIconSize: "  + closeIconSize);
				closeIcon.transform.position = new Vector3(objsSize.x - closeIconSize.x, (objsSize.y - closeIconSize.y)/2f, -6f);
				closeIcon.transform.parent = scaleLinkObjs.transform;
			}
		}
	}

	//关闭场景的公共方法
	public void CloseLinkScene(){
		if(GameObject.Find("moveLinkObjs")){
			Destroy(GameObject.Find("moveLinkObjs"));
		}
	}
}
