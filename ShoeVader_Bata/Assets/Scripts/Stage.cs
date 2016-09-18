﻿using UnityEngine;
using System.Collections;

public class Stage : MonoBehaviour {
	public GameObject emitter;
	public GameObject background;
	public GameObject boss;

	// Use this for initialization
	void Start () {
		emitter = (GameObject)Instantiate(emitter);
		background = (GameObject)Instantiate(background);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CallBoss(){
		boss = (GameObject)Instantiate(boss);
	}
}
