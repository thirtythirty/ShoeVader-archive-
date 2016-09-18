﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;


public class Player : MonoBehaviour {

	public Unit unit;
	public int hp;
	public int sp;
	private int hp_max;
	public int life = 1;
	public int player_num; // 1 or 2

	public GameObject statusField;

	private Renderer renderer_;
	private float BarScaleY;
	private float BarScaleX;
	private float HpBar1_increment;
	private float SpBar1_increment;
	private GameObject canvas;
	private Text Hptext;
	private GameObject HpBar;
	private Text Sptext;
	private GameObject SpBar;
	private GameObject[] lifeIcons = new GameObject[3];

	public SerialHandler serialHandler;
	private bool switch1 = false;
	private bool switch2 = false;
	private bool switch3 = false;
	private float horizontal = 0.0f;
	private float vertical = 0.0f;

	public GameObject SplashBullet;
	private float SplashBulletAngle = 0.0f;
	public float SplashBulletAddAngle = 30.0f;
	public GameObject custardBomb;

	// Use this for initialization
	IEnumerator Start () {

		serialHandler.OnDataReceived += OnDataReceived;

		unit = GetComponent<Unit> ();
		hp_max = hp;
		renderer_ = GetComponent<Renderer>();
		statusField = (GameObject)Instantiate (statusField);
		canvas = statusField.transform.FindChild ("Canvas").gameObject;
		Hptext = canvas.transform.FindChild("HP").gameObject.transform.GetComponentInChildren<Text>();
		HpBar = statusField.transform.FindChild ("HPBar_front").gameObject;
		SpBar = statusField.transform.FindChild ("SPBar_front").gameObject;

		BarScaleX = HpBar.transform.localScale.x;
		BarScaleY = HpBar.transform.localScale.y;
		HpBar1_increment = BarScaleY / hp;
		SpBar1_increment = BarScaleY / sp;
		Sptext = canvas.transform.FindChild("SP").gameObject.transform.GetComponentInChildren<Text>();
		Hptext.text = ""+hp;
		Sptext.text = ""+sp;
		GameObject lifeIconsGameObject = statusField.transform.FindChild ("LifeIcons").gameObject;
		lifeIcons [0] = lifeIconsGameObject.transform.FindChild ("LifeIcon1").gameObject;
		lifeIcons [1] = lifeIconsGameObject.transform.FindChild ("LifeIcon2").gameObject;
		lifeIcons [2] = lifeIconsGameObject.transform.FindChild ("LifeIcon3").gameObject;
		StatusUpdate ();
	
		StartCoroutine ("Damage");

		while (true) {
			if (Input.GetKey (KeyCode.Z) || switch1 == true) {
				unit.Shot (transform);

				yield return new WaitForSeconds (unit.shotDelay);
			} else if (Input.GetKey (KeyCode.X) || switch2 == true) {
				ShotSplashBullet ();
				yield return new WaitForSeconds (unit.shotDelay);
			} else if (Input.GetKey(KeyCode.C) || switch3 == true){
				if (sp >= 10) {
					StartCoroutine ("Damage");
					ShotCustardBomb ();
					sp -= 10;
					StatusUpdate ();
					yield return new WaitForSeconds (unit.shotDelay);
				} else {
					yield return new WaitForEndOfFrame ();
				}
			} else {
				yield return new WaitForEndOfFrame ();
			}
		}
	}

	// Update is called once per frame
	void Update () {
		Vector2 direction = new Vector2 (horizontal, vertical).normalized;

		Move (direction);

		float x = Input.GetAxisRaw ("Horizontal");
		float y = Input.GetAxisRaw ("Vertical");
		Vector2 direction2 = new Vector2 (x, y).normalized;
		Move (direction2);

	}

