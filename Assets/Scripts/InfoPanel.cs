using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour {

  public GameObject toggleP;
  public GameObject resetP;

  public void togglePanel(bool active){
    toggleP.SetActive (active);
    resetP.SetActive (!active);
  }
	
  public void updatePanel(string name, Sprite img, int health, int actions, int attacks){
    resetP.SetActive (false);

    toggleP.transform.Find ("CharImage").GetComponent<Image> ().sprite = img;
    toggleP.transform.Find ("HeaderTxt").GetComponent<Text> ().text = name;
    toggleP.transform.Find("Health").Find ("HealthText").GetComponent<Text> ().text = health.ToString();
    toggleP.transform.Find("Actions").Find ("ActionText").GetComponent<Text> ().text = actions.ToString();

    toggleP.transform.Find ("Atk").GetComponent<Image> ().enabled = attacks > 0;


  }
}
