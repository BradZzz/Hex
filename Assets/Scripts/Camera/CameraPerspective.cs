using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPerspective : MonoBehaviour {

  bool toggle = false;

  void Start() {
    tPer ();
  }

	void Update()
	{
    if(Input.GetKeyUp(KeyCode.P))
		{
      tPer ();
		}
	}

  void tPer(){
    if (!toggle) {
      transform.position = new Vector3 (transform.position.x,55,-70);
      transform.rotation = Quaternion.Euler(20,0,0);
    } else {
      transform.position = new Vector3 (transform.position.x,70,-40);
      transform.rotation = Quaternion.Euler(40,0,0);
    }
    toggle = !toggle;
  }
}