	void Move(Vector2 direction){
		Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));

		Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));
		min.x = min.x + ((max.x*2.0f) / 16.0f) * 3.0f;
		max.x = max.x - ((max.x*2.0f) / 16.0f) * 3.0f;

		Vector2 pos = transform.position;

		pos += direction * unit.speed * Time.deltaTime;

		pos.x = Mathf.Clamp (pos.x, min.x, max.x);
		pos.y = Mathf.Clamp (pos.y, min.y, max.y);

		transform.position = pos;

	}

	void StatusUpdate(){
		Hptext.text = ""+hp;
		Sptext.text = "" + sp;
		HpBar.transform.localScale = new Vector3(BarScaleX, HpBar1_increment*hp, 1);
		SpBar.transform.localScale = new Vector3(BarScaleX, SpBar1_increment*sp, 1);

		for (int i = 0; i < 3; i++) {
			GameObject lifeIcon = lifeIcons [i];
			if (i <= life-1) {
				lifeIcon.GetComponent<Renderer> ().material.color = new Color (1, 1, 1, 1);
			} else {
				lifeIcon.GetComponent<Renderer> ().material.color = new Color (1, 1, 1, 0);
			}
		}
	}

	void ShotSplashBullet(){
		Quaternion rotation = Quaternion.identity;
		rotation.eulerAngles = new Vector3 (0, 0, SplashBulletAngle);
		for (int i = 0; i < SplashBullet.transform.childCount; i++) {
			Transform shotPosition = SplashBullet.transform.GetChild(i);

			Instantiate (SplashBullet, transform.position,
				shotPosition.transform.rotation * rotation);
		}
		SplashBulletAngle += SplashBulletAddAngle;
		if (SplashBulletAngle > 360.0f) {
			SplashBulletAngle -= 360.0f;
		}
	}

	void ShotCustardBomb(){
		Instantiate (custardBomb, transform.position,transform.rotation);
	}

	public void AddSp(int addSp){
		if (BarScaleY < SpBar1_increment * (sp + addSp)) {
			return;
		}
		sp+=addSp;
		StatusUpdate();
	}

	void OnTriggerEnter2D (Collider2D c){
		if (LayerMask.LayerToName (gameObject.layer) == "PlayerDamage") {
			return;
		}

		string layerName = LayerMask.LayerToName (c.gameObject.layer);

		if (layerName == "Bullet(Enemy)") {
			Destroy (c.gameObject);
		}

		if (layerName == "Bullet(Enemy)" || layerName == "Enemy"  || layerName == "Boss") {
			hp -= 1;
//			unit.GetAnimator ().SetTrigger ("Invincible");
			StartCoroutine ("Damage");

			StatusUpdate ();
		}

		if (hp <= 0) {
			unit.Explosion ();
			FindObjectOfType<GameManager> ().GameOver (player_num);

			Destroy (statusField);
			Destroy (gameObject);

//			hp = hp_max;
//			HpBar.transform.localScale = new Vector3(HpBar.transform.localScale.x, BarScaleY, HpBar.transform.localScale.z);
//			StatusUpdate ();
		}
	}

	IEnumerator Damage (){
		gameObject.layer = LayerMask.NameToLayer("PlayerDamage");

		int count = 30;
		while (count > 0){
			//透明にする
			renderer_.material.color = new Color (1,1,1,0);
			//0.05秒待つ
			yield return new WaitForSeconds(0.05f);
			//元に戻す
			renderer_.material.color = new Color (1,1,1,1);
			//0.05秒待つ
			yield return new WaitForSeconds(0.05f);
			count--;
		}
		//レイヤーをPlayerに戻す
		gameObject.layer = LayerMask.NameToLayer("Player");
	}

	void OnDataReceived(string message){
		string[] data = message.Split(
			new string[]{"\t"}, System.StringSplitOptions.None);
		if (data.Length < 5) return;

		try {
			switch1 = data[0] == "1" ? true : false;
			switch2 = data[1] == "1" ? true : false;
			switch3 = data[2] == "1" ? true : false;
			horizontal = float.Parse(data[3]);
			if(horizontal > -100 && horizontal < 100){
				horizontal = 0;
			}
			vertical = float.Parse(data[4]);
			if(vertical > -100 && vertical < 100){
				vertical = 0;
			}

		} catch (System.Exception e) {
			Debug.LogWarning(e.Message);
		}
	}
}
