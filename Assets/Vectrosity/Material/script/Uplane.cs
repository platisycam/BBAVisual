using UnityEngine;
using System.Collections;

public class Uplane : MonoBehaviour {
	private bool b;
	//物体原始大小,颜色
	private Vector3 c;
	private Material box0;
	private Material box1;
	void Start () {
		box0 = Resources.Load ("Texture/Materials/box0", typeof(Material))as Material;
		box1 = Resources.Load ("Texture/Materials/box1", typeof(Material))as Material;
		c = gameObject.GetComponent<Renderer>().transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnMouseOver(){
		
		
		gameObject.GetComponent<Renderer>().transform.localScale=c*2;
		this.GetComponent<Renderer>().material = box1;
		//		print (c);
		b = true;
		
	}
	void OnMouseExit(){
		
		gameObject.GetComponent<Renderer>().transform.localScale=c;
		this .GetComponent<Renderer>().material = box0;
		b = false;
		
	}
	void OnMouseDown(){
		//a = Time.deltaTime;
	}
	void OnMouseUp(){
		//a = 0;
	}
}
