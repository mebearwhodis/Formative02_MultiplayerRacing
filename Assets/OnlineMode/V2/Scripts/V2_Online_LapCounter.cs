using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class V2_Online_LapCounter : MonoBehaviour
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
        if (_target.GetComponent<V2_Online_Checkpoints>().TurnNumber != _tempLapCount)
        {
            _lapCounterText.text = "Lap " + _target.GetComponent<V2_Online_Checkpoints>().TurnNumber + "/3";
            _tempLapCount++;
        }
    }
    
}
