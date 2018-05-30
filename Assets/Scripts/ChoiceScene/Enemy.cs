using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private int health = 2;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag.Equals("Wall")) {
			health--;
			if (health == 0) {
				Destroy (gameObject);
			}
		} else if (other.gameObject.tag.Equals("Player")) {
			Destroy (gameObject);
		} else if (other.gameObject.tag.Equals("Projectile")) {
			Destroy (gameObject);
		}
	}

	//Detect when there is a collision starting
	void OnTriggerExit2D(Collider2D other)
	{
		Debug.Log("OnTriggerExit2D: " + other.gameObject.name);
	}

	//Detect when there is are ongoing Collisions
	void OnTriggerStay2D(Collider2D other)
	{
		Debug.Log("OnTriggerStay2D: " + other.gameObject.name);
	}
}
