using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using LitJson;

public class SystemSetting : MonoBehaviour {
	//场景名称
	string sceneName = "";
	//摄像机初始化属性
	Vector3 camOrgPos = Vector3.zero;
    float startTime = 0f;
    Vector3 offset;
    //鼠标点击事件
    float mousedownTime;
	float singledonwDelay = 0.3f;
	bool isLoading = false;//处于等待上架状态
	bool enLoading = false;//处于可以上架状态
	bool enLoaded = false;//处于上架状态
	//临时数据
	string sceneData;
	string portSceneData;
	string roomName;
	RowData[] rowDatas;
	string roomid;
	string drawLineData;
	string uStatisticsData;
	//上架功能数据
	string prefabName;
	string instanceName;
	string kind;
	GameObject unloadDevice;//下架设备
	GameObject zonePicture;
    GameObject canvas;
    GameObject loadingDevice;//上架设备
    //资源码
    bool enUnload = false;//允许下架
	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas");

        DontDestroyOnLoad(this);
		zonePicture = GameObject.Find("ZonePicture");
		Camera.main.orthographicSize = 7f;
		Debug.Log("ready");
		this.GetComponent<ComunicationAction>().SendMsgToWeb("ready");
        //Camera.main.gameObject.AddComponent<ViewpointCtrl>();
        //测试代码

        //ZoneToRoomChangeSence();
        //if (SetSceneData(this.GetComponent<TestScript>().getStr5()))
        //{
        //    InitScene("roomscene");
        //}


        //if (this.GetComponent<SystemSetting>().setPortSceneData(this.GetComponent<TestScript>().getStr4()))
        //{
        //    InitScene("LinkScene");
        //}
        //this.GetComponent<SystemSetting>().setUStatisticsData(this.GetComponent<TestScript>().getUs1());

