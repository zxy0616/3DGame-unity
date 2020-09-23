using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstTry : MonoBehaviour {

	// Use this for initialization
	private int player1, player2;
	private int r_x, r_y;
	private int turn;
	private int[ , ] state = new int[3,3];
	private int turn_flag;
	// Use this for initialization
	void reset()
	{
		turn_flag = 0;
		turn = 1;
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				state [i, j] = 0;
			}
		}
	}
	void Start () {
		reset ();
	}
	int check()
	{
		for (int i = 0; i < 3; i++) {
			if (state [i, 0] != 0 && state [i, 0] == state [i, 1] && state [i, 1] == state [i, 2])
				return state [i, 0];
		}
		for (int i = 0; i < 3; i++) {
			if (state [0, i] != 0 && state [0, i] == state [1, i] && state [1, i] == state [2, i])
				return state [0, i];
		}
		int flag = state [0, 0];
		int flag2 = state [0, 2];
		for (int i = 0; i < 3; i++) {
			if (state [i, i] != flag && state [i, 2 - i] != flag2)
				return 0;
		}
		if (flag == state [2, 2])
			return flag;
		else
			return flag2;
	}
	void robotWin(){
		//cross line to win
		if(state[1,1] == player2){
			if (state [0, 0] == player2 && state [2, 2] == 0) {
				r_x = 2;
				r_y = 2;
				return;
			}
			if (state [2, 2] == player2 && state [0, 0] == 0) {
				r_x = 0;
				r_y = 0;
				return;
			}
			if (state [2, 0] == player2 && state [0, 2] == 0) {
				r_x = 0;
				r_y = 2;
				return;
			}
			if (state [0, 2] == player2 && state [2, 0] == 0) {
				r_x = 2;
				r_y = 0;
				return;
			}
		}
		for (int i = 0; i < 3; i++) {
			int row = i;
			int col = i;
			//row to win
			for(int j = 1; j < 3; j++) {
				if (state [row, j] == player2) {
					if (state [row, (j + 1) % 3] == player2 && state [row, (j - 1) % 3] == 0) {
						r_x = row;
						r_y = (j - 1) % 3;
						return;
					}
					if (state [row, (j + 1) % 3] == 0 && state [row, (j - 1) % 3] == player2) {
						r_x = row;
						r_y = (j + 1) % 3;
						return;
					}
				}
			}
			//column to win
			for(int j = 1; j < 3; j++) {
				if (state [j, col] == player2) {
					if (state [(j + 1) % 3, col] == player2 && state [(j - 1) % 3, col] == 0) {
						r_y = col;
						r_x = (j - 1) % 3;
						return;
					}
					if (state [(j + 1) % 3, col] == 0 && state [(j - 1) % 3, col] == player2) {
						r_y = col;
						r_x = (j + 1) % 3;
						return;
					}
				}
			}
		}
	}
	void playerWin(){
		if(state[1,1] == player1){
			if (state [0, 0] == player1 && state [2, 2] == 0) {
				r_x = 2;
				r_y = 2;
				return;
			}
			if (state [2, 2] == player1 && state [0, 0] == 0) {
				r_x = 0;
				r_y = 0;
				return;
			}
			if (state [2, 0] == player1 && state [0, 2] == 0) {
				r_x = 0;
				r_y = 2;
				return;
			}
			if (state [0, 2] == player1 && state [2, 0] == 0) {
				r_x = 2;
				r_y = 0;
				return;
			}
		}
		for (int i = 0; i < 3; i++) {
			int row = i;
			int col = i;
			//row to win
			for(int j = 1; j < 3; j++) {
				if (state [row, j] == player1) {
					if (state [row, (j + 1) % 3] == player1 && state [row, (j - 1) % 3] == 0) {
						r_x = row;
						r_y = (j - 1) % 3;
						return;
					}
					if (state [row, (j + 1) % 3] == 0 && state [row, (j - 1) % 3] == player1) {
						r_x = row;
						r_y = (j + 1) % 3;
						return;
					}
				}
			}
			//column to win
			for(int j = 1; j < 3; j++) {
				if (state [j, col] == player1) {
					if (state [(j + 1) % 3, col] == player1 && state [(j - 1) % 3, col] == 0) {
						r_y = col;
						r_x = (j - 1) % 3;
						return;
					}
					if (state [(j + 1) % 3, col] == 0 && state [(j - 1) % 3, col] == player1) {
						r_y = col;
						r_x = (j + 1) % 3;
						return;
					}
				}
			}
		}
	}
	void randomStep(){
		List<int> row = new List<int> ();
		List<int> col = new List<int> ();
		int count = 0;
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				if (state [i, j] == 0) {
					row.Add (i);
					col.Add (j);
					count++;
				}
			}
		}
		if (count == 0) {
			r_x = r_y = -1;
			return;
		}
		System.Random ran = new System.Random ();
		int index = ran.Next (0, count);
		r_x = row [index];
		r_y = col [index];
		return;
	}
	void OnGUI()
	{
		if (GUI.Button (new Rect (500, 100, 80, 50), "后手")) {
			if (turn_flag == 0)
				turn = -1;
			turn_flag = 1;
		}
		GUI.Box(new Rect(596, 100, 160, 350), "");
		GUIStyle style = new GUIStyle ();
		style.normal.textColor = new Color (46f / 256f, 163f / 256f, 256f / 256f);
		style.fontSize = 24;
		if (GUI.Button (new Rect (625, 380, 100, 50), "reset")) {
			reset ();
		}
		int result = check ();
		if (result == 1) {
			GUI.Label (new Rect (630, 300, 100, 50), "O win!!!", style);
		} else if (result == 2) {
			GUI.Label (new Rect (630, 300, 100, 50), "X win!!!", style);
		}
		int flag = 0;
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				if (state [i, j] == 0)
					flag = 1;
			}
		}
		if (flag == 0) {
			GUI.Label (new Rect (650, 300, 100, 50), "平局", style);
		}
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				if (state [i, j] == 1) {
					GUI.Button (new Rect (i * 50+600, j * 50+100, 50, 50), "O");
				}
				if (state [i, j] == 2) {
					GUI.Button (new Rect (i * 50+600, j * 50+100, 50, 50), "X");
				}
				if (result != 0)
					return;
					if (turn == 1) {
						if (GUI.Button (new Rect (i * 50 + 600, j * 50 + 100, 50, 50), "")) {
							if (turn_flag == 1) {
								player1 = 1;
								player2 = 2;
							} else {
								player1 = 2;
								player2 = 1;
							}
							state [i, j] = player1;
							turn = -turn;
						}
					} else {
						if (turn_flag == 1) {
							player1 = 1;
							player2 = 2;
						} else {
							player1 = 2;
							player2 = 1;
						}
						GUI.Button (new Rect (i * 50 + 600, j * 50 + 100, 50, 50), "");
						r_x = r_y = -1;
						robotWin ();
						if (r_x == -1 && r_y == -1) {
							playerWin ();
						}
						if (r_x == -1 && r_y == -1) {
							randomStep ();
						}
						if (r_x != -1 && r_y != -1 && state [r_x, r_y] == 0) {
							state [r_x, r_y] = player2;
						}
						turn = -turn;
					}
			}
		}
	}

}
