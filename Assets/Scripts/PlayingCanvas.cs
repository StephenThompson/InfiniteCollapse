using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
Author: Nigel Munro
*/

public class PlayingCanvas : MonoBehaviour {

    public GameObject shipBody;

    public GameObject levelTextGameObject;
    public GameObject scoreTextGameObject;
    public GameObject gemCountTextGameObject;

    private Text levelText;
    private Text scoreText;
    private Text gemCountText;

    public float currentScore = 0;

    public float resetTimer = 0.0f;

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
		if (!shipBody.GetComponent<PlayerController>().dead)
		{
            currentScore += (float)(shipBody.transform.position.z * Mathf.PI * 0.1f);

        } else if (resetTimer > 1) {
            SceneManager.LoadScene(1);
        } else {
            resetTimer += Time.deltaTime;
        }
        scoreText.text = "" + (int)currentScore;
	}

    void FixedUpdate() {
        
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
