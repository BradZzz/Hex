﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChoicePanel : MonoBehaviour {

	public enum minigameType {
		Asteroids, Jump, Pop, Shoot, Town, None
	}

	public GameObject panelParent;
	public GameObject player;
	public GameObject enemy;
	public GameObject shop;
	public GameObject clickable;
	public GameObject cGlossary;

	public minigameType gameType;

	public float gameTimeLimit = 20f;
	private float gameTimer;

	public float enemyGenSpeed = 5f;
	private float enemyGenTimer = 0;

	public float clickableGenSpeed = 5f;
	private float clickableGenTimer = 0;

	private bool started = false;
	private ChoiceGlossary glossy;

	private GameObject thisPlayer;
	private GameObject infoPnl;
	private GameObject[] infoBtns;
	private int windowH;
	private int windowW;

	private OptionInfo[] lstOptions;

	private float eGenSpeed(){
		switch(gameType){
		case ChoicePanel.minigameType.Jump:
			return 2f;
		default:
			return enemyGenSpeed;
		}
	}

	//At the start we need to pull out the player 
	//and attach it to the panel gameobject 
	void Start () {
		infoBtns = new GameObject[6];
		for (int i = 0; i < 6; i++) {
			infoBtns [i] = GameObject.Find ("Button_0" + (i + 1).ToString ());
		}
		infoPnl = GameObject.Find ("InfoPanel");

		glossy = cGlossary.GetComponent<ChoiceGlossary> ();
		populateInfoPanel (glossy);
	}

	public void selectChoice(int btn) {
		ChoiceInfo choice = glossy.choices [0];
		OptionInfo option = lstOptions [btn - 1];
		if (!option.reaction.Equals ("<confirm/>")) {
			GameObject.Find ("InfoDescription").GetComponent<Text> ().text = option.reaction;

			OptionInfo final = new OptionInfo ();
			final.TextOptions = new string[]{ "Continue" };
			final.result = (option.result == OptionInfo.resultType.MiniGame || option.result == OptionInfo.resultType.Battle)
				? option.result : OptionInfo.resultType.None;
			final.reaction = "<confirm/>";

			populateInfoButtons (new OptionInfo[]{ final });
		} else {
			if (option.result == OptionInfo.resultType.MiniGame) {
				infoPnl.SetActive (false);
				start ();
			} else if (option.result == OptionInfo.resultType.Battle) {
				SceneManager.LoadScene ("BattleScene");
			}else {
				SceneManager.LoadScene ("AdventureScene");
			}
		}
	}

	public void start () {
		windowH = (int)panelParent.GetComponent<RectTransform> ().rect.height;
		windowW = (int)panelParent.GetComponent<RectTransform> ().rect.width;

		gameTimer = gameTimeLimit;

		if (gameType != minigameType.Pop) {
			thisPlayer = Instantiate(player);
			Rigidbody2D body = thisPlayer.GetComponent<Rigidbody2D> ();

			switch(gameType) {
			case minigameType.Asteroids:
//				body.gravityScale = .35f;
//				body.mass = .5f;
//				body.angularDrag = .05f;

				body.mass = .002f;
				body.gravityScale = .5f;

				thisPlayer.transform.localPosition = new Vector3(0,windowH/4-50,0);
				break;
			case minigameType.Jump:
				body.mass = .002f;
				body.gravityScale = .5f;

				thisPlayer.transform.localPosition = new Vector3(-windowW/4,windowH/4-50,0);
				break;
			case minigameType.Town:
				this.thisPlayer.GetComponent<Player> ().speed = 20f;
				thisPlayer.transform.localPosition = new Vector3 (0, -windowH / 4 - 50, 0);
				break;
			default:
				thisPlayer.transform.localPosition = new Vector3(0,-windowH/4-50,0);
				break;
			}

			thisPlayer.transform.SetParent(panelParent.transform, false);
			thisPlayer.transform.localScale = new Vector3(200, 200, 1);

			thisPlayer.GetComponent<Player> ().setType (gameType);

			if (gameType == minigameType.Town) {
				for (int i = 0; i < 20; i++) {
					genStore ();
				}
			}
		}

		started = true;
	}

	public void end(bool result) {
		ChoiceInfo choice = glossy.choices [0];

		infoPnl.SetActive (true);
		panelParent.SetActive (false);

		if (result) {
			GameObject.Find ("InfoDescription").GetComponent<Text> ().text = choice.winningGreeting;
		} else {
			GameObject.Find ("InfoDescription").GetComponent<Text> ().text = choice.losingGreeting;
		}

		OptionInfo final = new OptionInfo ();
		final.TextOptions = new string[]{ "Continue on your way" };
		final.result = OptionInfo.resultType.None;
		final.reaction = "<confirm/>";

		populateInfoButtons(new OptionInfo[]{final});
	}

	void populateInfoPanel(ChoiceGlossary glossy){
		ChoiceInfo choice = glossy.choices [0];
		GameObject.Find ("InfoHeader").GetComponent<Text> ().text = choice.name;
		GameObject.Find ("InfoDescription").GetComponent<Text> ().text = choice.openingGreeting;

		populateInfoButtons(choice.options);
	}

	void populateInfoButtons(OptionInfo[] options){
		for (int i = 0; i < 6; i++) {
			if (i < options.Length) {
				infoBtns [i].SetActive (true);
				infoBtns [i].transform.transform.Find("Text").GetComponent<Text> ().text = options[i].TextOptions[0];
			} else {
				infoBtns [i].SetActive (false);
			}
		}

		lstOptions = options;
	}

	void Update()
	{
		if (started) {
			gameTimer -= Time.deltaTime;

			if (minigameType.Shoot == gameType || minigameType.Asteroids == gameType || minigameType.Jump == gameType) {
				enemyGenTimer -= Time.deltaTime;
				if(enemyGenTimer < 0)
				{
					genEnemy (gameType);
					enemyGenTimer = eGenSpeed();
				}
			}

			if (minigameType.Pop == gameType) {
				clickableGenTimer -= Time.deltaTime;
				if(clickableGenTimer < 0)
				{
					for (int i = 0; i < 7; i++) {
						if (i < 2) {
							genClickable (Color.green);
						} else {
							genClickable (Color.red);
						}
					}
					clickableGenTimer = clickableGenSpeed;
				}
			}
		}
	}

	private static Texture2D _staticRectTexture;
	private static GUIStyle _staticRectStyle;

	private static Texture2D _staticHealthTexture;
	private static GUIStyle _staticHealthStyle;

	void OnGUI() {
		if (started && gameType != ChoicePanel.minigameType.Town) {
			if (gameTimer > 0) {
				RectTransform boxRect = panelParent.GetComponent<RectTransform>();
				Vector3 guiPosition = transform.position;
				guiPosition.x -= 4 * boxRect.rect.width / 10;
				guiPosition.y += 5.5f * boxRect.rect.height / 10;

				float bxWidth = 8.94f * boxRect.rect.width / 10;

				Rect bRect = new Rect (guiPosition.x - 18, guiPosition.y - 38, bxWidth + 4, 32);

				if (_staticRectTexture == null) {
					_staticRectTexture = new Texture2D (1, 1);
				}
				if (_staticRectStyle == null) {
					_staticRectStyle = new GUIStyle ();
				}

				_staticRectTexture.SetPixel (0, 0, Color.black);
				_staticRectTexture.Apply ();

				_staticRectStyle.normal.background = _staticRectTexture;

				GUI.Box (bRect, GUIContent.none, _staticRectStyle);

				Rect hRect = new Rect (guiPosition.x - 16, guiPosition.y - 35, 
					bxWidth * (float)gameTimer / (float)gameTimeLimit, 26);

				if (_staticHealthTexture == null) {
					_staticHealthTexture = new Texture2D (1, 1);
				}
				if (_staticHealthStyle == null) {
					_staticHealthStyle = new GUIStyle ();
				}

				_staticHealthTexture.SetPixel (0, 0, Color.yellow);
				_staticHealthTexture.Apply ();

				_staticHealthStyle.normal.background = _staticHealthTexture;

				GUI.Box (hRect, GUIContent.none, _staticHealthStyle);

				Debug.Log (guiPosition.ToString());
			} else {
				end(false);
			}
		}
	}

	void genEnemy(ChoicePanel.minigameType type) {
		float x = Random.Range (-3.5f * windowW / 10, 3.5f * windowW / 10);
		float y = Random.Range (-3.5f * windowH / 10, 3.5f * windowH / 10);

		GameObject thisEnemy = Instantiate(enemy);
		thisEnemy.transform.SetParent(panelParent.transform, false);
		thisEnemy.transform.localPosition = new Vector3(x,windowH/4+50,0);
		thisEnemy.transform.localScale = new Vector3(200, 200, 1);

		if (type == ChoicePanel.minigameType.Asteroids || type == ChoicePanel.minigameType.Jump) {
			float difficulty = -.07f;

			Rigidbody2D body = thisEnemy.GetComponent<Rigidbody2D> ();
			body.mass = 0;
			body.angularDrag = 0;
			body.gravityScale = 0;

			body.AddForce(new Vector2(difficulty,0),ForceMode2D.Force);

			thisEnemy.transform.localPosition = new Vector3(3*windowW/10,y,0);
			thisEnemy.transform.localScale = new Vector3(900, 900, 1);
		}
	}

	void genStore() {
		float x = Random.Range (-windowW / 3, windowW / 3);
		float y = Random.Range (-windowH / 3, windowH / 3);

		GameObject thisShop = Instantiate(shop);
		thisShop.transform.SetParent(panelParent.transform, false);
		thisShop.transform.localPosition = new Vector3(x,y,0);
		thisShop.transform.localScale = new Vector3(200, 200, 1);
	}

	void genClickable(Color meshColor) {
		float x = Random.Range (-windowW / 3, windowW / 3);
		float y = Random.Range (-windowH / 3, windowH / 3);

		GameObject thisClick = Instantiate(clickable);
		thisClick.transform.SetParent(panelParent.transform, false);
		thisClick.transform.localPosition = new Vector3(x,y,0);
		thisClick.transform.localScale = new Vector3(300, 300, 1);

		thisClick.GetComponent<SpriteRenderer> ().color = meshColor;
		if (meshColor == Color.red) {
			thisClick.tag = "Enemy";
		} else {
			thisClick.transform.localScale = new Vector3 (150, 150, 1);
		}
		thisClick.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range (-.1f,.1f),Random.Range (-.1f,.1f)),ForceMode2D.Force);
	}
}
