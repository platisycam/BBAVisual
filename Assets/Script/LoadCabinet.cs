using UnityEngine;
using System.Collections;
using LitJson;

public class LoadCabinet : MonoBehaviour {
	RowData[] rowDatas;
	string rowid;
	bool enLoading = false;
	float x;
	float y;
	string prefabName;
	string kind;
	string instanceName;
	string bEquid;
	string equLabel;
	string roomid;
	int enterNo = 0;
	GameObject systemObject;
	GameObject canvas;
	// Use this for initialization
	void Start () {
		rowDatas = GameObject.Find("SystemObject").GetComponent<SystemSetting>().GetRowDatas();
		systemObject = GameObject.Find("SystemObject");
		canvas = GameObject.Find("Canvas");
	}

	// Update is called once per frame
	void Update () {
		if(systemObject.GetComponent<SystemSetting>().GetEnLoaded()){
			LoadedCabinet();
		}
		if(systemObject.GetComponent<SystemSetting>().GetIsLoading()){
			for(int i = 0; i < rowDatas.Length; i++){
//				if(this.transform.position.y > (rowDatas[i].y + this.transform.GetComponent<BoxCollider2D>().size.y/4f) && 
//						(this.transform.position.y < (rowDatas[i].y + this.transform.GetComponent<BoxCollider2D>().size.y*3/4f))){
				if(this.transform.position.x > (rowDatas[i].x + this.transform.GetComponent<BoxCollider2D>().size.x/2f) &&
					this.transform.position.y > (rowDatas[i].y + this.transform.GetComponent<BoxCollider2D>().size.y/4f) && 
					(this.transform.position.y < (rowDatas[i].y + this.transform.GetComponent<BoxCollider2D>().size.y*3/4f))){
					rowid = rowDatas[i].id;
					x = rowDatas[i].x + this.transform.GetComponent<BoxCollider2D>().size.x/2f;
					y = rowDatas[i].y + this.transform.GetComponent<BoxCollider2D>().size.y/2f;
					enLoading = true;
					systemObject.GetComponent<SystemSetting>().SetEnloading(enLoading);
					break;
				}else{
					enLoading = false;
					systemObject.GetComponent<SystemSetting>().SetEnloading(enLoading);
				}

			}
			if(enLoading && enterNo == 0){
				this.transform.GetComponent<SpriteRenderer>().color = Color.green;
			}else{
				this.transform.GetComponent<SpriteRenderer>().color = Color.red;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D e){
		//判断触发那个物体
		if("cabinet".Equals(e.tag)){
			enterNo++;
		}
	}

	void OnTriggerExit2D(Collider2D e){
		//判断触发那个物体
		if("cabinet".Equals(e.tag)){
			enterNo--;
		}
	}

	public void LoadedCabinet(){
		systemObject.GetComponent<SystemSetting>().SetLoadingDevice();
        this.transform.position = new Vector3(x, y, -1f);
		this.tag = "cabinet";
		this.transform.parent = GameObject.Find(rowid).transform;
		this.transform.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 1f);
		Destroy(this.GetComponent<LoadCabinet>());
	}

	public void SetCabinetInfo(string prefabName, string kind, string instanceName, string bEquid, string equLabel){
		this.prefabName = prefabName;
		this.kind = kind;
		this.instanceName = instanceName;
		this.bEquid =bEquid;
		this.equLabel = equLabel;
		this.roomid = GameObject.Find("SystemObject").GetComponent<SystemSetting>().GetRoomid();
	}

	public string GetCabinetInfo(){
//		string str = "loadDeviceInfo?" + "{BEquid:" + bEquid + ";EquInstance:" + instanceName + ";Computerroomid:" + roomid 
//			+ ";EquLabel:"+ equLabel + ";rowid:" + rowid + ";EquKind:" + kind + ";Action:load" + "}";
		string str = "loadDeviceInfo?" + "{BEquid:" + bEquid + ";EquInstance:" + instanceName + ";Computerroomid:" + roomid 
			+ ";EquLabel:"+ equLabel + ";rowid:" + rowid + ";EquKind:" + kind + ";Action:load" + "}";

		return str;
	}
}
