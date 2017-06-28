using UnityEngine;
using System.Collections;

public class RowProperty : MonoBehaviour {
	private string rowid;
	private float x;
	private float y;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setRowid(string rowid){
		this.rowid = rowid;
	}

	public string getRowid(){
		return this.rowid;
	}

	public void setX(float x){
		this.x = x;
	}

	public float getX(){
		return this.x;
	}

	public void setY(float y){
		this.y = y;
	}

	public float getY(){
		return this.y;
	}
}
