using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
Author: Nigel Munro
*/
public class GameStateManager : MonoBehaviour {

    public enum States
    {
        MAIN_MENU,
        PLAYING,
        PAUSED,
        SCORES,
        DEAD
    }

    public int gameState;
    public GameObject mainMenuCanvas;
    public GameObject scoresCanvas;


    void Start () {
        gameState = (int)States.MAIN_MENU;
    }
	
	
	void Update () {

        if (Input.GetButtonDown("Cancel") && gameState == (int)GameStateManager.States.SCORES)
        {
            ScoresToMenu();
        }

    }

    public void Play()
    {
        if (Application.isEditor)
        {
            SceneManager.LoadScene(1);
        } else
        {
            SceneManager.LoadScene(1);
        }
        /*
        mainMenuCanvas.SetActive(false);
        playingCanvas.SetActive(true);
        gameState = (int)States.PLAYING;
        Time.timeScale = 1.0f;
        */
    }


    public void Scores()
    {
        mainMenuCanvas.SetActive(false);
        scoresCanvas.SetActive(true);
        gameState = (int)States.SCORES;
        GetComponent<ScoreCanvas>().DisplayScores();
    }

    public void ScoresToMenu()
    {
        scoresCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
        gameState = (int)States.MAIN_MENU;
    }
}