        //测试代码结束
    }

    void OnGUI(){
        //if (gui.button(new rect(10, 10, 100, 30), "cabinet"))
        //{
        //    getloaddeviceinfo("{\"equinstance\":\"000110\",\"modelname\":\"g650-1\",\"equkind\":\"cabinet\",\"bequid\":\"eqjgg01000000042\"}");
        //}
        //if (gui.button(new rect(10, 50, 100, 30), "device"))
        //{
        //    loaddevice("3650-48poe+-4x10g", "device", "a", "123", "s");
        //}
        //if (gui.button(new rect(10, 90, 100, 30), "card"))
        //{
        //    loadcard("ws-x45-sup6l-e", "card", "a", "123", "s");

        //}
    }


    // Update is called once per frame
    void Update () {
		if(Input.GetKey(KeyCode.R)){
			Debug.Log("in r");
			if(zonePicture.activeSelf.Equals(false)){
				Debug.Log("in if");
                this.GetComponent<SceneSetting>().ResetScene();
                zonePicture.SetActive(true);
			}
		}
        //loadingDevice = GameObject.FindGameObjectWithTag("loading");
        if (loadingDevice != null) {
            if (isLoading && "loading".Equals(loadingDevice.tag))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0f;//改变一下z值的目的在于，如果物体跟鼠标的z值一致，那么将无法侦查鼠标点击。
                loadingDevice.transform.position = mousePos + offset;
            }
            //增加上架状态时，ESC取消上架设备
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Press ESC!");
                isLoading = false;
                if (loadingDevice)
                {
                    Destroy(loadingDevice);
                    if (canvas.GetComponent<ShowDeviceInformation>())
                    {
                        canvas.GetComponent<ShowDeviceInformation>().DestoryDeviceInformation();
                    }
                }
            }
        }
       
    }

	public void InitScene(string scene){
		Debug.Log("scenename: " + scene);
		sceneName = scene;

        if ("RoomScene".Equals(scene)){
			//加载场景
			ZoneToRoomChangeSence();
            this.GetComponent<SceneSetting>().ResetScene();
            this.GetComponent<RoomSceneInit>().CreateScene();

		}
		if("DeviceScene".Equals(scene)){
			ZoneToRoomChangeSence();
            this.GetComponent<SceneSetting>().ResetScene();
            this.GetComponent<RoomSceneInit>().CreateScene();
			//this.GetComponent<DeviceSceneInit>().CreateScene();
		}
		if("PortScene".Equals(scene)){
            ZoneToRoomChangeSence();
            this.GetComponent<SceneSetting>().ResetScene();
            this.GetComponent<RoomSceneInit>().CreateScene();
            //this.GetComponent<PortSceneInit>().CreateScene();
        }
		if("LinkScene".Equals(scene)){
            ZoneToRoomChangeSence();
            this.GetComponent<LinkSceneInit>().CreateScene();
        }
	}

	

	public void GetLoadDeviceInfo(string str){
		if(roomid != null){
			string bEquid ="";
			string equLabel = "";
			//Debug.Log("device info");
			JsonData jd = JsonMapper.ToObject(str);
			if(((IDictionary)jd).Contains("ModelName") && jd["ModelName"] != null){
				prefabName = jd["ModelName"].ToString();
			}
			if(((IDictionary)jd).Contains("EquKind") && jd["EquKind"] != null){
				kind = jd["EquKind"].ToString();
			}
			if(((IDictionary)jd).Contains("EquInstance") && jd["EquInstance"] != null){
				instanceName = jd["EquInstance"].ToString();
			}
			if(((IDictionary)jd).Contains("BEquid") && jd["BEquid"] != null){
				bEquid = jd["BEquid"].ToString();
			}
			if(((IDictionary)jd).Contains("EquLabel") && jd["EquLabel"] != null){
				equLabel = jd["EquLabel"].ToString();
			}

			if(kind != null && !GameObject.FindGameObjectWithTag("loading")){
				switch(kind){
				case "cabinet":
					this.GetComponent<SceneSetting>().LoadCabinet(prefabName, kind, instanceName, bEquid, equLabel);
					break;
				case "device":
                        this.GetComponent<SceneSetting>().LoadDevice(prefabName, kind, instanceName, bEquid, equLabel);
					break;
				case "card":
                        this.GetComponent<SceneSetting>().LoadCard(prefabName, kind, instanceName, bEquid, equLabel);
					break;
				default:
					break;
				}
			}
		}else{
			//返回错误信息
			this.GetComponent<ComunicationAction>().SendMsgToWeb("RetrieveUnityData?error");
		}

	}

	
		

	//设置鼠标按下时间，结合MouseupTime方法判断单击事件
	public void MousedownTime(){
		this.mousedownTime = Time.time;
	}

	//判断鼠标点击事件脚本
	public bool IsMouseSingleClick(){
		if(Time.time - this.mousedownTime < singledonwDelay){
			return true;
		}else{
			return false;
		}
	}

    public void SetAuthority(string str) {
        Debug.Log("setAuthority: in");
        string[] strs = str.Split(';');
        for (int i = 0; i < strs.Length; i++) {
            //获得系统管理权限，则可以操作上下架功能
            if ("KSHSXJGL0001".Equals(strs[i])) {
                Debug.Log("setAuthority: ture");
                enUnload = true;
            }
        }
    }

    //用显隐代表ZoneSence场景切换到RoomSence场景
    public void ZoneToRoomChangeSence()
    {

        if (zonePicture)
        {
            zonePicture.gameObject.SetActive(false);
        }
    }

    public void MouseSingleDown(GameObject obj) {
        if (obj.tag.Equals("picture"))
        {
            this.GetComponent<ComunicationAction>().SendMsgToWeb("getManagementName?" + obj.name);
        }
        else if (obj.tag.Equals("switch") || obj.tag.Equals("line"))
        {
            Debug.Log("链路单击事件: " + obj.name);
            this.GetComponent<ComunicationAction>().SendMsgToWeb(
                "getLinkedObjectInfo?" + obj.tag + "?" + obj.name);
            canvas.GetComponent<AddUI>().AddMsgBox("LinkMsg");
        }
        else if (obj.tag.Equals("cabinet") || obj.tag.Equals("device") || obj.tag.Equals("card") || obj.tag.Equals("port") || obj.tag.Equals("room"))
        {
            //其他物体的单击事件
            Debug.Log("设备单击事件: " + obj.name + ", tag: " + obj.tag);
            if (obj.tag.Equals("device") || obj.tag.Equals("card"))
            {
                canvas.GetComponent<AddUI>().AddMsgBox("DeviceMsg");
                SetUnloadDevice(obj);
            }
            if (obj.tag.Equals("port"))
            {
                canvas.GetComponent<AddUI>().AddMsgBox("PortMsg");
                this.GetComponent<ComunicationAction>().SendMsgToWeb("portSceneData?" + obj.transform.GetComponent<Properties>().getid());
                this.GetComponent<ComunicationAction>().SendMsgToWeb("drawLineData?" + obj.transform.GetComponent<Properties>().getid());
            }
            if (obj.tag.Equals("room"))
            {
                canvas.GetComponent<AddUI>().AddMsgBox("RoomMsg");
                this.GetComponent<ComunicationAction>().SendMsgToWeb("uStatisticsData?" + obj.tag + "?" + obj.name);
            }
            if (obj.tag.Equals("cabinet"))
            {
                canvas.GetComponent<AddUI>().AddMsgBox("CabinetMsg");
                SetUnloadDevice(obj);
                this.GetComponent<ComunicationAction>().SendMsgToWeb("uStatisticsData?" + obj.tag + "?" + obj.name);
            }
            this.GetComponent<ComunicationAction>().SendMsgToWeb(
                "getGameObjectInfo?" + GetSceneName() + "?" + obj.tag + "?" + obj.name + "?" + obj.transform.parent.name);
        }
        else if ("loading".Equals(obj.tag))
        {
            loadingDevice = obj;
            //上架设备的单击事件
            if (!isLoading)
            {
                isLoading = true;
                SetOffset();
            }
            else if (isLoading && enLoading)
            {
                //上架设备跟随时的单击事件
                isLoading = false;
                enLoaded = true;
                //机柜上架
                if (kind != null)
                {
                    string loadData = "";
                    switch (kind)
                    {
                        case "cabinet":
                            loadData = obj.GetComponent<LoadCabinet>().GetCabinetInfo();
                            break;
                        case "device":
                            loadData = obj.GetComponent<LoadDevice>().GetDeviceInfo();
                            break;
                        case "card":
                            loadData = obj.GetComponent<LoadCard>().GetCardInfo();
                            break;
                        default:
                            break;
                    }
                    Debug.Log("loadData: " + loadData);
                    this.GetComponent<ComunicationAction>().SendMsgToWeb(loadData);
                }
            }
        }
        else if ("linkSceneCloseIcom".Equals(obj.name))
        {
            this.GetComponent<LinkSceneInit>().CloseLinkScene();
            obj.GetComponent<PortLinkLine>().DestoryDrawLine();
            SetUnloadDevice(null);
            GameObject.Find("Canvas").GetComponent<AddUI>().ShowHiddenName(true);
        }
    }

    public void SetLoadingDevice() {
        enLoaded = false;
        isLoading = false;
        loadingDevice = null;
    }

    void SetOffset()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        offset = loadingDevice.transform.position - mousePos;
    }

    public void SetRoomName(string roomName){
		this.roomName = roomName;
	}

	public string GetRoomName(){
		return this.roomName;
	}

	public bool SetSceneData(string sceneData){
        if (sceneData != null && !"".Equals(sceneData))
        {
            this.sceneData = sceneData;
            Debug.Log("inif");
            return true;
        }
        else {
            return false;
        }

	}

	public string GetSceneData(){
		return sceneData;
	}

	public void SetCamOrgPos(Vector3 postion){
		this.camOrgPos = postion;
	}

	public Vector3 GetCamOrgPos(){
		return this.camOrgPos;
	}

	public string GetSceneName(){
		return this.sceneName;
	}

	public string GetPortSceneData(){
		return this.portSceneData;
	}

	public bool SetPortSceneData(string portSceneData){
		this.portSceneData = portSceneData;
		return true;
	}

	public void SetIsLoading(bool isLoading){
		this.isLoading = isLoading;
	}

	public bool GetIsLoading(){
		return isLoading;
	}

	public void SetRowDatas(RowData[] rowDatas){
		this.rowDatas = rowDatas;
	}

	public RowData[] GetRowDatas(){
		//Debug.Log("get");
		return this.rowDatas;
	}

	public void SetEnloading(bool enLoading){
		this.enLoading = enLoading;
	}

	public bool GetEnloading(){
		return this.enLoading;
	}

	public void SetRoomid(string roomid){
		this.roomid = roomid;
	}

	public string GetRoomid(){
		return this.roomid;
	}

	public void SetEnLoaded(bool enLoaded){
		this.enLoaded = enLoaded;
	}

	public bool GetEnLoaded(){
		return this.enLoaded;
	}
		
	public string GetKind(){
		return this.kind;
	}

	public void SetDrawLineData(string drawLineData){
		this.drawLineData = drawLineData;
	}

	public string GetDrawLineData(){
		return this.drawLineData;
	}

	public void SetUStatisticsData(string uStatisticsData){
		this.uStatisticsData = uStatisticsData;
	}

	public string GetUStatisticsData(){
		return this.uStatisticsData;
	}

	public void SetUnloadDevice(GameObject unloadDevice){
		this.unloadDevice = unloadDevice;
	}

	public GameObject GetUnloadDevice(){
		return this.unloadDevice;
	}

    public bool GetEnUnload() {
        return this.enUnload;
    }

    public void SetEnUnload(bool enUnload) {
        this.enUnload = enUnload;
    }

}
