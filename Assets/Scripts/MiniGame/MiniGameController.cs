﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameController : MonoBehaviour {

	public enum minigameType {
		Asteroids, Jump, Pop, Shoot, Town, None
	}

	public GameObject panelParent;
	public GameObject player;
	public GameObject enemy;
	public GameObject shop;
	public GameObject clickable;

	public minigameType gameType;

	public float gameTimeLimit = 20f;
	private float gameTimer;

	public float enemyGenSpeed = 2f;
	private float enemyGenTimer = 0;

	public float clickableGenSpeed = 2f;
	private float clickableGenTimer = 0;

	private bool started = false;
	private ChoiceGlossary glossy;

	private GameObject thisPlayer;
	private GameObject infoPnl;
	private GameObject[] infoBtns;
	private int windowH;
	private int windowW;

	private float eGenSpeed(){
		switch(gameType){
		case MiniGameController.minigameType.Jump:
			return 2f;
		default:
			return enemyGenSpeed;
		}
	}

	// Use this for initialization
	void Start () {
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
	
	private static Texture2D _staticRectTexture;
	private static GUIStyle _staticRectStyle;

	private static Texture2D _staticHealthTexture;
	private static GUIStyle _staticHealthStyle;

	void OnGUI() {
		if (started && gameType != MiniGameController.minigameType.Town) {
			if (gameTimer > 0) {
				RectTransform boxRect = panelParent.GetComponent<RectTransform>();
				Vector3 guiPosition = transform.position;
				guiPosition.x = 0.5f * boxRect.rect.width / 10;
				guiPosition.y += 5.8f * boxRect.rect.height / 10;

				float bxWidth = 9f * boxRect.rect.width / 10;

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

	void Update()
	{
		if (started) {
			gameTimer -= Time.deltaTime;

			if (minigameType.Shoot == gameType || minigameType.Asteroids == gameType || minigameType.Jump == gameType) {
				enemyGenTimer -= Time.deltaTime;
				if(enemyGenTimer < 0)
				{
					Debug.Log ("Gen Speed: " + eGenSpeed().ToString());
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

	public void end(bool result) {
		//SceneManager.LoadScene ("ChoiceScene");
	}

	void genStore() {
		float x = Random.Range (-windowW / 3, windowW / 3);
		float y = Random.Range (-windowH / 3, windowH / 3);

		GameObject thisShop = Instantiate(shop);
		thisShop.transform.SetParent(panelParent.transform, false);
		thisShop.transform.localPosition = new Vector3(x,y,0);
		thisShop.transform.localScale = new Vector3(200, 200, 1);
	}

	void genEnemy(MiniGameController.minigameType type) {
		float x = Random.Range (-3.5f * windowW / 10, 3.5f * windowW / 10);
		float y = Random.Range (-3.5f * windowH / 10, 3.5f * windowH / 10);

		GameObject thisEnemy = Instantiate(enemy);
		thisEnemy.transform.SetParent(panelParent.transform, false);
		thisEnemy.transform.localPosition = new Vector3(x,6*windowH/10,0);
		thisEnemy.transform.localScale = new Vector3(200, 200, 1);

		if (type == MiniGameController.minigameType.Asteroids || type == MiniGameController.minigameType.Jump) {
			float difficulty = -.14f;

			Rigidbody2D body = thisEnemy.GetComponent<Rigidbody2D> ();
			body.mass = 0;
			body.angularDrag = 0;
			body.gravityScale = 0;

			body.AddForce(new Vector2(difficulty,0),ForceMode2D.Force);

			thisEnemy.transform.localPosition = new Vector3(6*windowW/10,y,0);
			thisEnemy.transform.localScale = new Vector3(900, 900, 1);
		}
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
