using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; 


public class GameManager : MonoBehaviour {
	public GameObject player1;
	public GameObject player2;
	public GameObject player1_instance;
	public GameObject player2_instance;
	public GameObject myController;

	public GameObject continue_menu;
	public GameObject[] Stages;
	public int nowStageNum = 0;
	private GameObject nowStage;

	public int active_player_num = 2; // 1 or 2
	public int continue_count = 0;

	// Use this for initialization
	void Start () {
		Instantiate (myController);
		createStage (nowStageNum);
		createPlayer ();
	}

	public void GameOver(int player_num){
		if (player_num == 1) {
			Destroy (player1_instance);
			player1_instance = null;
		} else if(player_num == 2){
			Destroy (player2_instance);
			player2_instance = null;
		}

		if (player1_instance == null && player2_instance == null) {
			if (continue_count > 3) {
				Instantiate (continue_menu);
			} else {
				Instantiate (continue_menu);
			}
		}
	}

	private void createPlayer(){
		if (active_player_num == 1) {
			player1_instance = (GameObject)Instantiate (player1);
			player2_instance = null;
		} else if (active_player_num == 2) {
			player1_instance = (GameObject)Instantiate (player1);
			player2_instance = (GameObject)Instantiate (player2);
		}

		player1_instance.name = "Player1";
		player2_instance.name = "Player2";
	}

	private void createStage (int stage_num){
		nowStage = Instantiate (Stages [stage_num]);
	}

	public void Continue(){
		createPlayer();
		continue_count += 1;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeStage(){
		nowStageNum += 1;
		if (nowStageNum < Stages.Length) {
			Destroy (nowStage);
			createStage (nowStageNum);
		} else {
			SceneManager.LoadScene ("result");
		}
	}
}
