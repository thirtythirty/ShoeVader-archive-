using UnityEngine;
using System.Collections;

public class ContinueMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey (KeyCode.Y)){
			// コンティニュー
			Time.timeScale = 1;
			FindObjectOfType<GameManager> ().Continue();
			Destroy (gameObject);
		} else if (Input.GetKey (KeyCode.N)){
			// スタート画面に戻る
		}
	}
}
