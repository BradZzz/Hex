using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoicePanel : MonoBehaviour {

	public float speed = 20;
	public GameObject panelParent;
	public GameObject player;

	private GameObject thisPlayer;

	//At the start we need to pull out the player 
	//and attach it to the panel gameobject 
	void Start () {
		thisPlayer = Instantiate(player);
		thisPlayer.transform.SetParent(panelParent.transform, false);
		thisPlayer.transform.localPosition = new Vector3(0,0,0);
		thisPlayer.transform.localScale = new Vector3(200, 200, 1);
	}

	void Update()
	{
		float mv = speed * Time.deltaTime;
		if(Input.GetKey(KeyCode.RightArrow))
		{
			thisPlayer.transform.Translate(new Vector3(mv,0,0));
		}
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			thisPlayer.transform.Translate(new Vector3(-mv,0,0));
		}
		if(Input.GetKey(KeyCode.DownArrow))
		{
			thisPlayer.transform.Translate(new Vector3(0,-mv,-mv));
		}
		if(Input.GetKey(KeyCode.UpArrow))
		{
			thisPlayer.transform.Translate(new Vector3(0,mv,mv));
		}
	}

	private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
	{
		Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
		Color[] rpixels = result.GetPixels(0);
		float incX = (1.0f / (float)targetWidth);
		float incY = (1.0f / (float)targetHeight);
		for (int px = 0; px < rpixels.Length; px++)
		{
			rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
		}
		result.SetPixels(rpixels, 0);
		result.Apply();
		return result;

	}

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
