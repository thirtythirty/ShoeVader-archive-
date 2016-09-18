using UnityEngine;
using System.Collections;

public class PlayerBullet : Bullet {
	public int player_num;
	public bool extraBulletCanShot;
	public GameObject extraBullet;

		
	void OnTriggerEnter2D(Collider2D c){
		string layerName = LayerMask.LayerToName (c.gameObject.layer);
		Debug.Log (layerName);
		if (layerName == "Bullet(Player)") {
			if (extraBulletCanShot == true) {
				if (player_num != c.gameObject.GetComponent<PlayerBullet> ().player_num) {
					Instantiate (extraBullet, transform.position, transform.rotation);

					extraBulletCanShot = false;
				}
			}
		}
	}
}
