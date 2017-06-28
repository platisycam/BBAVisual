using UnityEngine;
using System.Collections;

public class LoadDevice : MonoBehaviour {
	string cabinetInstance;
	bool enLoading = false;
	GameObject cabinet1;
	GameObject cabinet2;
	GameObject cabinet0;
	float uHeight = 1.259912f;
	float uBottom = 1.735892f;
	int triggerDeviceNo = 0;
	int u;
	string prefabName;
	string kind;
	string instanceName;
	string bEquid;
	string equLabel;
	GameObject systemObject;
	GameObject canvas;
	// Use this for initialization
	void Start () {
		systemObject = GameObject.Find("SystemObject");
		canvas = GameObject.Find("Canvas");
		canvas.GetComponent<ShowDeviceInformation>().CreateDeviceInformation(this.gameObject);
	}

	// Update is called once per frame
	void Update () {
		if(systemObject.GetComponent<SystemSetting>().GetEnLoaded()){
			LoadedDevice();
		}
		if(systemObject.GetComponent<SystemSetting>().GetIsLoading()){
			if(cabinet1 != null && cabinet2 == null){
				cabinet0 = cabinet1;
			}else if(cabinet2 != null && cabinet1 == null){
				cabinet0 = cabinet2;
			}else if(cabinet1 == null && cabinet2 == null){
				cabinet0 = null;
			}else if(Mathf.Abs(this.transform.position.x - cabinet1.transform.position.x) 
				< Mathf.Abs(this.transform.position.x - cabinet2.transform.position.x)){
				cabinet0 = cabinet1;
			}else{
				cabinet0 = cabinet2;
			}
			if(cabinet0 != null && triggerDeviceNo == 0){
				if(this.transform.position.x > (cabinet0.transform.position.x - cabinet0.GetComponent<BoxCollider2D>().size.x/4f) &&
					this.transform.position.x < (cabinet0.transform.position.x + cabinet0.GetComponent<BoxCollider2D>().size.x/4f) &&
					this.transform.position.y - this.GetComponent<BoxCollider2D>().size.y/2f > (cabinet0.transform.position.y - cabinet0.GetComponent<BoxCollider2D>().size.y/2f + uBottom ) &&
					this.transform.position.y - this.GetComponent<BoxCollider2D>().size.y/2f < (cabinet0.transform.position.y + (cabinet0.GetComponent<BoxCollider2D>().size.y/2f))){
					this.transform.GetComponent<SpriteRenderer>().color = Color.green;
					systemObject.GetComponent<SystemSetting>().SetEnloading(true);
					u = Mathf.FloorToInt(((this.transform.position.y - this.GetComponent<BoxCollider2D>().size.y/2f) - 
						(cabinet0.transform.position.y - cabinet0.GetComponent<BoxCollider2D>().size.y/2f + uBottom))/uHeight + 1);
					canvas.GetComponent<ShowDeviceInformation>().EnDisplay();
					canvas.GetComponent<ShowDeviceInformation>().DisplayDeviceInformation(u.ToString());
				}else{
					systemObject.GetComponent<SystemSetting>().SetEnloading(false);
					this.transform.GetComponent<SpriteRenderer>().color = Color.red;
					//销毁
					canvas.GetComponent<ShowDeviceInformation>().UnDisplay();
				}
			}else if(triggerDeviceNo != 0){
				systemObject.GetComponent<SystemSetting>().SetEnloading(false);
				this.transform.GetComponent<SpriteRenderer>().color = Color.red;
				//销毁
				canvas.GetComponent<ShowDeviceInformation>().UnDisplay();
			}
		}
	}


	public void LoadedDevice(){
        systemObject.GetComponent<SystemSetting>().SetLoadingDevice();
		this.transform.position = new Vector3(cabinet0.transform.position.x, cabinet0.transform.position.y - cabinet0.GetComponent<BoxCollider2D>().size.y/2f + uBottom + (u - 1) * uHeight + this.GetComponent<BoxCollider2D>().size.y/2f, -2f);
		this.transform.parent = cabinet0.transform;
		this.tag = "device";
		this.transform.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 1f);
		GameObject.Find("Canvas").GetComponent<ShowDeviceInformation>().DestoryDeviceInformation();
		Destroy(this.GetComponent<LoadDevice>());
	}

	void OnTriggerEnter2D(Collider2D e){
		//判断触发那个物体
		if("cabinet".Equals(e.tag)){
			if(cabinet1 == null){
				cabinet1 = e.gameObject;
			}else if(cabinet2 == null){
				cabinet2 = e.gameObject;
			}else if(Vector3.Distance(this.transform.position, cabinet1.transform.position) > 
				Vector3.Distance(this.transform.position, cabinet2.transform.position)){
				cabinet1 = e.gameObject;
			}else{
				cabinet2 = e.gameObject;
			}
		}
		if("device".Equals(e.tag)){
			triggerDeviceNo++;
		}
	}

	void OnTriggerExit2D(Collider2D e){
		//判断触发那个物体
		if("cabinet".Equals(e.tag)){
			if(e.gameObject == cabinet1){
				cabinet1 = null;
			}else if(e.gameObject == cabinet2){
				cabinet2 = null;
			}
		}
		if("device".Equals(e.tag)){
			triggerDeviceNo--;
		}
	}

	public void SetDeviceInfo(string prefabName, string kind, string instanceName, string bEquid, string equLabel){
		this.prefabName = prefabName;
		this.kind = kind;
		this.instanceName = instanceName;
		this.bEquid =bEquid;
		this.equLabel = equLabel;
	}

	public string GetDeviceInfo(){
		string fEquid = cabinet0.name;
		string str = "loadDeviceInfo?" + "{BEquid:" + bEquid + ";EquInstance:" + instanceName + ";Fequid:" + fEquid 
			+ ";Slot:" + u + ";EquLabel:"+ equLabel + ";EquKind:" + kind +";Action:load}";
		return str;
	}
		
}
