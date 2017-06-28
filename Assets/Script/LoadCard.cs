using UnityEngine;
using System.Collections;

public class LoadCard : MonoBehaviour {
	GameObject device;
	GameObject[] slots;
	GameObject slotOjb;
	bool isOverlap;
	bool isInDevice;
	string prefabName;
	string kind;
	string instanceName;
	string bEquid;
	string equLabel;
	GameObject systemObject;
	GameObject canvas;
	// Use this for initialization
	void Start () {
		slots = GameObject.FindGameObjectsWithTag("slot");
		systemObject = GameObject.Find("SystemObject");
		canvas = GameObject.Find("Canvas");
		canvas.GetComponent<ShowDeviceInformation>().CreateDeviceInformation(this.gameObject);
	}
	
	// Update is called once per frame

	void Update () {
		if(systemObject.GetComponent<SystemSetting>().GetEnLoaded()){
			LoadedCard();
		}
		if(systemObject.GetComponent<SystemSetting>().GetIsLoading()){
			if(device != null && slots.Length != 0 && !isOverlap){
				for(int i = 0; i < slots.Length; i++){
					Debug.Log("device: " + device + ", slots.length: " + slots[i].transform.parent);
					if(device.name.Equals(slots[i].transform.parent.name)){
						if(this.transform.position.x > slots[i].transform.position.x - this.GetComponent<BoxCollider2D>().size.x/4f &&
							this.transform.position.x < slots[i].transform.position.x + this.GetComponent<BoxCollider2D>().size.x/4f &&
							this.transform.position.y > slots[i].transform.position.y - this.GetComponent<BoxCollider2D>().size.y/2f &&
							this.transform.position.y < slots[i].transform.position.y + this.GetComponent<BoxCollider2D>().size.y/2f){
							isInDevice = true;
							slotOjb = slots[i];
							break;
						}else{
							isInDevice = false;
							slotOjb = null;
						}
					}else{
						isInDevice = false;
						slotOjb = null;
					}
				}
			}
			if(isInDevice && !isOverlap && systemObject.GetComponent<SystemSetting>().GetIsLoading()){
				systemObject.GetComponent<SystemSetting>().SetEnloading(true);
				canvas.GetComponent<ShowDeviceInformation>().DisplayDeviceInformation(slotOjb.name.Replace("Pmark",""));
				this.transform.GetComponent<SpriteRenderer>().color = Color.green;
				//显示U位的方法
				canvas.GetComponent<ShowDeviceInformation>().EnDisplay();
			}else{
				systemObject.GetComponent<SystemSetting>().SetEnloading(false);
				this.transform.GetComponent<SpriteRenderer>().color = Color.red;
				//销毁U位的方法
				canvas.GetComponent<ShowDeviceInformation>().UnDisplay();
			}
		}
	}

	void DisplayU(int n){
		Debug.Log(n);
	}

	public void LoadedCard(){
        systemObject.GetComponent<SystemSetting>().SetLoadingDevice();
        this.transform.position = slotOjb.transform.position;
		this.transform.parent = device.transform;
		this.tag = "card";
		this.transform.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 1f);
		GameObject.Find("Canvas").GetComponent<ShowDeviceInformation>().DestoryDeviceInformation();
		Destroy(this.GetComponent<LoadDevice>());
	}

	void OnTriggerEnter2D(Collider2D e){
		if("device".Equals(e.tag)){
			device = e.gameObject;
		}
		if("card".Equals(e.tag)){
			isOverlap = true;
		}
	}

	void OnTriggerExit2D(Collider2D e){
		if("device".Equals(e.tag)){
			device = null;
		}
		if("card".Equals(e.tag)){
			isOverlap = false;
		}
	}

	public void SetCardInfo(string prefabName, string kind, string instanceName, string bEquid, string equLabel){
		this.prefabName = prefabName;
		this.kind = kind;
		this.instanceName = instanceName;
		this.bEquid =bEquid;
		this.equLabel = equLabel;
	}

	public string GetCardInfo(){
		string slot = slotOjb.name.Replace("Pmark","");
		string fEquid = device.name;
		string str = "loadDeviceInfo?" + "{BEquid:" + bEquid + ";EquInstance:" + instanceName + ";Fequid:" + fEquid 
			+ ";Slot:" + slot + ";EquLabel:"+ equLabel + ";EquKind:" + kind +";Action:load}";
		return str;
	}
}
