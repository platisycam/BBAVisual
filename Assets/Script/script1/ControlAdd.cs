using UnityEngine;
using System.Collections;
using LitJson;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlAdd : MonoBehaviour {
	public GameObject item;
	private GameObject content;
	private bool isEmpty = true;
	//string s = "[{\"value\":\"FD320.1\",\"key\":\"操作间名称\"},{\"value\":\"3\",\"key\":\"机柜数量\"}]";
	//string ss="[{\"value\":\"FD320.1-1-2-A\",\"key\":\"端口名称\"},{\"value\":\"01\",\"key\":\"端口号\"},{\"value\":\"铜口\",\"key\":\"端口类型\"},{\"value\":\"未用\",\"key\":\"端口状态\"}]";
	//string ss1="[{\"value\":\"交换机实例1\",\"key\":\"设备名称\"},{\"value\":\"3650-48PoE+-4x10G\",\"key\":\"设备型号\"},{\"value\":\"(32,32)\",\"key\":\"所在U位\"},{\"value\":\"思科\",\"key\":\"品牌\"},{\"value\":\"普通交换机\",\"key\":\"kind\"},{\"value\":\"交换机\",\"key\":\"type\"},{\"value\":\"24\",\"key\":\"铜口数\"},{\"value\":\"0\",\"key\":\"光口数\"},{\"value\":\"FD320.1-1\",\"key\":\"所在机柜名称\"},{\"value\":\"FD320.1-1\",\"key\":\"所在操作间名称\"}]";

	// Use this for initialization
	void Start () {
		content = GameObject.Find("Content");

	}
	
	// Update is called once per frame
	void Update () {
//		string s = "[{\"key\":\"名称\",\"value\":\"FX10023100237\"},{\"key\":\"所属\",\"value\":\"P310-E-08\"},{\"key\":\"U位置\",\"value\":\"12-13\"},{\"key\":\"布局\",\"value\":\"\"},{\"key\":\"设备型号\",\"value\":\"1U设备\"}]";
//		if(Input.GetKeyDown(KeyCode.C)){
//			unityModelBaseData (ss1);
//		}
	}

    //测试函数
    //public void testdata()
    //{
    //    unitymodelbasedata(ss1);
    //}

    //基本信息
    public void unityModelBaseData(string receiverstr){
		if(isEmpty){
			//正式数据
			JsonData jd = JsonMapper.ToObject(receiverstr);
			//测试数据
			//JsonData jd = JsonMapper.ToObject(s);
			//测试数据结束
			string name;
			string value; 
			for(int i = 0; i < jd.Count; i++){
				name =jd[i]["key"].ToString();
				value=jd[i]["value"].ToString();
				GameObject go = (GameObject)Instantiate(item);
				go.transform.parent = content.transform;
				go.transform.localScale = Vector3.one;
				Debug.Log("key:" + name + ", value:" + value);
				go.transform.Find("Key").GetComponent<Text> ().text=name;
				go.transform.Find("Value").GetComponent<Text> ().text=value;
//				string[] CellContent = new string[]{"IP:\nPort:"};
//				value = CellContent[0].ToString();
			}
			isEmpty = !isEmpty;
		}

	}
	//关闭面板
	public void OncloseDevice(){
        Destroy(this);
		if(GameObject.Find("msgBox")){
			Destroy(GameObject.Find("msgBox"));
		}
	}
	//增加列表项
//	public void AddItem(){
//		Panel = Resources.Load ("Popprefab/Device", typeof(GameObject)) as GameObject;
//		_Panel =Instantiate (Panel,transform.position,Quaternion.identity)as GameObject;
//		_Panel.transform.parent = GameObject.Find ("Canvas").transform;
//		_Panel.transform.localScale = Vector3.one;
//	}

	//显示链路板
	public void ChangeSence(){
		//生成链路物体
		GameObject.Find("SystemObject").GetComponent<SystemSetting>().InitScene("LinkScene");
		//画线
		GameObject.Find("SystemObject").GetComponent<PortLinkLine>().BeginDrawLine();
		GameObject.Find ("Canvas").GetComponent<AddUI> ().ShowHiddenName (false);
	}

	//设备下架
	public void DownLoadDevice(){
		GameObject.Find("SystemObject").GetComponent<ComunicationAction>().SendMsgToWeb(GameObject.Find ("SystemObject").GetComponent<UnloadObj> ().GetUnloadObjInfo ());
	}
}
