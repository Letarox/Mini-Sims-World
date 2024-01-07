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
        _icon.sprite = item.Icon;
    }
    private void OnItemClick()
    {
        //check if the state is not inventory, to proper setup the action for the items. In case it its, sets the action for the inventory handling
        if (_item != null)
        {
            if (GameManager.Instance.CurrentActionState != ActionState.Inventory)
            {
                bool isInBuyMode = GameManager.Instance.CurrentActionState == ActionState.Buy ? true : false;
                if(OnButtonClick != null)
                    OnButtonClick?.Invoke(_item, isInBuyMode);
            }
            else
            {
                if(OnItemEquipClick != null)
                    OnItemEquipClick?.Invoke(_item);
            }
        }
    }
    public void UpdateCostText(ActionState state)
    {
        //Update the button text according to either buying/selling or equipping and unequipping
        if (_goldText != null)
        {
            if (state != ActionState.Inventory)
            {
                _goldText.text = $"Cost: {_item.CostAmount}";
            }
            else
            {
                if (_item != null && UIManager.Instance != null && UIManager.Instance.PlayerInventory != null)
                {
                    _goldText.text = $"Value: {_item.SellAmount}";
                }
            }
        }
    }
    public void UpdateButtonMode(ActionState state)
    {
        //Update the button text according to either buying/selling or equipping and unequipping
        if (_button != null && _buttonText != null)
        {
            if (state != ActionState.Inventory)
            {
                _buttonText.text = state == ActionState.Buy ? "BUY" : "SELL";
                if(state == ActionState.Buy)
                    _button.image.color = UIManager.Instance.PlayerInventory.Gold >= _item.CostAmount ? Color.green : Color.red;
            }
            else
            {
                if (_item != null && UIManager.Instance != null && UIManager.Instance.PlayerInventory != null)
                {
                    _buttonText.text = UIManager.Instance.PlayerInventory.CheckIfItemEquipped(_item) ? "UNEQUIP" : "EQUIP";
                    _button.image.color = UIManager.Instance.PlayerInventory.CheckIfItemEquipped(_item) ? Color.grey : Color.cyan;
                    _buttonText.fontSize = 12f;
                }
            }
        }
    }
}