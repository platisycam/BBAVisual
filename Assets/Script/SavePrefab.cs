using UnityEngine;
using System.Collections;
//using UnityEditor;

public class SavePrefab : MonoBehaviour {
	GameObject obj;
	string panel = "panel";
	string prefabName = "prefabname";
	string url = "Assets/Resources/Prefab/";
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	/*
	 * 	public void SaveAsPrefab(){
		obj = GameObject.FindWithTag(panel);
		if(obj){
			Object tempPrefab = PrefabUtility.CreateEmptyPrefab(url + prefabName + ".prefab");
			PrefabUtility.ReplacePrefab(obj, tempPrefab);
		}
	}
	 * 
	 */

}
