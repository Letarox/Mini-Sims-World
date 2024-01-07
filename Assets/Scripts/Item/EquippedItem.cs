using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquippedItem
{
    [SerializeField] private Item _equippedItem;
    [SerializeField] private SpriteRenderer _visualReference;

    public Item ItemEquipped
    {
        get { return _equippedItem; }
        set { _equippedItem = value; }
    }

    public SpriteRenderer VisualReference
    {
        get { return _visualReference; }
        set { _visualReference = value; }
    }
}