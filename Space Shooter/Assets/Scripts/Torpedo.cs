using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Torpedo : MonoBehaviour
{
    public float explosionRadius = 10.0f;
    private bool isExploding = false;
    public float startWait = 0.5f;
    public float speed = 5;

    private void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    IEnumerator Explode()
    {
        isExploding = true;
        GetComponent<Rigidbody>().velocity = transform.forward * 0.0f;
        Sequence explosion = DOTween.Sequence(); 
        explosion.Append(transform.DOScale(10.0f, 1));
        explosion.Join(transform.DOShakePosition(0.1f, 0.1f));
        explosion.AppendInterval(0.1f);
        yield return explosion.WaitForCompletion();
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            StartCoroutine(Explode());
        }
    }

    // Trigger explosion on collision with other objects
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Obstacle")
        {
            if (isExploding == false)
            {
                StartCoroutine(Explode());
            }
        }
    }

}
