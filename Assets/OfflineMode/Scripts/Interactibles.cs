using System.Collections;
using UnityEngine;

public class Interactibles : MonoBehaviour
{
    [SerializeField] private float _boostSpeed = 0;
    private Rigidbody _rb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SpeedBoost"))
        {
            _rb.velocity += transform.forward * _boostSpeed;
            Debug.Log("Vroomvroom");
            Debug.Log(_rb.velocity.magnitude);
        }
        else if (other.gameObject.CompareTag("ItemBox"))
        {
            other.gameObject.SetActive(false);
            //Rand for power-up
            Debug.Log(Random.Range(0,5));
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