using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "VehicleDatas/VehicleProfile", fileName = "VehicleProfile")]
public class VehicleProfile : ScriptableObject
{
 //Model = No physics
 public GameObject _model;

//Prefab = with physics
 public GameObject _prefab;

 [Header("Gameplay Datas")] 
 public string Name;
 public string ModelName;
}
