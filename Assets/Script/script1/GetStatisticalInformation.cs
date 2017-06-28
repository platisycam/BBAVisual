using UnityEngine;
using System.Collections;
using LitJson;
using UnityEngine.UI;
/*
	四大统计数据；
 */
public class GetStatisticalInformation : MonoBehaviour {
	private GameObject uHeightUsingData;
	private GameObject _uHeightUsingData;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//标题名、所有U位、已使用U位
		string s = "{\"titleShowName\":\"\", \"totalCount\":\"\", \"usedCount\":\"\"}";
	}
	//U位统计
	public void UnityAllUHightData(){
		string str = GameObject.Find("SystemObject").GetComponent<SystemSetting>().GetUStatisticsData();
		if(str != null){
			string roomNameUHeight;
			float roomAllUHeight;
			float roomUseUHeight;
			float roomSurplusUHeight;
			float roomUsageUHeight;
			if (str.Equals ("cancel")) 
			{
				if (_uHeightUsingData != null) 
				{
					Destroy(_uHeightUsingData);
				}

			}
			else{
				JsonData jd = JsonMapper.ToObject(str);
				roomNameUHeight = jd ["titleShowName"].ToString ();
				roomAllUHeight = float.Parse (jd["totalCount"].ToString());
				roomUseUHeight = float.Parse (jd["usedCount"].ToString());
				roomSurplusUHeight = roomAllUHeight - roomUseUHeight;
				if (roomUseUHeight  <= 0.0001) 
				{
					roomUsageUHeight=0;	
				}
				else
				{
					roomUsageUHeight=roomUseUHeight/roomAllUHeight;

				}
				if (_uHeightUsingData != null) 
				{
					Destroy(_uHeightUsingData);
				}
				//实例化
				uHeightUsingData=Resources.Load("Popprefab/SpaceUsage",typeof(GameObject)) as GameObject;
				_uHeightUsingData=Instantiate (uHeightUsingData,transform.position,Quaternion.identity)as GameObject;
				_uHeightUsingData.transform.parent=GameObject.Find("Canvas").transform;
				_uHeightUsingData.transform.localScale = new Vector3 (1f, 1f, 1f);
				//_uHeightUsingData.SetActive(true);

				_uHeightUsingData.transform.FindChild ("title").GetComponent<Text> ().text = roomNameUHeight;
				_uHeightUsingData.transform.FindChild ("total").GetComponent<Text> ().text = roomAllUHeight.ToString();
				_uHeightUsingData.transform.FindChild ("use").GetComponent<Text> ().text = roomUseUHeight.ToString();
				_uHeightUsingData.transform.FindChild ("surplus").GetComponent<Text> ().text = roomSurplusUHeight.ToString();
				_uHeightUsingData.transform.FindChild ("utilization").GetComponent<Text> ().text = (roomUsageUHeight*100).ToString("F2");
				//ChooseUseagePercent.utilization = roomUsageUHeight;
			}
		}

	}
}
