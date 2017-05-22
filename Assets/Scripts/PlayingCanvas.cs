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

    public GameObject pausedCanvas;
    public GameObject playingCanvas;

    public GameObject deathCanvas;

    private Text levelText;
    private Text scoreText;
    private Text gemCountText;

    private Text finalScoreText;

    public float currentScore = 0;

    //public float resetTimer = 0.0f;

    public int gameState;
    public int level = 0;
    public int score = 0;

    Vector3 lastPosition;

    // Use this for initialization
    void Start () {
        Resume();

        levelText = levelTextGameObject.GetComponent<Text>();
        levelText.text = "Level " + 999999;

        scoreText = scoreTextGameObject.GetComponent<Text>();
        scoreText.text = "" + 888888;

        gemCountText = gemCountTextGameObject.GetComponent<Text>();
        gemCountText.text = "" + 777777;

        lastPosition = shipBody.transform.position;

    }
	
	// Update is called once per frame
	void Update () {
        // We are playing and open the pause menu
        if (Input.GetButtonDown("Cancel") && gameState == (int)GameStateManager.States.PLAYING)
        {
            Pause();
            
            //if (Application.isEditor) {
            //}

        } else
        // Pause to Playing
        if (Input.GetButtonDown("Cancel") && gameState == (int)GameStateManager.States.PAUSED)
        {
            Resume();
        }

        // Escape on death screen goes to main menu
        if (Input.GetButtonDown("Cancel") && gameState == (int)GameStateManager.States.DEAD)
        {
            SceneManager.LoadScene(0);
        }

        // Space on death screen restarts game
        if (Input.GetButtonDown("Jump") && gameState == (int)GameStateManager.States.DEAD)
        {
            SceneManager.LoadScene(1);
        } else
        {
            scoreText.text = "" + (int)currentScore;
        }

        // Player dies while playing
        if (shipBody.GetComponent<PlayerController>().dead && gameState == (int)GameStateManager.States.PLAYING)
        {
            gameState = (int)GameStateManager.States.DEAD;
            playingCanvas.SetActive(false);
            deathCanvas.SetActive(true);
            ScoreStorer.AddScore((int)currentScore);
        }

        // Accumulate score while player is alive
        if (!shipBody.GetComponent<PlayerController>().dead)
		{
            currentScore += ((shipBody.transform.position - lastPosition).magnitude);
            lastPosition = shipBody.transform.position;

        }
        
	}

    public void Pause()
    {
        
        gameState = (int)GameStateManager.States.PAUSED;
        Time.timeScale = 0.0f;
        pausedCanvas.SetActive(true);
    }

    public void Resume()
    {
        pausedCanvas.SetActive(false);
        gameState = (int)GameStateManager.States.PLAYING;
        Time.timeScale = 1.0f;
    }

    public void QuitToMenu()
    {
        Debug.Log("Hi");
        SceneManager.LoadScene(0);
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
