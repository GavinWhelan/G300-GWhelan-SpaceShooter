using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
};

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Boundary boundary;
    public float tilt;

    private GameObject torpedoObject;
    public TMP_Text torpedoText;
    private bool torpedoTutorialStart = false;
    private bool torpedoTutorialMid = false;
    private bool torpedoTutorialEnd = false;
    private bool torpedoReady = true;

    public GameObject shot;
    public GameObject bomb;
    public Transform shotSpawn;
    public float fireRate = 0.5f;
    private float nextFire = 0.0f;

    // Fires a bolt on mouse click
    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            GetComponent<AudioSource>().Play();
        }

        if (torpedoTutorialEnd == true)
        {
            torpedoText.text = "Torpedo Ready";
           
        }
        if (Input.GetButton("Fire2") && Time.time > nextFire && GameObject.FindWithTag("Torpedo") == null && torpedoReady == true)
        {
            nextFire = Time.time + fireRate;
            Instantiate(bomb, shotSpawn.position, shotSpawn.rotation);
            GetComponent<AudioSource>().Play();
            torpedoReady = false;
            StartCoroutine(TorpedoTimer());
        }

        // The torpedo tutorial
        torpedoObject = GameObject.FindWithTag("Torpedo");
        if (torpedoTutorialStart == false)
        {
            torpedoText.text = "RIGHT CLICK to FIRE a TORPEDO";
            torpedoTutorialStart = true;
        }
        if (torpedoObject != null && torpedoTutorialEnd == false)
        {
            torpedoText.text = "RIGHT CLICK AGAIN to TRIGGER the TORPEDO";
            torpedoTutorialMid = true;
        }
        if (torpedoObject == null && torpedoTutorialMid == true)
        {
            torpedoText.text = "";
            torpedoTutorialEnd = true;
        }
    }

    // Controls player position
    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        GetComponent<Rigidbody>().velocity = movement * speed;

        GetComponent<Rigidbody>().position = new Vector3
        (
            Mathf.Clamp (GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp (GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
        );

        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
    }

    IEnumerator TorpedoTimer()
    {
        torpedoText.text = "Torpedo Ready";
        yield return new WaitForSeconds(5.0f);
        torpedoReady = true;
    }
}
