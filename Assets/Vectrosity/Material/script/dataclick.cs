using UnityEngine;
using System.Collections;

public class dataclick : MonoBehaviour {
	AsyncOperation async;
	bool isEnter = false;
	bool isOver =  false;
	bool isBlur = false;
	bool isBeginSingle = false;
	private string str = "";
	
	private bool haveUI=true;
	
	
	int i = 0;
	float first;
	// Use this for initialization
	void Start () {
	
	}
	void setter(bool ui){
		haveUI = ui;
	}
	// Update is called once per frame
	void Update () {

	}
	void OnMouseDown()
	{
		if(haveUI){
			i++;
			if (1 == i) {
				first = Time.time;
				isBeginSingle = true;
			}
			if (2 == i) {
				if (Time.time - first < 0.3f) {
					isBeginSingle = false;
					OnMouseDoubleDown ();
					i = 0;
				} else {
					i--;
					first = Time.time;
					isBeginSingle = true;
				}
				
			}
		}
	}
	void OnMouseDoubleDown(){
		if(gameObject.transform .parent .name.Contains("3")){
			iTween.CameraFadeAdd ();
			iTween.CameraFadeTo (1f, 2f);
			StartCoroutine(loadScene(2));
		}
		if(gameObject.transform .parent.name.Contains("2")){
			iTween.CameraFadeAdd ();
			iTween.CameraFadeTo (1f, 2f);
			StartCoroutine(loadScene(3));
		}
		if(gameObject.transform .parent.name.Contains("1")){
			iTween.CameraFadeAdd ();
			iTween.CameraFadeTo (1f, 2f);
			StartCoroutine(loadScene(4));
		}
	}
	IEnumerator loadScene(int i)
	{
		//		print ("loadf3");
		//异步读取场景。
		//Globe.loadName 就是A场景中需要读取的C场景名称。
		async = Application.LoadLevelAsync(i);
		//		async = Application.LoadLevelAdditiveAsync(1);
		//		print ("in");
		
		//读取完毕后返回， 系统会自动进入C场景
		yield return async;
		
	}
}
