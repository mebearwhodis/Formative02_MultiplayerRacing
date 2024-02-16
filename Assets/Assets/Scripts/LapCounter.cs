using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LapCounter : MonoBehaviour
{
    public GameObject _target;
    private TextMeshProUGUI _lapCounterText;
    private int _tempLapCount = 0;

    private void Start()
    {
        _lapCounterText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (_target.GetComponentInChildren<Checkpoints>().TurnNumber != _tempLapCount)
        {
            _lapCounterText.text = "Lap " + (_target.GetComponentInChildren<Checkpoints>().TurnNumber +1) + "/3";
            _tempLapCount++;
        }
    }
    
}
