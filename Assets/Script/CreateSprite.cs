using UnityEngine;
using System.Collections;

public class CreateSprite : MonoBehaviour {
	string deviceName = "panel001";
	string portName = "port1";


	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {


	}

	//生成面板实例
	public void CreatePanelClone(string PrefabName){
		Debug.Log(PrefabName);
		//场景里只能有一个Panel，生成Panel前先检查是否存在Panel。
		if(!GameObject.FindWithTag("panel")){
			GameObject prefab = (GameObject)Resources.Load("Prefab/" +PrefabName);
			GameObject panel = (GameObject)Instantiate(prefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
			panel.name = deviceName;
			//增加碰撞盒，检测鼠标事件
			panel.AddComponent<BoxCollider2D>();
			//panel.AddComponent<Rigidbody2D>();
			//panel.GetComponent<BoxCollider2D>().isTrigger = true;
			//panel.GetComponent<Rigidbody2D>().isKinematic = true;
			//增加标签
			panel.transform.tag = "panel";
			//增加鼠标事件脚本
			panel.AddComponent<SpriteMouseEvent>();
			//调用脚本中的followMouse方法
			//panel.GetComponent<SpriteMouseEvent>().followMouse();
		}
	}

	public void CreatePortClone(string PrefabName){
		if(GameObject.FindWithTag("panel")){
			GameObject prefab = (GameObject)Resources.Load("Prefab/" +PrefabName);
			GameObject port = (GameObject)Instantiate(prefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
			port.name = portName;
			port.AddComponent<BoxCollider2D>();
			port.AddComponent<Rigidbody2D>();
			port.GetComponent<BoxCollider2D>().isTrigger = true;
			port.GetComponent<Rigidbody2D>().isKinematic = true;
			//增加标签
			port.transform.tag = "port";
			port.AddComponent<SpriteMouseEvent>();
			port.GetComponent<SpriteMouseEvent>().FollowMouse();
		}
	}

	public GameObject CreateLayoutPortsClone(string PrefabName){
		if(GameObject.FindWithTag("panel")){
			GameObject prefab = (GameObject)Resources.Load("Prefab/" +PrefabName);
			GameObject port = (GameObject)Instantiate(prefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
			port.name = portName;
			port.AddComponent<BoxCollider2D>();
			port.AddComponent<Rigidbody2D>();
			port.GetComponent<BoxCollider2D>().isTrigger = true;
			port.GetComponent<Rigidbody2D>().isKinematic = true;
			//增加标签
			port.transform.tag = "port";
//			port.AddComponent<SpriteMouseEvent>();
//			port.GetComponent<SpriteMouseEvent>().followMouse();
			return port;
		}
		else return null;
	}
}
