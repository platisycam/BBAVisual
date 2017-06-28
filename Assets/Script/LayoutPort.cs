using UnityEngine;
using System.Collections;


public class LayoutPort : MonoBehaviour {
	int rowNo = 2;
	int colNo = 3;
	float rowPitch = 0.8f;
	float colPitch = 0.8f;
	GameObject layoutBox;
	string port = "port01";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreatePorts(){
		//加上生成物体的脚本
		if(!this.GetComponent<CreateSprite>()){
			this.gameObject.AddComponent<CreateSprite>();
		}
		//生成一个Port实例，取得Port尺寸。
		GameObject prefab = (GameObject)Resources.Load("Prefab/" + port);
		GameObject portObj = Instantiate(prefab);
		portObj.AddComponent<BoxCollider2D>();
		if(portObj.GetComponent<BoxCollider2D>()){
			//根据Port尺寸，生成layoutBox的大小，取得值后销毁生成的Port。
			Vector3 portBoxSize = portObj.GetComponent<BoxCollider2D>().size;
			float box_x = portBoxSize.x * colNo + colPitch * (colNo - 1);
			float box_y = portBoxSize.y * rowNo + rowPitch * (rowNo - 1);
			Destroy(portObj);

			layoutBox = new GameObject();
			layoutBox.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			layoutBox.AddComponent<BoxCollider2D>();
			layoutBox.AddComponent<Rigidbody2D>();
			layoutBox.GetComponent<BoxCollider2D>().isTrigger = true;
			layoutBox.GetComponent<Rigidbody2D>().isKinematic = true;
			layoutBox.AddComponent<SpriteMouseEvent>();
			layoutBox.GetComponent<SpriteMouseEvent>().FollowMouse();
			//增加标签
			layoutBox.transform.tag = "layout";
			if(!layoutBox.GetComponent<BoxCollider2D>()){
				layoutBox.gameObject.AddComponent<BoxCollider2D>();
				layoutBox.GetComponent<BoxCollider2D>().size = new Vector3(box_x, box_y, 0f);
			}

			float org_x = layoutBox.transform.position.x - box_x/2 + portBoxSize.x/2;
			float org_y = layoutBox.transform.position.y + box_y/2 - portBoxSize.y/2;

			//批量生成port
			GameObject portTemp;
			for(int i = 0; i < colNo; i++){
				for(int j = 0; j < rowNo; j++){
					if(!this.GetComponent<CreateSprite>()){
						this.gameObject.AddComponent<CreateSprite>();
					}
					portTemp = this.GetComponent<CreateSprite>().CreateLayoutPortsClone(port);
					portTemp.transform.position = new Vector3(org_x + i*portBoxSize.x + i*colPitch, org_y + j*portBoxSize.y + j*rowPitch, 0f);
					if(layoutBox){
						portTemp.transform.parent = portTemp.transform;
					}
				}
			}
		}
	}
}