using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class V2_Online_ItemIconManager : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private List<Sprite> _itemIcons;
    private V2_Online_Interactibles _items;
    
    // Start is called before the first frame update
    void Start()
    {
        _items = GetComponentInChildren<V2_Online_Interactibles>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeItemIcon(_items.ItemHeld);
    }
    
    private void ChangeItemIcon(int itemNumber)
    { 
        _itemImage.sprite = _itemIcons[itemNumber];
    }
}
