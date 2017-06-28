using UnityEngine;
using System.Collections;

public class Properties : MonoBehaviour {
	private string id;//端口ID
	private string lable;
	private string activity;//是否为活跃设备，即是否可以响应鼠标事件
	private string highLight;//高亮，是否突出显示
	private string length;
	private string type;
	private string status;
	private string isChecked;
	private Color usedColor;
	private Color unusedColor;
	private Color unopenColor;
	private Color checkedColor;
	private Color statusColor;

	void Awake(){
		usedColor = Color.red;
		unusedColor = Color.green;
		unopenColor = Color.gray;
		checkedColor = Color.blue;
	}

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	private void setColorWithStatus(string status){
		switch(status){
		case"A":
			this.transform.GetComponent<SpriteRenderer>().color = usedColor;
			break;
		case"B":
			this.transform.GetComponent<SpriteRenderer>().color = unusedColor;
			break;
		case"C":
			this.transform.GetComponent<SpriteRenderer>().color = unopenColor;
			break;
		default:
			break;
		}
		//Debug.Log("status: " + status + ", color: " + this.transform.GetComponent<SpriteRenderer>().color);
	}

	public string getid(){
		return this.id;
	}

	public void setid(string id){
		this.id = id;
	}

	public string getActivity(){
		return this.activity;
	}

	public void setActivity(string activity){
		this.activity = activity;
	}

	public string getHighLight(){
		return this.highLight;
	}

	public void setHightLight(string highLight){
		this.highLight = highLight;
	}

	public string getLength(){
		return this.length;
	}

	public void setLenght(string length){
		this.length = length;
	}

	public string getType(){
		return this.type;
	}

	public void setType(string type){
		this.type = type;
	}

	public string getStatus(){
		return this.status;
	}

	public void setStatus(string status){
		this.status = status;
		setColorWithStatus(status);
	}

	public string getIsChecked(){
		return this.isChecked;
	}

	public void setChecked(string isChecked){
		this.isChecked = isChecked;
	}

	public string getLable(){
		return this.lable;
	}

	public void setLable(string lable){
		this.lable = lable;
	}
}
