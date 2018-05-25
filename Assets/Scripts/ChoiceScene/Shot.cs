using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {

	public float speed = .2f;

	void Update () {
		transform.Translate(new Vector3(0,speed,0));
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("Collision : " + collision.gameObject.name);
		Destroy (gameObject);
	}

	//Detect when there is a collision starting
	void OnCollisionExit2D(Collision2D collision)
	{
		//Ouput the Collision to the console
		Debug.Log("Collision : " + collision.gameObject.name);
	}

	//Detect when there is are ongoing Collisions
	void OnCollisionStay2D(Collision2D collision)
	{
		//Output the Collision to the console
		Debug.Log("Stay : " + collision.gameObject.name);
	}
}
