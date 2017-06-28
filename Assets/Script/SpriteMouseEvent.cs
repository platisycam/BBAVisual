using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteMouseEvent : MonoBehaviour {
	Vector3 offset;
	bool isFollow = true;//Sprite是否在跟随状态
	int isInPortNo = 0; //重叠的Port个数
	bool isInPanel = false;//是否与Panel重叠
	Color origColor = new Color();
	Color redColor = new Color(1f, 0f, 0f);
	Color greenColor = new Color(0f, 1f, 0f);
	GameObject pareObj;
	string port = "port";
	string panel = "panel";
	Vector3 panelBoxSize;//Panel碰撞盒大小
	Vector3	panelLayoutSize;//Panel摆放大小
	// Use this for initialization
	void Start () {
		if(port.Equals(this.tag) ){
			//如果是端口，需要找到做为父物体的Panel
			pareObj = GameObject.FindWithTag(panel);
			//取得Panel和Port碰撞盒大小，根据两个碰撞盒，设置一个摆放位置的大小。
			panelBoxSize = pareObj.transform.GetComponent<BoxCollider2D>().size;
			Vector3 PortBoxSize = this.transform.GetComponent<BoxCollider2D>().size;
			panelLayoutSize = new Vector3(panelBoxSize.x - 2f*PortBoxSize.x, panelBoxSize.y - 2f*PortBoxSize.y);
			pareObj.GetComponent<BoxCollider2D>().size = panelLayoutSize;
		}
		if(this.GetComponent<SpriteRenderer>()){
			origColor = this.GetComponent<SpriteRenderer>().color;
		}
		SetStatus();
	}

	// Update is called once per frame
	void Update () {
		if(isFollow){
			SpriteMove();
			if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Escape)){
				Debug.Log("D");
				DestroyObj();
			}
		}

	}

	//鼠标单击事件，一次单击用于释放Sprite结束跟随状态，一次单击开始跟随状态。
	void OnMouseDown(){
		if(GameObject.Find("UnityObject").GetComponent<UIEventAction>().getIsDelete()){
			DestroyObj();
		}
		if(isFollow){
			//如果是跟随状态检查是否可以释放物体
			CheckReleaseSprite();
		}else{
			//如果不是跟随状态，开始跟随状态，需要拿到鼠标位置和物体位置的偏移值。
			GetOffset();
			isFollow = true;
			if(port.Equals(this.tag)){
				pareObj.GetComponent<BoxCollider2D>().size = panelLayoutSize;
			}
		}

		SetStatus();
	}

	void OnMouseDrag(){

	}

	void OnTriggerEnter2D(Collider2D e){
		if("panel".Equals(e.tag)){
			isInPanel = true;
		}
		if("port".Equals(e.tag)){
			isInPortNo += 1;
		}
		SetStatus();
	}

	void OnTriggerExit2D(Collider2D e){
		if("panel".Equals(e.tag)){
			isInPanel = false;
		}
		if("port".Equals(e.tag)){
			isInPortNo -= 1;
		}
		SetStatus();
	}

	//取到物体中心相对于鼠标点击位置的偏移。
	void GetOffset(){
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0f;
		offset = this.transform.position - mousePos;
	}

	//Sprite位移方法，实现Sprite位移和冲突检测
	void SpriteMove(){
		//实现位移
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0f;//改变一下z值的目的在于，如果物体跟鼠标的z值一致，那么将无法侦查鼠标点击。
		this.transform.position = mousePos + offset;
		//冲突检测
	}

	//Sprite跟随鼠标，用于点击后释放前的Sprite跟随。
	public void FollowMouse(){
		isFollow = true;
	}

	void CheckReleaseSprite(){
		if(("port".Equals(this.tag) && isInPanel && isInPortNo == 0) || panel.Equals(this.tag)){
			//可以放置
			if(panel.Equals(this.tag)){
				this.transform.position += new Vector3(0f, 0f, 1f);
			}else if("port".Equals(this.tag) && pareObj){
				this.transform.parent = pareObj.transform;
				pareObj.GetComponent<BoxCollider2D>().size = panelBoxSize;
				GameObject[] ports = GameObject.FindGameObjectsWithTag(port);
				//将端口数量作为端口名称
				this.name = ports.Length.ToString();
			}
			isFollow = false;
		}
	}

	void SetStatus(){
		if(isFollow){
			if(("port".Equals(this.tag) && isInPanel && isInPortNo == 0) || "panel".Equals(this.tag)){
				this.GetComponent<SpriteRenderer>().color = greenColor;
			}else if("port".Equals(this.tag) && (!isInPanel || isInPortNo > 0)){
				this.GetComponent<SpriteRenderer>().color = redColor;
			}
		}else{
			this.GetComponent<SpriteRenderer>().color = origColor;
		}
	}

	void DestroyObj(){
		Debug.Log("destroyobj");
		if(port.Equals(this.tag)){
			pareObj.GetComponent<BoxCollider2D>().size = panelBoxSize;
		}
		Destroy(this.gameObject);
		GameObject.Find("UnityObject").GetComponent<UIEventAction>().setIsDelete(false);
	}
}
