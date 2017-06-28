using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using Vectrosity;

public class PortLinkLine : MonoBehaviour {
	public List<string> linkdata = new List<string> ();//存放画线数据
	public Dictionary<int, VectorLine> line=new Dictionary<int, VectorLine>();//画线插件初始化
	Vector3[] points; //点数组
	public Material linematerial;//线的颜色
	int linenum=0;//基本线计数标识

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
//		if (Input.GetKeyDown (KeyCode.B)) {
//			DestoryDrawLine ();
//		}
	}

	//开始画线接口
	public void BeginDrawLine(){
		Debug.Log("BeginDrawLine: ");
		string str = this.GetComponent<SystemSetting>().GetDrawLineData();
		if(str != null){
			Debug.Log("str: " + str);
			ReceiveLinkLineData (str);
			DrawLine (FindScenesVectorPoint()[0],FindScenesVectorPoint()[1],"green",1);
		}
	}

	//接收画线数据，存入linkdata列表中
	public void ReceiveLinkLineData(string str){
		
		linkdata.Clear ();

		JsonData jd = JsonMapper.ToObject (str);

		string upDevice;
		string upPort;
		string downDevice;
		string downPort;

		upDevice = jd["upDevice"].ToString ();
		linkdata.Add (upDevice);
		upPort = jd["upPort"].ToString ();
		linkdata.Add (upPort);
		downDevice = jd["downDevice"].ToString ();
		linkdata.Add (downDevice);
		downPort = jd["downPort"].ToString ();
		linkdata.Add (downPort);
	}

	//获取三维坐标
	Vector3[] FindScenesVectorPoint(){
		
		Vector3 startpoint = GameObject.Find (linkdata [0]).gameObject.transform.FindChild (linkdata [1]).transform.position;
		Vector3 endpoint = GameObject.Find (linkdata [2]).gameObject.transform.FindChild (linkdata [3]).transform.position;
		return new Vector3[]{ startpoint, endpoint };
	}

	//画线
	void DrawLine(Vector3 start, Vector3 end, string LineType, int PointType){
		
		points = new Vector3[]{new Vector3(start.x,start.y,-5f),new Vector3(end.x,end.y, -5f) };
		line.Add (linenum,new VectorLine("Line1",points,Color.red,linematerial,1.5f,Vectrosity.LineType.Continuous, Joins.Weld));
		line[linenum].Draw3DAuto ();
		linenum ++;
		//GameObject.Find ("Vector Line1").transform.position = new Vector3 (GameObject.Find ("Vector Line1").transform.position.x,GameObject.Find ("Vector Line1").transform.position.y,-4.0f);
	}

	//销毁画线
	public void DestoryDrawLine(){
		if (line != null) {
			for (int i = 0; i < linenum; i++) {
				VectorLine desline = line [i];
				Vectrosity.VectorLine.Destroy (ref desline);
			}
			linenum = 0;
			line.Clear();
		}
	}
}
