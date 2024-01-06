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
            bool isInBuyMode = UIManager.Instance.IsInBuyMode;
            OnButtonClick?.Invoke(_item, isInBuyMode);
        }
    }
    public void UpdateButtonMode(bool isInBuyMode)
    {
        if (_button != null)
        {
            _buttonText.text = isInBuyMode ? "BUY" : "SELL";
        }
    }
}
