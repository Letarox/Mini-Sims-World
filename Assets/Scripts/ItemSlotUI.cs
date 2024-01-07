using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _nameText, _goldText, _buttonText;
    [SerializeField] Image _icon;
    [SerializeField] Button _button;
    private Item _item;
    public event Action<Item, bool> OnButtonClick;
    public event Action<Item> OnItemEquipClick;
    public Item Item => _item;
    private void Start()
    {
        _button.onClick.AddListener(OnItemClick);
    }
    public void SetData(Item item)
    {
        _item = item;
        _nameText.text = item.Name;
        _goldText.text = "Cost: " + item.CostAmount;
        _icon.color = item.MyColor;
        //_icon.sprite = item.Icon.sprite;
    }
    private void OnItemClick()
    {
        if (_item != null)
        {
            if (GameManager.Instance.CurrentActionState != ActionState.Inventory)
            {
                bool isInBuyMode = GameManager.Instance.CurrentActionState == ActionState.Buy ? true : false;
                OnButtonClick?.Invoke(_item, isInBuyMode);
            }
            else
            {
                OnItemEquipClick?.Invoke(_item);
            }
        }
    }
    public void UpdateButtonMode(ActionState state)
    {
        if (_button != null)
        {
            if(state != ActionState.Inventory)
                _buttonText.text = state == ActionState.Buy ? "BUY" : "SELL";
            else
            {
                _buttonText.text = UIManager.Instance.PlayerInventory.CheckIfItemEquipped(_item) ? "UNEQUIP" : "EQUIP";
                _buttonText.fontSize = 12f;
            }
        }
    }
}