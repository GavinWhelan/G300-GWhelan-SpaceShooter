using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject hazard;
    public GameObject pickup;
    public GameObject playerExplosion;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public TMP_Text scoreText;
    public TMP_Text restartText;
    public TMP_Text gameOverText;
    public TMP_Text healthText;

    private bool gameOver;
    private bool restart;
    private int score;
    public int health;

    private GameObject playerObject;

    // Sets score to 0 and health to 100, and starts spawning waves
    private void Start()
    {
        gameOver = false;
        restart = false;
        gameOverText.text = "";
        restartText.text = "";
        score = 0;
        UpdateScore();
        health = 100;
        UpdateHealth();
        StartCoroutine(SpawnWaves());


        // Find the Player object
        playerObject = GameObject.FindWithTag("Player");
        if (playerObject == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
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

    public void DamagePlayer(int newHealthValue)
    {
        health -= newHealthValue;
        UpdateHealth();
    }

    public void HealPlayer(int newHealthValue)
    {
        if(health + newHealthValue >= 100)
        {
            health = 100;
        }
        else
        {
            health += newHealthValue;
        }
        UpdateHealth();
    }

    void UpdateHealth()
    {
        healthText.text = "Health: " + health;
        if (health == 0)
        {
            Destroy(playerObject.gameObject);
            Instantiate(playerExplosion, playerObject.transform.position, playerObject.transform.rotation);
            GameOver();
        }
    }
}
