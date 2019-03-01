using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour
{
    // Destroys object when it leaves the box collider
    private void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}
