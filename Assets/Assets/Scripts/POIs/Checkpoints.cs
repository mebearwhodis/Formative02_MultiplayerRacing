using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoints : MonoBehaviour
{
    private Rigidbody _rb;
    private VehiclesInputs _input;
    private int _checkpointsPassed = 0;
    private GameObject _currentCheckpoint;
    private Vector3 _respawnPoint;
    [SerializeField]private bool _canEndTurn;
    private int _turnNumber = 0;
    [SerializeField] private GameObject _thisVehicle;
    private Quaternion _rotation;

    private SetPlayerProfile _setPlayerProfile;

    public int TurnNumber => _turnNumber;
    public GameObject CurrentCheckpoint => _currentCheckpoint;

    public Quaternion Rotation => _rotation;
    public Vector3 RespawnPoint => _respawnPoint;

    private void Start()
    {
        _input = _thisVehicle.GetComponent<VehiclesInputs>();
        _respawnPoint = _thisVehicle.transform.position;
        _rotation = _thisVehicle.transform.rotation;

        _setPlayerProfile = this.transform.parent.GetComponentInParent<SetPlayerProfile>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            if (_currentCheckpoint != other.gameObject)
            {
                _checkpointsPassed++;
            }
            _currentCheckpoint = other.gameObject;
            _respawnPoint = _thisVehicle.transform.position;
            _rotation = other.transform.rotation;
        }
        else if (other.gameObject.CompareTag("MidCheckpoint"))
        {
            _canEndTurn = true;
            _currentCheckpoint = other.gameObject;
            _respawnPoint = _thisVehicle.transform.position;
            _rotation = other.transform.rotation;
        }
        else if (other.gameObject.CompareTag("FinishLine"))
        {
            //if (_checkpointsPassed == 10 && _canEndTurn)
            if (_canEndTurn)
            {
                if (_turnNumber == 2)
                {
                    //End Game
                    //this._setPlayerProfile.IsFirst = true;
                    SceneManager.LoadScene("GameOver");
                }
                else
                {
                    _canEndTurn = false;
                    _checkpointsPassed = 0;
                    _turnNumber++;
                    
                    
                    _currentCheckpoint = other.gameObject;
                    _respawnPoint = _thisVehicle.transform.position;
                    _rotation = other.transform.rotation;
                }
            }
        }
    }
}