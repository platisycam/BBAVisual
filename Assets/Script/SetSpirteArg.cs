using UnityEngine;
using System.Collections;

public class SetSpirteArg : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log(this.GetComponent<MeshFilter>().mesh.bounds.size);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
