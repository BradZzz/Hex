using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoicePanel : MonoBehaviour {

	public enum minigameType {
		Shoot, Town, None
	}

	public GameObject panelParent;
	public GameObject player;
	public GameObject navPlayer;
	public GameObject enemy;
	public GameObject shop;

	public minigameType gameType;

	public float enemyGenSpeed = 5f;
	private float enemyGenTimer = 0;

	private GameObject thisPlayer;
	private int windowH;
	private int windowW;

	//At the start we need to pull out the player 
	//and attach it to the panel gameobject 
	void Start () {
		windowH = (int)panelParent.GetComponent<RectTransform> ().rect.height;
		windowW = (int)panelParent.GetComponent<RectTransform> ().rect.width;

		if (gameType == minigameType.Shoot) {
			thisPlayer = Instantiate(player);
		} else if (gameType == minigameType.Town) {
			thisPlayer = Instantiate(navPlayer);
		}
		thisPlayer.transform.SetParent(panelParent.transform, false);
		thisPlayer.transform.localPosition = new Vector3(0,-windowH/4-50,0);
		thisPlayer.transform.localScale = new Vector3(200, 200, 1);

		if (gameType == minigameType.Town) {
			for (int i = 0; i < 20; i++) {
				genStore ();
			}
		}
	}

	void Update()
	{
		if (minigameType.Shoot == gameType) {
			enemyGenTimer -= Time.deltaTime;
			if(enemyGenTimer < 0)
			{
				genEnemy ();
				enemyGenTimer = enemyGenSpeed;
			}
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
}
