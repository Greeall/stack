using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class account : MonoBehaviour {

	Text count;

	// Use this for initialization
	void Start () 
	{
		count = gameObject.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		count.text = "" + Falling_cube.count;
	}
}
