using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Create new Item")]
public class Item : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private int _costAmount;
    [SerializeField] private int _sellAmount;
    [SerializeField] private Image _icon;
    [SerializeField] private Color _color;
    [SerializeField] private EquipmentType _equipmentType;
    public string Name => _name;
    public int CostAmount => _costAmount;
    public int SellAmount => _sellAmount;
    public Image Icon => _icon;
    public Color MyColor => _color;
    public EquipmentType EquipmentType => _equipmentType;
}

public enum EquipmentType
{
    Head,
    Chest,
    Feet
}