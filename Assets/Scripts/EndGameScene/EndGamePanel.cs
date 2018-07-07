using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGamePanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
    Debug.Log ("Start");

    // Win / Lose
    GameObject status = GameObject.Find ("Status");
    // A,B,C,etc
    GameObject scoreGrade = GameObject.Find ("ScoreGrade");
    // Siberian Guru, etc
    GameObject scoreRankText = GameObject.Find ("ScoreRankText");
    // 1,2,3,4
    GameObject placeRankText = GameObject.Find ("PlaceRankText");

    GameObject rationScore = GameObject.Find ("RationScore");
    GameObject goldScore = GameObject.Find ("GoldScore");
    GameObject repScore = GameObject.Find ("RepScore");
    GameObject enemyScore = GameObject.Find ("EnemyScore");
    GameObject squadScore = GameObject.Find ("SquadScore");
    GameObject attributeScore = GameObject.Find ("AttributeScore");
    GameObject totalScore = GameObject.Find ("TotalScore");

    GameInfo game = BaseSaver.getGame ();

    HighScoreInfo.EndInfo end = HighScoreInfo.makeEndInfo (game);

    status.GetComponent<Text> ().text = "Victory!";
    scoreGrade.GetComponent<Text> ().text = HighScoreInfo.returnGrade(end.score);
    scoreRankText.GetComponent<Text> ().text = "Title: \n\n" + HighScoreInfo.returnRank(end.score);
    placeRankText.GetComponent<Text> ().text = "Map Rank: 1st";

    rationScore.GetComponent<Text> ().text = "Rations: " + end.getRationsScore() + " (" + end.rations.ToString() + ")";
    goldScore.GetComponent<Text> ().text = "Gold: " + end.getGoldScore() + " (" + end.gold.ToString() + ")";
    repScore.GetComponent<Text> ().text = "Reputation: " + end.getRepScore() + " (" + end.rep.ToString() + ")";
    enemyScore.GetComponent<Text> ().text = "Enemies Killed: " + end.enemies.ToString();
    squadScore.GetComponent<Text> ().text = "Squad Score: " + end.squad.ToString();
    attributeScore.GetComponent<Text> ().text = "Attributes: " + end.attributes.ToString();
    totalScore.GetComponent<Text> ().text = "Total: " + end.score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		//Check for the click here
    if (Input.GetMouseButtonDown (0)) {
      SceneManager.LoadScene ("MainMenuScene");
    }
	}
}
