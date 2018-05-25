using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float speed = 100f;
	public float shotInterval = 1f;
	public GameObject shot;

	private float lastShot = 0;

	void Start () {}

	void Update()
	{
		/*
		 * Check here to make sure the player cannot leave the box
		 */
		lastShot -= Time.deltaTime;

		float mv = speed * Time.deltaTime;
		if(Input.GetKey(KeyCode.RightArrow))
		{
			transform.Translate(new Vector3(mv,0,0));
		}
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			transform.Translate(new Vector3(-mv,0,0));
		}
//		if(Input.GetKey(KeyCode.DownArrow))
//		{
//			transform.Translate(new Vector3(0,-mv,0));
//			//transform.Translate(new Vector3(0,-mv,-mv));
//		}
//		if(Input.GetKey(KeyCode.UpArrow))
//		{
//			transform.Translate(new Vector3(0,mv,0));
//			//transform.Translate(new Vector3(0,mv,mv));
//		}
		if(Input.GetKey(KeyCode.Space))
		{
			if (lastShot <= 0) {
				Debug.Log ("Space");
				GameObject sht = Instantiate(shot);
				Vector3 cPos = transform.position;
				cPos.y += 10;
				sht.transform.position = cPos;

				lastShot = shotInterval;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("Collision : " + collision.gameObject.name);
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
