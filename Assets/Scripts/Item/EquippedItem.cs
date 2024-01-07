using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquippedItem
{
    [SerializeField] private Item _equippedItem;
    [SerializeField] private List<SpriteRenderer> _visualReferences;

    public Item ItemEquipped
    {
        get { return _equippedItem; }
        set { _equippedItem = value; }
    }

    public List<SpriteRenderer> VisualReferences
    {
        get => _visualReferences;
        set { _visualReferences = value; }
    }
}