using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems; // いらない

using UnityEngine.SceneManagement; 



public class ModeSelecter : MonoBehaviour {
	public static int SelectedMode = 0;
	public GameObject eventSystemGameObject;
	public GameObject Buttons;
	public int nowSelect = 0;
	private EventSystem eventSystem;
	public GameObject myController;



	// Use this for initialization
	void Start () {
		Instantiate (myController);
//		eventSystem = eventSystemGameObject.GetComponent<EventSystem> ();
//		eventSystem.SetSelectedGameObject(Buttons.transform.FindChild ("Bitter").gameObject);

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Z) || MyController.Controller1.switch1 || Input.GetKey (KeyCode.B) || MyController.Controller2.switch1) {
			SelectedMode = 0;
			SceneManager.LoadScene ("game");
		} else if (Input.GetKey (KeyCode.X) || MyController.Controller1.switch2 || Input.GetKey (KeyCode.N) || MyController.Controller2.switch2) {
			SelectedMode = 2;
			SceneManager.LoadScene ("game");
		} else if (Input.GetKey (KeyCode.C) || MyController.Controller1.switch3 || Input.GetKey (KeyCode.M) || MyController.Controller2.switch3) {
			SelectedMode = 1;
			SceneManager.LoadScene ("game");
		}
	}
}
