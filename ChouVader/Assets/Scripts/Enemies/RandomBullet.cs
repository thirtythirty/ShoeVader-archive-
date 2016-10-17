using UnityEngine;
using System.Collections;

public class RandomBullet : MonoBehaviour {
	public GameObject bullet;
	public int bulletTotal = 10;
	public int barrageTimes = 5;
	public float waitTime = 1.0f;
	public float shotAngle = 0.0f;

	// Use this for initialization
	void Start () {
		StartCoroutine (ShotRandom ());
	}

	void ShotBulletByAngle(float angle){
		Quaternion rotation = Quaternion.identity;
		rotation.eulerAngles = new Vector3 (0, 0, angle);

		Instantiate(bullet, transform.position, rotation);
	}

	IEnumerator ShotRandom(){
		for (int i = 0; i < barrageTimes; i++) {
			UnityEngine.Random.seed = i;
			for (int j = 0; j < bulletTotal; j++) {
				var r = Random.Range(-1*(shotAngle/2),shotAngle/2);
				float angle = 0.0f+r;
				ShotBulletByAngle (angle);
			}
			yield return new WaitForSeconds (waitTime);
		}

		Destroy (gameObject);
		yield break;
	}
}
