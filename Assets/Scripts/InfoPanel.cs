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
	
  public void updatePanel(string name, Sprite img, int health, int actions, int attacks, bool isPlayer){
    resetP.SetActive (false);

    toggleP.transform.Find ("CharImage").GetComponent<Image> ().sprite = img;
    toggleP.transform.Find ("HeaderTxt").GetComponent<Text> ().text = name;
    toggleP.transform.Find("Health").Find ("HealthText").GetComponent<Text> ().text = health.ToString();
    toggleP.transform.Find("Actions").Find ("ActionText").GetComponent<Text> ().text = actions.ToString();

    toggleP.transform.Find ("Atk").GetComponent<Image> ().enabled = attacks > 0;

    if (isPlayer) {
      toggleP.transform.parent.gameObject.GetComponent<Image> ().color = new Color (1, 1, 1, .4f);
    } else {
      toggleP.transform.parent.gameObject.GetComponent<Image> ().color = new Color (1, .8f, .8f, .8f);
    }
  }
}
