using UnityEngine;
using System.Collections;

public class ChooseUseagePercent : MonoBehaviour {
	public static float utilization;
	private int Utilization;
	
	void Start () {
		Utilization = (int)(utilization/2*100);
		setFalse ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
	void setFalse(){
		for(int i = 1;i<=(Utilization+1);i++){
			GameObject use = this.transform.Find(i.ToString()).gameObject;
			use.SetActive (true);
		}
	}

}
