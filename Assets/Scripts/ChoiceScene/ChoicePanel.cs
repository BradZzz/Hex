using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoicePanel : MonoBehaviour {

//	public float speed = 20;
	public GameObject panelParent;
	public GameObject player;
	public GameObject enemy;

	public float enemyGenSpeed = 5f;
	private float enemyGenTimer = 0;

	private GameObject thisPlayer;
	private int windowH;

	//At the start we need to pull out the player 
	//and attach it to the panel gameobject 
	void Start () {
		windowH = (int)panelParent.GetComponent<RectTransform> ().rect.height;

		thisPlayer = Instantiate(player);
		thisPlayer.transform.SetParent(panelParent.transform, false);
		thisPlayer.transform.localPosition = new Vector3(0,-windowH/4-50,0);
		thisPlayer.transform.localScale = new Vector3(200, 200, 1);
	}

	void Update()
	{
		enemyGenTimer -= Time.deltaTime;
		if(enemyGenTimer < 0)
		{
			genEnemy ();
			enemyGenTimer = enemyGenSpeed;
		}

	}

	void genEnemy() {
		float x = Random.Range (-windowH / 3, windowH / 3);

		GameObject thisEnemy = Instantiate(enemy);
		thisEnemy.transform.SetParent(panelParent.transform, false);
		thisEnemy.transform.localPosition = new Vector3(x,windowH/4+50,0);
		thisEnemy.transform.localScale = new Vector3(200, 200, 1);
	}

//	private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
//	{
//		Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
//		Color[] rpixels = result.GetPixels(0);
//		float incX = (1.0f / (float)targetWidth);
//		float incY = (1.0f / (float)targetHeight);
//		for (int px = 0; px < rpixels.Length; px++)
//		{
//			rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
//		}
//		result.SetPixels(rpixels, 0);
//		result.Apply();
//		return result;
//
//	}

//	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
}
