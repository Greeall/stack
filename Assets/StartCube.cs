﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCube : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Renderer> ().material.SetColor ("_Color", CubeColor.CurrentColor ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
