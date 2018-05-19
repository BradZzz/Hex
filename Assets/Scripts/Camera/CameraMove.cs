using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

	public float speed = 1;

	void Update()
	{
		float mv = speed * Time.deltaTime;
		if(Input.GetKey(KeyCode.RightArrow))
		{
			transform.Translate(new Vector3(mv,0,0));
		}
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			transform.Translate(new Vector3(-mv,0,0));
		}
		if(Input.GetKey(KeyCode.DownArrow))
		{
			transform.Translate(new Vector3(0,-mv,-mv));
		}
		if(Input.GetKey(KeyCode.UpArrow))
		{
			transform.Translate(new Vector3(0,mv,mv));
		}
	}
}
