﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Unit : MonoBehaviour {

	public float speed;
	public float shotDelay;
	public GameObject bullet;
	public bool canShot;
	public GameObject explosion;
	public Rigidbody2D rb;

	private Animator animator;

	public void Explosion(){
		Instantiate(explosion, transform.position,
			transform.rotation);
	}

	public void Shot(Transform origin){
		for (int i = 0; i < bullet.transform.childCount; i++) {

			Transform shotPosition = bullet.transform.GetChild(i);
			Instantiate (bullet, origin.position,
				shotPosition.transform.rotation);
		}
	}

	public Animator GetAnimator(){
		return animator;
	}

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
