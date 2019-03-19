using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;
    private GameController gameController;
    public int damageValue = 50;

    // Find GameController by tag
    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if(gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if(gameControllerObject == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    // Destroys obstacle and anything that came into contact with it
    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Pickup")
        {
            if (other.tag == "Boundary")
            {
                return;
            }
            if (this.tag != "EnemyBolt")
            {
                Instantiate(explosion, transform.position, transform.rotation);
                if (other.tag == "Enemy")
                {
                    return;
                }
            }
            if (other.tag == "Player")
            {
                // Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                gameController.DamagePlayer(damageValue);
            }
            else
            {
                Destroy(other.gameObject);
            }
            gameController.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }
}
