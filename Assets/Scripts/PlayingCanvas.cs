using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayingCanvas : MonoBehaviour {

    public GameObject levelTextGameObject;
    public GameObject scoreTextGameObject;
    public GameObject gemCountTextGameObject;

    private Text levelText;
    private Text scoreText;
    private Text gemCountText;

    // Use this for initialization
    void Start () {
        
        levelText = levelTextGameObject.GetComponent<Text>();
        levelText.text = "Level " + 999999;

        scoreText = scoreTextGameObject.GetComponent<Text>();
        scoreText.text = "" + 888888;

        gemCountText = gemCountTextGameObject.GetComponent<Text>();
        gemCountText.text = "" + 777777;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setLevel(int level)
    {
        levelText.text = "Level " + level;
    }

    public void setScore(int score)
    {
        scoreText.text = "" + score;
    }

    public void setWut(int wut)
    {
        gemCountText.text = "" + wut;
    }
}
