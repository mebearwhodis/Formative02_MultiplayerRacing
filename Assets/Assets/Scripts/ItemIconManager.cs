using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemIconManager : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private List<Sprite> _itemIcons;
    [SerializeField] private TextMeshProUGUI _itemEffect;
    private Interactibles _items;
    
    // Start is called before the first frame update
    void Start()
    {
        _items = GetComponentInChildren<Interactibles>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeItemIcon(_items.ItemHeld);
        if (_items.IsImmune)
        {
            _itemEffect.text = "Immune";
        }
        else if (_items.IsStunned)
        {
            _itemEffect.text = "Stunned";
        }
        else if (_items.IsConfused)
        {
            _itemEffect.text = "Confused";
        }
        else
        {
            _itemEffect.text = "";
        }
    }
    
    private void ChangeItemIcon(int itemNumber)
    { 
        _itemImage.sprite = _itemIcons[itemNumber];
    }
}
