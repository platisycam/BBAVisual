using UnityEngine;
using System.Collections;

public class DeviceButtonName : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//ChangeSence 链路按钮名称的显隐
	public void SHLink(bool link){
		if (true.Equals (link)) {
			transform.Find ("Link").gameObject.SetActive (true);
		} else if (false.Equals (link)) {
			transform.Find ("Link").gameObject.SetActive (false);
		}
	}

	//DownDevice 设备下架按钮的显隐
	public void SHDownDevice(bool dd){
		if (true.Equals (dd)) {
			transform.Find ("UnLoadDevice").gameObject.SetActive (true);
		} else if (false.Equals (dd)) {
			transform.Find ("UnLoadDevice").gameObject.SetActive (false);
		}
	}

	//UStatistical U位统计按钮的显隐
	public void SHUStatistical(bool us){
		if (true.Equals (us)) {
			transform.Find ("Statistical").gameObject.SetActive (true);
		} else if (false.Equals (us)) {
			transform.Find ("Statistical").gameObject.SetActive (false);
		}
	}
}
