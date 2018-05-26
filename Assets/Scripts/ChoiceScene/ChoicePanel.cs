using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoicePanel : MonoBehaviour {

	public enum minigameType {
		Jump, Pop, Shoot, Town, None
	}

	public GameObject panelParent;
	public GameObject player;
	public GameObject enemy;
	public GameObject shop;
	public GameObject clickable;

	public minigameType gameType;

	public float gameTimeLimit = 20f;
	private float gameTimer;

	public float enemyGenSpeed = 5f;
	private float enemyGenTimer = 0;

	public float clickableGenSpeed = 5f;
	private float clickableGenTimer = 0;

	private GameObject thisPlayer;
	private int windowH;
	private int windowW;

	//At the start we need to pull out the player 
	//and attach it to the panel gameobject 
	void Start () {
		windowH = (int)panelParent.GetComponent<RectTransform> ().rect.height;
		windowW = (int)panelParent.GetComponent<RectTransform> ().rect.width;

		gameTimer = gameTimeLimit;

		if (gameType != minigameType.Pop) {
			switch(gameType) {
			case minigameType.Shoot:
				thisPlayer = Instantiate(player);
				break;
			case minigameType.Town:
				thisPlayer = Instantiate(player);
				break;
			}
			
			thisPlayer.transform.SetParent(panelParent.transform, false);
			thisPlayer.transform.localPosition = new Vector3(0,-windowH/4-50,0);
			thisPlayer.transform.localScale = new Vector3(200, 200, 1);

			thisPlayer.GetComponent<Player> ().setType (gameType);

			if (gameType == minigameType.Town) {
				for (int i = 0; i < 20; i++) {
					genStore ();
				}
			}
		}
	}

	void Update()
	{
		gameTimer -= Time.deltaTime;

		if (minigameType.Shoot == gameType) {
			enemyGenTimer -= Time.deltaTime;
			if(enemyGenTimer < 0)
			{
				genEnemy ();
				enemyGenTimer = enemyGenSpeed;
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

	private static Texture2D _staticRectTexture;
	private static GUIStyle _staticRectStyle;

	private static Texture2D _staticHealthTexture;
	private static GUIStyle _staticHealthStyle;

	void OnGUI() {
		if (gameTimer > 0) {
			RectTransform boxRect = panelParent.GetComponent<RectTransform>();
			Vector3 guiPosition = transform.position;
			guiPosition.x -= 4 * boxRect.rect.width / 10;
			guiPosition.y += 5.5f * boxRect.rect.height / 10;

			float bxWidth = 8.94f * boxRect.rect.width / 10;

			//Black Box Base
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

			//Health Overlay
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
			}
	}

	void genEnemy() {
		float x = Random.Range (-windowH / 3, windowH / 3);

		GameObject thisEnemy = Instantiate(enemy);
		thisEnemy.transform.SetParent(panelParent.transform, false);
		thisEnemy.transform.localPosition = new Vector3(x,windowH/4+50,0);
		thisEnemy.transform.localScale = new Vector3(200, 200, 1);
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
//			thisClick.transform.localScale = new Vector3 (100, 100, 1);
//
//			Vector3 temp = thisClick.GetComponent<RectTransform>().localScale;
//			temp.x = temp.x / 2;
//			temp.y = temp.y / 2;
//			thisClick.GetComponent<RectTransform>().localScale = new Vector3(10f,10f,1);
		} else {
			thisClick.transform.localScale = new Vector3 (150, 150, 1);
		}
		thisClick.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range (-.1f,.1f),Random.Range (-.1f,.1f)),ForceMode2D.Force);

		//thisClick.transform.rotation = Quaternion.Euler(Random.Range (-1, 1), Random.Range (-1, 1), Random.Range (-1, 1));
	}
}
