using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Interactibles : MonoBehaviour
{
    private enum PowerUps
    {
        None = 0,

        // Immunity to other objects
        Immunity,

        // -Pushes other players
        Bomb,

        // -Stops other players from doing anything for a few seconds
        Shock,

        // -Gives a huge boost
        Boost,

        // -Inverse controls (left/right) for a few seconds
        ConfuseRay
    }

    private List<PowerUps> _powerUpsList = new List<PowerUps>()
    {
        PowerUps.None,
        PowerUps.Immunity,
        PowerUps.Bomb,
        PowerUps.Shock,
        PowerUps.Boost,
        PowerUps.ConfuseRay
    };

    [Header("Testing")] 
    [SerializeField] private int _itemHeld = 0;
    [SerializeField] private bool _isImmune = false;
    [SerializeField] private bool _isConfused = false;
    [SerializeField] private bool _isStunned = false;

    public int ItemHeld => _itemHeld;

    public bool IsImmune => _isImmune;

    public bool IsConfused
    {
        get => _isConfused;
        set => _isConfused = value;
    }

    public bool IsStunned
    {
        get => _isStunned;
        set => _isStunned = value;
    }

    [Header("Items Tweak")] [SerializeField] private float _immunityDuration = 3f;
    [SerializeField] private float _pushForce = 10000f;
    [SerializeField] private float _shockDuration = 2f;
    [SerializeField] private float _itemBoostSpeed = 25f;
    [SerializeField] private float _confusionDuration = 2f;


    [Header("Other")] [SerializeField] private float _boostSpeed = 10f;

    private Rigidbody _rb;
    private VehiclesInputs _input;
    [SerializeField] private GameObject _thisVehicle;

    private void Start()
    {
        _input = _thisVehicle.GetComponent<VehiclesInputs>();
        _rb = _thisVehicle.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_input._itemUsed && _itemHeld != 0)
        {
            UseItem(_itemHeld);
            _itemHeld = 0;
        }
    }

    //Interact with and pick up items
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SpeedBoost"))
        {
            // Calculate boost direction based on the speed boost object's forward vector
            Vector3 boostDirection = other.transform.forward.normalized;
            _rb.velocity += boostDirection * _boostSpeed;
        }
        else if (other.gameObject.CompareTag("ItemBox"))
        {
            other.gameObject.SetActive(false);
            if (_itemHeld == 0)
            {
                //Rand for power-up
                _itemHeld = (Random.Range(0, 5) + 1);
            }
            //Make it reappear after a delay
            StartCoroutine(SetActiveAfterDelay(other.gameObject, 5.0f));
        }
    }

    //Respawn items
    private IEnumerator SetActiveAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(true);
    }

    //Use items
    private void UseItem(int itemHeld)
    {
        switch (itemHeld)
        {
            case 1:
                UseImmunity();
                break;
            case 2:
                UseBomb();
                break;
            case 3:
                UseShock();
                break;
            case 4:
                UseBoost();
                break;
            case 5:
                UseConfuseRay();
                break;
            default:
                break;
        }
    }


    
    //1 Immunity Item
    private void UseImmunity()
    {
        _isImmune = true;
        //Add visual feedback here

        Invoke("StopImmunity", _immunityDuration);
    }

    private void StopImmunity()
    {
        _isImmune = false;
        //Remove visual feedback here
    }

    //2 Bomb Item
    private void UseBomb()
    {
        GameObject[] vehicles = GameObject.FindGameObjectsWithTag("Vehicle");

        foreach (GameObject vehicle in vehicles)
        {
            // Exclude the player who initiated the push & players that are immune
            if (vehicle != _thisVehicle && !vehicle.GetComponentInChildren<Interactibles>()._isImmune)
            {
                Rigidbody rb = vehicle.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Calculate push direction away from the user
                    Vector3 pushDirection = (vehicle.transform.position - transform.position).normalized;
                    // Apply force to push the player
                    rb.AddForce(new Vector3(pushDirection.x * _pushForce, 1, pushDirection.z * _pushForce),
                        ForceMode.Impulse);
                }
            }
        }
    }

    //3 Shock Item
    private void UseShock()
    {
        GameObject[] vehicles = GameObject.FindGameObjectsWithTag("Vehicle");

        foreach (GameObject vehicle in vehicles)
        {
            if (vehicle != _thisVehicle && !vehicle.GetComponentInChildren<Interactibles>()._isImmune)
            {
                MonoBehaviour script = vehicle.GetComponent("SimcadeVehicleController") as MonoBehaviour;
                if (script != null)
                {
                    script.enabled = false;
                }

                vehicle.GetComponentInChildren<Interactibles>().IsStunned = true;
            }
        }

        Invoke("ReactivateInputs", _shockDuration);
    }

    private void ReactivateInputs()
    {
        GameObject[] vehicles = GameObject.FindGameObjectsWithTag("Vehicle");

        foreach (GameObject vehicle in vehicles)
        {
            if (vehicle != _thisVehicle && !vehicle.GetComponentInChildren<Interactibles>()._isImmune)
            {
                MonoBehaviour script = vehicle.GetComponent("SimcadeVehicleController") as MonoBehaviour;
                if (script != null)
                {
                    script.enabled = true;
                }
                
                vehicle.GetComponentInChildren<Interactibles>().IsStunned = false;
            }
        }
    }

    //4 Boost Item
    private void UseBoost()
    {
        _rb.velocity += transform.forward * _itemBoostSpeed;
    }

    //5 ConfuseRay Item
    private void UseConfuseRay()
    {
        GameObject[] vehicles = GameObject.FindGameObjectsWithTag("Vehicle");

        foreach (GameObject vehicle in vehicles)
        {
            if (vehicle != _thisVehicle && !vehicle.GetComponentInChildren<Interactibles>()._isImmune)
            {
                vehicle.GetComponent<VehiclesInputs>()._invertControls = true;
                vehicle.GetComponentInChildren<Interactibles>().IsConfused = true;
            }
        }

        Invoke("RestoreControls", _confusionDuration);
    }

    private void RestoreControls()
    {
        GameObject[] vehicles = GameObject.FindGameObjectsWithTag("Vehicle");

        foreach (GameObject vehicle in vehicles)
        {
            if (vehicle != _thisVehicle && !vehicle.GetComponentInChildren<Interactibles>()._isImmune)
            {
                vehicle.GetComponent<VehiclesInputs>()._invertControls = false;
                vehicle.GetComponentInChildren<Interactibles>().IsConfused = false;
            }
        }
    }
}