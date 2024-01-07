using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{    
    public static UIManager Instance;
    [SerializeField] GameObject _backgroundPanel,_shopActionList, _itemShopList, _inventoryList, _shopHeader, _inventoryHeader, _shopContainer, _inventoryContainer;
    [SerializeField] GameObject _itemTemplatePrefab;
    [SerializeField] TextMeshProUGUI _playerGold,_proximityMessage, _actionText;
    private readonly WaitForSeconds _actionTextDelay = new(3f);
    private List<ItemSlotUI> _itemSlots = new();
    private PlayerInventory _playerInventory;
    private Shop _shop; 
    public static event Action<Item> OnItemAction;
    public PlayerInventory PlayerInventory => _playerInventory;
    private void Awake()
    {
        Instance = this;
    }
    public void SetShop(Shop shop)
    {
        _shop = shop;
    }
    public void SetPlayer(PlayerInventory playerInventory)
    {
        _playerInventory = playerInventory;
    }
    public void UpdateButtonText(ActionState state)
    {
        foreach (ItemSlotUI slot in _itemSlots)
        {
            slot.UpdateButtonMode(state);
        }
    }
    public void SetShopItemsData(List<Item> items)
    {
        if (_itemSlots != null)
        {
            foreach (ItemSlotUI slot in _itemSlots)
            {
                Destroy(slot.gameObject);
            }
            _itemSlots.Clear();
        }

        foreach (Item item in items)
        {
            if (_playerInventory != null && !_playerInventory.CheckIfItemEquipped(item))
            {
                GameObject newItemSlotObject = Instantiate(_itemTemplatePrefab, _shopContainer.transform);
                ItemSlotUI newItemSlot = newItemSlotObject.GetComponent<ItemSlotUI>();
                newItemSlot.SetData(item);
                if (newItemSlot != null)
                {
                    newItemSlot.OnButtonClick += HandleItemSlotButtonClick;
                }

                _itemSlots.Add(newItemSlot);
            }
        }
    }
    public void SetInventoryItemsData(List<Item> items)
    {
        if (_itemSlots != null)
        {
            foreach (ItemSlotUI slot in _itemSlots)
            {
                Destroy(slot.gameObject);
            }
            _itemSlots.Clear();
        }

        foreach (Item item in items)
        {
            GameObject newItemSlotObject = Instantiate(_itemTemplatePrefab, _inventoryContainer.transform);
            ItemSlotUI newItemSlot = newItemSlotObject.GetComponent<ItemSlotUI>();
            newItemSlot.SetData(item);
            if (newItemSlot != null)
            {
                newItemSlot.OnItemEquipClick += HandleItemEquipButtonClick;
            }

            _itemSlots.Add(newItemSlot);
        }
    }
    private void HandleItemSlotButtonClick(Item item, bool isBuying)
    {
        if (isBuying)
        {
            if(_shop != null)
                _shop.SellItemToPlayer(item, _shop.PlayerInventory);
        }
        else
        {
            if (_shop != null)
                _shop.BuyItemFromPlayer(item, _shop.PlayerInventory);
        }
    }
    private void HandleItemEquipButtonClick(Item item)
    {
        EquipOrUnequipItem(item);
    }
    public void OpenShopActionList(ActionState state)
    {
        _backgroundPanel.SetActive(true);
        _shopActionList.SetActive(true);
        _shopHeader.SetActive(true);
        SetProximityMessage(false);
        GameManager.Instance.SetActionState(state);
    }
    public void OpenShopUI(List<Item> items, ActionState state)
    {
        _shopActionList.SetActive(false);
        _itemShopList.SetActive(true);
        _shopHeader.SetActive(true);
        SetShopItemsData(items);
        GameManager.Instance.SetActionState(state);
        UpdateButtonText(state);
    }
    public void LeaveShop()
    {
        _backgroundPanel.SetActive(false);
        _shopActionList.SetActive(false);
        _itemShopList.SetActive(false);
        _shopHeader.SetActive(false);
        SetProximityMessage(true);
        _shop.PlayerInventory.SetBuyStatus(false);
        GameManager.Instance.SetActionState(ActionState.None);
    }
    public void LeaveInventory()
    {
        _backgroundPanel.SetActive(false);
        _inventoryList.SetActive(false);
        _inventoryHeader.SetActive(false);
        GameManager.Instance.SetActionState(ActionState.None);
    }
    public void SetProximityMessage(bool active)
    {
        _proximityMessage.gameObject.SetActive(active);
    }
    public void UpdatePlayerGoldDisplay(int amount)
    {
        _playerGold.text = $"Gold: {amount}";
    }

    public IEnumerator ActionItemRoutine(string message)
    {
        _actionText.text = message;
        _actionText.gameObject.SetActive(true);
        yield return _actionTextDelay;
        _actionText.gameObject.SetActive(false);
    }

    public void OpenInventoryManagement(List<Item> items)
    {
        _backgroundPanel.SetActive(true);
        _inventoryList.SetActive(true);
        _inventoryHeader.SetActive(true);
        SetProximityMessage(false);
        GameManager.Instance.SetActionState(ActionState.Inventory);
        SetInventoryItemsData(items);
        UpdateButtonText(GameManager.Instance.CurrentActionState);
    }

    private void EquipOrUnequipItem(Item item)
    {
        if(OnItemAction != null)
            OnItemAction?.Invoke(item);
    }
}