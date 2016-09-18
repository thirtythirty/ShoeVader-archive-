using UnityEngine;
using System.Collections;

public class AccelerateEnemy : Enemy {
	public Quaternion Forcedirection = Quaternion.Euler(0, 0, 0);
	public float Angle = 0.0f;
	public int ForceSize = 1;

	public override void Update (){
		Forcedirection = Quaternion.Euler (0, 0, Angle);

		unit.rb.AddForce (Forcedirection * Vector2.right * ForceSize);
	}
}
