using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Interactibles : MonoBehaviour
{
    private enum PowerUps
    {
    // -MarioStar -> Means there's a way to "die" or it's just terrain immunity
    Star,
    // -MarioSquid (if split screen)
    Squid,
    // -Throw/Drop bomb that pushes other player(s)
    Bomb,
    // -Kind of mario thunderbolt, make other player(s) stop for a few sec?
    Shock,
    // -Few seconds boost?
    Boost,
    // -Inverse controls (left/right) for a few seconds?
    ConfuseRay
    }

    private List<PowerUps> _powerUpsList = new List<PowerUps>()
    {
        PowerUps.Star,
        PowerUps.Squid,
        PowerUps.Bomb,
        PowerUps.Shock,
        PowerUps.Boost,
        PowerUps.ConfuseRay
    };
    
    [SerializeField] private float _boostSpeed = 0;
    private Rigidbody _rb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SpeedBoost"))
        {
            _rb.velocity += transform.forward * _boostSpeed;
            Debug.Log(_rb.velocity.magnitude);
        }
        else if (other.gameObject.CompareTag("ItemBox"))
        {
            other.gameObject.SetActive(false);
            //Rand for power-up
            Debug.Log(_powerUpsList[Random.Range(0,6)]);
            //Make it reappear after a delay
            StartCoroutine(SetActiveAfterDelay(other.gameObject, 5.0f));
        }
    }

    private IEnumerator SetActiveAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(true);
    }
}