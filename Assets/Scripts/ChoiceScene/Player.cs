using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private enum actionDir {
		Up, Down, Left, Right, Action
	}

	public float speed = 100f;
	public float shotInterval = 1f;
	public GameObject shot;

	private float lastShot = 0;
	private ChoicePanel.minigameType type = ChoicePanel.minigameType.None;

	public void setType(ChoicePanel.minigameType type) {
		this.type = type;
	}

	private bool canMove(actionDir mv){
		switch(mv) {
		case actionDir.Up:
			switch(type){
			case ChoicePanel.minigameType.Shoot:
				return false;
			case ChoicePanel.minigameType.Asteroids:
				return false;
			default:
				return true;
			}
		case actionDir.Down:
			switch(type){
			case ChoicePanel.minigameType.Shoot:
				return false;
			case ChoicePanel.minigameType.Asteroids:
				return false;
			default:
				return true;
			}
		case actionDir.Left:
			switch(type){
			default:
				return true;
			}
		case actionDir.Right:
			switch(type){
			default:
				return true;
			}
		case actionDir.Action:
			switch(type){
			case ChoicePanel.minigameType.Town:
				return false;
			default:
				return true;
			}
		default:
			return true;
		}
	}

	void Start () {}

	protected void Update()
	{
		/*
		 * Check here to make sure the player cannot leave the box
		 */
		lastShot -= Time.deltaTime;

		float mv = speed * Time.deltaTime;
		if(Input.GetKey(KeyCode.RightArrow) && canMove(actionDir.Right))
		{
			if (type == ChoicePanel.minigameType.Asteroids) {
				GetComponent<Rigidbody2D> ().AddTorque (-speed / 2);
			} else {
				transform.Translate (new Vector3 (mv, 0, 0));
			}
		}
		if(Input.GetKey(KeyCode.LeftArrow) && canMove(actionDir.Left))
		{
			if (type == ChoicePanel.minigameType.Asteroids) {
				GetComponent<Rigidbody2D> ().AddTorque (speed / 2);
			} else { 
				transform.Translate (new Vector3 (-mv, 0, 0));
			}
		}
		if(Input.GetKey(KeyCode.DownArrow) && canMove(actionDir.Down))
		{
			transform.Translate (new Vector3 (0, -mv, 0));
		}
		if(Input.GetKey(KeyCode.UpArrow) && canMove(actionDir.Up))
		{
			transform.Translate (new Vector3 (0, mv, 0));
		}
		if(Input.GetKey(KeyCode.Space) && canMove(actionDir.Action))
		{
			if (type == ChoicePanel.minigameType.Asteroids) {
				Debug.Log ("Space");
				GetComponent<Rigidbody2D>().AddForce(transform.up * (speed / 2), ForceMode2D.Force);
			} else {
				if (lastShot <= 0) {
					GameObject sht = Instantiate(shot);
					Vector3 cPos = transform.position;
					cPos.y += 10;
					sht.transform.position = cPos;

					lastShot = shotInterval;
				}
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
