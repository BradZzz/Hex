using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {

	public float speed = .2f;

	private MiniGameController controllerPrime;

	public void init (MiniGameController controllerPrime) {
		this.controllerPrime = controllerPrime;
		if (this.controllerPrime == null) {
			Debug.Log ("Controller null in shot");
		} else {
			Debug.Log ("Controller null in shot");
		}
	}

	void Update () {
		transform.Translate(new Vector3(0,speed,0));
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		//Debug.Log("Collision Enter: " + collision.gameObject.name);
		//Debug.Log("Collision Enter: " + collision.gameObject.tag);
		Destroy (gameObject);
//		if (collision.gameObject.tag.Equals("Enemy")) {
//			controllerPrime.addPoints (1);
//			Destroy (gameObject);
//			Debug.Log("Destroy");
//		}
	}

	//Detect when there is a collision starting
	void OnCollisionExit2D(Collision2D collision)
	{
		//Debug.Log("Collision Exit: " + collision.gameObject.name);
	}

	//Detect when there is are ongoing Collisions
	void OnCollisionStay2D(Collision2D collision)
	{
		//Debug.Log("Collision Stay: " + collision.gameObject.name);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//Debug.Log("OnTriggerEnter2D: " + other.gameObject.name);
		//Debug.Log("OnTriggerEnter2D: " + other.gameObject.tag);

		if (other.gameObject.tag.Equals("Enemy")) {
			Debug.Log ("hit enemy!");
			if (controllerPrime) {
				Debug.Log ("valid controller!");
				controllerPrime.addPoints (1);
			} else {
				Debug.Log ("controller empty");
			}
		}
		Destroy (gameObject);
//		Debug.Log("OnTriggerEnter2D: " + other.gameObject.name);
//		Destroy (gameObject);
	}

	//Detect when there is a collision starting
	void OnTriggerExit2D(Collider2D other)
	{
		//Debug.Log("OnTriggerExit2D: " + other.gameObject.name);
	}

	//Detect when there is are ongoing Collisions
	void OnTriggerStay2D(Collider2D other)
	{
		//Debug.Log("OnTriggerStay2D: " + other.gameObject.name);
	}
}
