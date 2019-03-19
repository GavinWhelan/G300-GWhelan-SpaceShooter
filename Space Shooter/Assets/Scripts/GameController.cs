using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject asteroidHazard;
    public GameObject enemyHazardRandom;
    public GameObject enemyHazardFormation;
    private GameObject hazard;
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
    public TMP_Text torpedoText;

    private bool gameOver;
    private bool restart;
    private int score;
    public int health;

    private GameObject playerObject;
    private GameObject torpedoObject;

    private bool torpedoTutorialStart;
    private bool torpedoTutorialStageOne;
    private bool torpedoTutorial3;

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
        torpedoTutorialStart = false;
        torpedoTutorialStageOne = false;
        torpedoTutorial3 = false;
        UpdateHealth();
        StartCoroutine(SpawnWaves());


        // Find the Player object
        playerObject = GameObject.FindWithTag("Player");
        if (playerObject == null)
        {
            Debug.Log("Cannot find Player object");
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

        torpedoObject = GameObject.FindWithTag("Torpedo");
        if(torpedoTutorialStart == false)
        {
            torpedoText.text = "RIGHT CLICK to FIRE a TORPEDO";
            torpedoTutorialStart = true;
        }
        if (torpedoObject != null && torpedoTutorialStageOne == false)
        {
            torpedoText.text = "RIGHT CLICK AGAIN to TRIGGER the TORPEDO";
            torpedoTutorialStageOne = true;
        }
        if (torpedoObject == null && torpedoTutorialStageOne == true)
        {
            torpedoText.text = "";
        }
    }

    // Spawns obstacles in waves, with delays in between waves; also spawns a pickup at the end of the wave
    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            float waveType = Random.Range(0.0f, 10.0f);
            for (int j = 0; j <= 2; j++)
            {
                if (j != 2)
                {
                    // Spawn a number of hazards equal to hazardCount
                    for (int i = 0; i < hazardCount; i++)
                    {
                        Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                        float typeProbability = Random.Range(0.0f, 10.0f);
                        Quaternion spawnRotation = Quaternion.identity;

                        // Choose what type of hazard to spawn, with the specific probability
                        if (typeProbability <= 8.0f)
                        {
                            hazard = asteroidHazard;
                        }
                        else
                        {
                            hazard = enemyHazardRandom;
                        }

                        // Make that thang happen
                        Instantiate(hazard, spawnPosition, spawnRotation);
                        yield return new WaitForSeconds(spawnWait);
                    }
                    // Spawn a pickup
                    Instantiate(pickup, new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z), Quaternion.identity);
                    yield return new WaitForSeconds(waveWait);
                }
                else
                {
                    // Spawn a formation of enemies
                    float formationFactor = 5.0f;
                    for (int i = 0; i <= 4; i++)
                    {
                        if (i == 0 || i == 4)
                        {
                            SpawnAtPosition(0.0f * formationFactor, enemyHazardFormation);
                        }
                        else if (i == 1 || i == 3)
                        {
                            SpawnAtPosition(0.7071f * formationFactor, enemyHazardFormation);
                            SpawnAtPosition(-0.7071f * formationFactor, enemyHazardFormation);
                        }
                        else
                        {
                            SpawnAtPosition(1.0f * formationFactor, enemyHazardFormation);
                            SpawnAtPosition(-1.0f * formationFactor, enemyHazardFormation);
                        }
                        yield return new WaitForSeconds(spawnWait);

                    }

                    // Spawn a pickup
                    Instantiate(pickup, new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z), Quaternion.identity);
                    yield return new WaitForSeconds(waveWait);
                }

                // Restart text waits until the end of the current spawn wave, for finality
                if (gameOver)
                {
                    restartText.text = "Press 'R' for restart";
                    restart = true;
                    break;
                }
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
        if (health <= 0)
        {
            healthText.text = "Health: 0";
            Destroy(playerObject.gameObject);
            Instantiate(playerExplosion, playerObject.transform.position, playerObject.transform.rotation);
            GameOver();
        }
    }

    // Spawns a game object at a given x position at the top of the screen; used for making formations
    void SpawnAtPosition(float xPosition, GameObject spawnHazard)
    {
        Vector3 spawnPosition = new Vector3(xPosition, spawnValues.y, spawnValues.z);
        Quaternion spawnRotation = Quaternion.identity;
        Instantiate(spawnHazard, spawnPosition, spawnRotation);
    }
}
