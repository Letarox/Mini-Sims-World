using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item/Create new Item")]
public class Item : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private int _costAmount;
    [SerializeField] private int _sellAmount;
    [SerializeField] private Sprite _icon;
    [SerializeField] private List<Sprite> _bodyParts;
    [SerializeField] private EquipmentType _equipmentType;
    public string Name => _name;
    public int CostAmount => _costAmount;
    public int SellAmount => _sellAmount;
    public Sprite Icon => _icon;
    public List<Sprite> BodyParts => _bodyParts;
    public EquipmentType EquipmentType => _equipmentType;
}

public enum EquipmentType
{
    Head,
    Chest
}