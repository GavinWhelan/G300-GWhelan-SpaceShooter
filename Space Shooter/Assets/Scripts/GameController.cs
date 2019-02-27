using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject hazard;
    public GameObject pickup;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public TMP_Text scoreText;
    public TMP_Text restartText;
    public TMP_Text gameOverText;

    private bool gameOver;
    private bool restart;
    private int score;

    // Sets score to 0, and begins to start spawning waves
    private void Start()
    {
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
    }

    private void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    // Spawns obstacles in waves, with delays in between waves; also spawns a pickup at the end of the wave
    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true) {
            // Spawn a number of hazards equal to hazardCount
            for (int i = 0; i < hazardCount; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }

            // Spawn a pickup
            Instantiate(pickup, new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z), Quaternion.identity);
            yield return new WaitForSeconds(waveWait);

            // Restart text waits until the end of the current spawn wave, for finality
            if (gameOver)
            {
                restartText.text = "Press 'R' for restart";
                restart = true;
                break;
            }
        }
    }

    // Adds to the game score
    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOverText.text = "Game over!";
        gameOver = true;
    }
}
