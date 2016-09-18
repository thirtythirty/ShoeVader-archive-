using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public Unit unit;

	public int hp = 1;

	public int point = 100;

	public bool nowSlow = false;
	private Coroutine SlowCorutine;

	// Use this for initialization
	public virtual IEnumerator Start () {
		init ();

		if (unit.canShot == false) {
			yield break;
		}

		Move (unit.speed);

		// 画面内に入っているか判定
		Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));
		Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));
		min.x = min.x + ((max.x*2.0f) / 16.0f) * 3.0f;
		max.x = max.x - ((max.x*2.0f) / 16.0f) * 3.0f;
		while(transform.position.x < min.x || transform.position.x > max.x
			|| transform.position.y < min.y || transform.position.y > max.y){
			yield return new WaitForSeconds(1.0f);
		}

		while (true) {
			ShotBullet ();

			yield return new WaitForSeconds (unit.shotDelay);
		}
	}

	public virtual void init(){
		unit = GetComponent<Unit> ();
	}

	public virtual void ShotBullet(){
//		// 子要素を全て取得する
//		for (int i = 0; i < transform.childCount; i++) {
//
//			Transform shotPosition = transform.GetChild(i);
//
//			// ShotPositionの位置/角度で弾を撃つ
//			unit.Shot (shotPosition);
//		}
		unit.Shot (transform);

	}
		
	public virtual void Move (float speed){
		unit.rb = GetComponent<Rigidbody2D> ();
		unit.rb.velocity = transform.up * speed;
	}

	// Update is called once per frame
	public virtual void Update () {

	}

	public void GetSlow(GameObject StickyCustard){
		if (nowSlow == false) {
			SlowCorutine = StartCoroutine (Slow (StickyCustard));
		} else {
			StopCoroutine (SlowCorutine);
			SlowCorutine = StartCoroutine (Slow (StickyCustard));
		}
	}
	public virtual IEnumerator Slow(GameObject StickyCustard){
		float slowSpeed = unit.speed / 10.0f;
		nowSlow = true;

		Move (slowSpeed);
		while (StickyCustard != null && nowSlow == true) {
			yield return new WaitForEndOfFrame ();
		}
		nowSlow = false;
		Move (unit.speed);
		yield break;
	}


	public virtual void OnTriggerEnter2D(Collider2D c){
		string layerName = LayerMask.LayerToName (c.gameObject.layer);

		if (layerName != "Bullet(Player)" && layerName != "Explosion(Player)") {
			return;
		}

		Transform colliderTransform = c.transform;
		int player_num = 0;

		if (layerName == "Bullet(Player)") {
			Bullet bullet = colliderTransform.GetComponent<PlayerBullet> ();
			player_num = colliderTransform.GetComponent<PlayerBullet> ().player_num;

			hp = hp - bullet.power;

			Destroy (c.gameObject);
		} else if (layerName == "Explosion(Player)") {
			PlayerExplosion explosion = colliderTransform.GetComponent<PlayerExplosion> ();
			player_num = colliderTransform.GetComponent<PlayerExplosion> ().player_num;

			hp = hp - explosion.power;
		}

		if (hp <= 0) {
			FindObjectOfType<Score> ().AddPoint (point, player_num);

			unit.Explosion ();
			Destroy (gameObject);
		} else {
			unit.GetAnimator ().SetTrigger ("Damage");
		}
	}
}