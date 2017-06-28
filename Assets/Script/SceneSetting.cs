using UnityEngine;
using System.Collections;

public class SceneSetting : MonoBehaviour {
    //摄像机数据
    float leftWidth = 20;
    float rightWidth = 20;
    float topWidth = 20;
    float bottomWidth = 20;
    float heightGap = 1;
    float widthGap = 1;
    float maxX = 0f;
    float maxY = 0f;
    float minX = 0f;
    float minY = 0f;
    float maxExtX = 0f;
    float maxExtY = 0f;
    float maxCamSize = 0f;
    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    //销毁场景下所有物体
    public void ResetScene()
    {
        //摄像机初始化属性
        Vector3 camOrgPos = Vector3.zero;
        leftWidth = 20;
        rightWidth = 20;
        topWidth = 20;
        bottomWidth = 20;
        heightGap = 1;
        widthGap = 1;
        minX = 0f;
        minY = 0f;
        maxExtX = 0f;
        maxExtY = 0f;
        maxCamSize = 0f;

        //this.GetComponent<SystemSetting>().SetSceneData("");
        //if (GameObject.Find("senceObjes"))
        //{
        //    Transform senceObjes = GameObject.Find("senceObjes").transform;
        //    for (int i = 0; i < senceObjes.childCount; i++)
        //    {
        //        Destroy(senceObjes.GetChild(i).gameObject);
        //    }
        //}
        if (GameObject.Find("Canvas"))
        {
            Transform canvas = GameObject.Find("Canvas").transform;
            for (int i = 0; i < canvas.childCount; i++)
            {
                Destroy(canvas.GetChild(i).gameObject);
            }
        }
        if (GameObject.Find("senceObjes"))
        {

            Destroy(GameObject.Find("senceObjes"));

        }

        if (GameObject.Find("moveLinkObjs"))
        {
            Destroy(GameObject.Find("moveLinkObjs"));
        }
        if (GameObject.FindWithTag("room"))
        {
            Destroy(GameObject.FindWithTag("room"));
        }
        if (GameObject.FindWithTag("creating"))
        {
            Destroy(GameObject.FindWithTag("creating"));
        }
        if (GameObject.FindWithTag("loading"))
        {
            Destroy(GameObject.FindWithTag("loading"));
        }

    }

    //上架机柜设备
    public void LoadCabinet(string prefabName, string kind, string instanceName, string bEquid, string equLabel)
    {
        GameObject temePrefab = (GameObject)Resources.Load("Prefab/" + prefabName);
        if (temePrefab == null)
        {
            temePrefab = (GameObject)Resources.Load("Prefab/" + "");
        }
        GameObject cabinet = (GameObject)Instantiate(temePrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -5f), Quaternion.identity);
        cabinet.tag = "loading";
        cabinet.name = instanceName;
        cabinet.AddComponent<BoxCollider2D>();
        cabinet.GetComponent<BoxCollider2D>().isTrigger = true;
        cabinet.AddComponent<Rigidbody2D>();
        cabinet.GetComponent<Rigidbody2D>().isKinematic = true;
        cabinet.AddComponent<DeviceMouseEvent>();
        cabinet.AddComponent<ShowCabinetName>();
        cabinet.AddComponent<LoadCabinet>();
        cabinet.GetComponent<ShowCabinetName>().DisplayCabinetName(equLabel);
        cabinet.GetComponent<LoadCabinet>().SetCabinetInfo(prefabName, kind, instanceName, bEquid, equLabel);
    }

    //上架柜内设备
    public void LoadDevice(string prefabName, string kind, string instanceName, string bEquid, string equLabel)
    {
        GameObject temePrefab = (GameObject)Resources.Load("Prefab/" + prefabName);
        if (temePrefab == null)
        {
            temePrefab = (GameObject)Resources.Load("Prefab/" + "");
        }
        GameObject device = (GameObject)Instantiate(temePrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -5f), Quaternion.identity);
        device.tag = "loading";
        device.name = instanceName;
        device.AddComponent<BoxCollider2D>();
        device.GetComponent<BoxCollider2D>().isTrigger = true;
        device.AddComponent<Rigidbody2D>();
        device.GetComponent<Rigidbody2D>().isKinematic = true;
        device.AddComponent<DeviceMouseEvent>();
        device.AddComponent<LoadDevice>();
        device.GetComponent<LoadDevice>().SetDeviceInfo(prefabName, kind, instanceName, bEquid, equLabel);
    }

    //上架板卡设备
    public void LoadCard(string prefabName, string kind, string instanceName, string bEquid, string equLabel)
    {

        GameObject temePrefab = (GameObject)Resources.Load("Prefab/" + prefabName);
        if (temePrefab == null)
        {
            temePrefab = (GameObject)Resources.Load("Prefab/" + "");
        }
        GameObject card = (GameObject)Instantiate(temePrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -5f), Quaternion.identity);
        card.tag = "loading";
        card.name = instanceName;
        card.AddComponent<BoxCollider2D>();
        card.GetComponent<BoxCollider2D>().isTrigger = true;
        card.AddComponent<Rigidbody2D>();
        card.GetComponent<Rigidbody2D>().isKinematic = true;
        card.AddComponent<DeviceMouseEvent>();
        card.AddComponent<LoadCard>();
        card.GetComponent<LoadCard>().SetCardInfo(prefabName, kind, instanceName, bEquid, equLabel);

    }

    public void setMinX(float x)
    {
        this.minX = x;
    }

    public float getMinX()
    {
        return this.minX;
    }

    public void setMinY(float y)
    {
        this.minY = y;
    }

    public float getMinY()
    {
        return this.minY;
    }

    public void setMaxCamSize(float maxCamSize)
    {
        this.maxCamSize = maxCamSize;
    }

    public float getMaxCamSize()
    {
        return this.maxCamSize;
    }

    public void setMaxExtX(float x)
    {
        this.maxExtX = x;
    }

    public float getMaxExtX()
    {
        return this.maxExtX;
    }

    public void setMaxExtY(float y)
    {
        this.maxExtY = y;
    }

    public float getMaxExtY()
    {
        return this.maxExtY;
    }

    public float getLeftWidth()
    {
        return this.leftWidth;
    }

    public float getRightWidth()
    {
        return this.rightWidth;
    }

    public float getTopWidth()
    {
        return this.topWidth;
    }

    public float getBottomWidth()
    {
        return this.bottomWidth;
    }

    public float getHeightGap()
    {
        return this.heightGap;
    }

    public float getWidthGap()
    {
        return this.widthGap;
    }
}
