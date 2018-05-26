using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPlayer : Player {

	void Update()
	{
		base.Update ();

		float mv = speed * Time.deltaTime;
		if(Input.GetKey(KeyCode.DownArrow))
		{
			transform.Translate(new Vector3(0,-mv,0));
		}
		if(Input.GetKey(KeyCode.UpArrow))
		{
			transform.Translate(new Vector3(0,mv,0));
		}
	}
}
