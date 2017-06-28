using UnityEngine;
using System.Collections;
using LitJson;
using UnityEngine.UI;
using UnityEngine.Sprites;

public class UHeightStatistic : MonoBehaviour {
	GameObject Panel;
	GameObject _Panel;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//弹出U位统计信息板
	public void AddSpaceUsage(){
		if (!GameObject.Find ("UStatistic")) {
			Panel = Resources.Load ("Popprefab/SpaceUsage", typeof(GameObject)) as GameObject;
			_Panel = Instantiate (Panel, transform.position, Quaternion.identity) as GameObject;
			_Panel.name = "UStatistic";
			_Panel.transform.parent = GameObject.Find("Canvas").transform;
			_Panel.transform.position = GameObject.Find ("Canvas").transform.position;
			_Panel.transform.localScale = Vector3.one;
			DisplayStatisticData(_Panel);
		}
	}

	int Uutil;
	int Uutiltoint;
	//显示板上的数据信息
	void DisplayStatisticData(GameObject panel){
		string receivedata = GameObject.Find("SystemObject").GetComponent<SystemSetting>().GetUStatisticsData();
		if(receivedata != null && receivedata != ""){
			JsonData jd = JsonMapper.ToObject (receivedata);
			string Utitle = "";
			string Utotal = "";
			string Uuse = "";
			string Usurplus = "";
			string Uutilization = "";

			if(((IDictionary)jd).Contains("title") && jd["title"] != null){
				Utitle = jd ["title"].ToString ();
				panel.transform.FindChild ("title").GetComponent<Text> ().text = Utitle;
			}
			if(((IDictionary)jd).Contains("total") && jd["total"] != null){
				Utotal = jd ["total"].ToString ();
				panel.transform.FindChild ("total").GetComponent<Text> ().text = Utotal;
			}
			if(((IDictionary)jd).Contains("use") && jd["use"] != null){
				Uuse = jd ["use"].ToString ();
				panel.transform.FindChild ("use").GetComponent<Text> ().text = Uuse;
			}
			if(((IDictionary)jd).Contains("surplus") && jd["surplus"] != null){
				Usurplus = jd ["surplus"].ToString ();
				panel.transform.FindChild ("surplus").GetComponent<Text> ().text = Usurplus;
			}
			if(((IDictionary)jd).Contains("utilization") && jd["utilization"] != null){
				Uutilization = jd ["utilization"].ToString ();
				panel.transform.FindChild ("utilization").GetComponent<Text> ().text = Uutilization;
				Uutil = int.Parse (Uutilization);

				Uutiltoint = Mathf.CeilToInt (Uutil / 2);
				//		print (Uutil + ":::::22222");

				if (Uutil > 0 && Uutil <= 25) {
					ChangeUImage ("green");
				} else if (Uutil > 25 && Uutil <= 50) {
					ChangeUImage ("yellow");
				} else if (Uutil > 50 && Uutil <= 70) {
					ChangeUImage ("orange");
				} else if (Uutil > 70 && Uutil <= 100) {
					ChangeUImage ("red");
				} else {
					return;
				}
			}
		}
	}

	//销毁U位统计板
	public void DestoryStatistic(){
		if (GameObject.Find ("UStatistic")) {
			Destroy (GameObject.Find ("UStatistic"));
		}
	}

	//更换U位统计表上使用率代表的图片比例
	public void ChangeUImage(string image){
		//更换背景图片本身的颜色，但是会跟原始颜色叠加显现
//		Image image;
//		image = GameObject.Find("parents/1").GetComponent<Image> ();
//		image.overrideSprite = Resources.Load ("Image/1yellow", typeof(Sprite)) as Sprite;
//		image.color = Color.red;
		for(int i=1;i<= Uutiltoint;i++){
			GameObject.Find ("parents/"+i).GetComponent<Image> ().sprite = Resources.Load("Image/"+image,typeof(Sprite)) as Sprite;
		}

	}

}
