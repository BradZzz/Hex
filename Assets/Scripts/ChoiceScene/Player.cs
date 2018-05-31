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
	private MiniGameController.minigameType type = MiniGameController.minigameType.None;
	private MiniGameController controllerPrime;

	public void init(MiniGameController controllerPrime, MiniGameController.minigameType type) {
		this.controllerPrime = controllerPrime;
		setType (type);
	}

	public void setType(MiniGameController.minigameType type) {
		this.type = type;
	}

	private bool canMove(actionDir mv){
		switch(mv) {
		case actionDir.Up:
			switch(type){
			case MiniGameController.minigameType.Jump:
				return false;
			case MiniGameController.minigameType.Shoot:
				return false;
			case MiniGameController.minigameType.Asteroids:
				return false;
			default:
				return true;
			}
		case actionDir.Down:
			switch(type){
			case MiniGameController.minigameType.Jump:
				return false;
			case MiniGameController.minigameType.Shoot:
				return false;
			case MiniGameController.minigameType.Asteroids:
				return false;
			default:
				return true;
			}
		case actionDir.Left:
			switch(type){
			case MiniGameController.minigameType.Jump:
				return false;
			default:
				return true;
			}
		case actionDir.Right:
			switch(type){
			case MiniGameController.minigameType.Jump:
				return false;
			default:
				return true;
			}
		case actionDir.Action:
			switch(type){
			case MiniGameController.minigameType.Town:
				return false;
			default:
				return true;
			}
		default:
			return true;
		}
	}

	void Start () {
		if (type == MiniGameController.minigameType.Shoot) {
			speed *= 2f; 
		}
	}

	protected void Update()
	{
		/*
		 * Check here to make sure the player cannot leave the box
		 */
		lastShot -= Time.deltaTime;

		float mv = speed * Time.deltaTime;
		if(Input.GetKey(KeyCode.RightArrow) && canMove(actionDir.Right))
		{
			if (type == MiniGameController.minigameType.Asteroids) {
				GetComponent<Rigidbody2D> ().AddTorque (-.01f);
			} else {
				transform.Translate (new Vector3 (mv, 0, 0));
			}
		}
		if(Input.GetKey(KeyCode.LeftArrow) && canMove(actionDir.Left))
		{
			if (type == MiniGameController.minigameType.Asteroids) {
				GetComponent<Rigidbody2D> ().AddTorque (.01f);
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
			if (type == MiniGameController.minigameType.Asteroids || type == MiniGameController.minigameType.Jump) {
//				switch(type){
//				case ChoicePanel.minigameType.Jump:
//					GetComponent<Rigidbody2D> ().AddForce (transform.up * .02f, ForceMode2D.Force);
//					break;
//				case ChoicePanel.minigameType.Asteroids:
//					GetComponent<Rigidbody2D> ().AddForce (transform.up * (speed / 2), ForceMode2D.Force);
//					break;
//				}
				GetComponent<Rigidbody2D> ().AddForce (transform.up * .02f, ForceMode2D.Force);
			} else {
				if (lastShot <= 0) {
					GameObject sht = Instantiate(shot);
					sht.GetComponent<Shot> ().init (controllerPrime);
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
