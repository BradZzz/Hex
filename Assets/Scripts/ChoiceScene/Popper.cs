using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popper : MonoBehaviour {

	public float speed = .2f;
	public int health = 3;

//	void Update () {
//		transform.Translate(new Vector3(0,speed,0));
//	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("Collision : " + collision.gameObject.name);
		Debug.Log ("Tag: " + collision.gameObject.tag);
		if (collision.gameObject.tag.Equals("Wall")) {
			//Vector2 forceVec = -GetComponent<Rigidbody2D>().velocity.normalized * 20f;
			GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range (-.1f,.1f),Random.Range (-.1f,.1f)),ForceMode2D.Force);
		} else if (collision.gameObject.tag.Equals("Enemy") && !gameObject.tag.Equals("Enemy")) {
			health--;
			if (health <= 0) {
				Destroy (gameObject);
			}
		}
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

	void OnMouseDown(){
		if (gameObject.tag.Equals("Enemy")) {
			Destroy (this.gameObject);
		}
	}   
}
