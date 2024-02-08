using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointGizmos : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 0.25f);
    }
}