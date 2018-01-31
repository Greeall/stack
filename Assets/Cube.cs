using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {


	public static float r, g, b;
	public static float rBack, gBack, bBack;

	void Awake () 
	{
		//SetRandomColor ();
	}

	void Update () 
	{
		
	}

	public static void SetRandomColor()
	{
		r = Random.Range (0, 100) / 100f;
		g = Random.Range (0, 100) / 100f;
		b = Random.Range (0, 100) / 100f;
		
		rBack = Random.Range (0, 100) / 100f;
		gBack = Random.Range (0, 100) / 100f;
		bBack = Random.Range (0, 100) / 100f;

		GameObject.Find("CUBE").GetComponent<Renderer>().material.SetColor("_Color", new Color(r, g, b));
		GameObject.Find ("shadow").GetComponent<Renderer> ().material.SetColor ("_Color", new Color (r, g, b));
	}
}
