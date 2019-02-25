using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    public float lifetime;

    // Destroys the object after a certain amount of time
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
