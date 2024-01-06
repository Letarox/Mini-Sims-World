using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] GameObject _shopBackground,_shopActionList, _itemShopList, _shopHeader, _container;
    [SerializeField] GameObject _itemTemplatePrefab;
    [SerializeField] TextMeshProUGUI _playerGold,_proximityMessage, _actionText;
    private readonly WaitForSeconds _actionTextDelay = new(3f);
    private List<ItemSlotUI> _shopSlots = new();
    private bool _isInBuyMode = true;
    public bool IsInBuyMode => _isInBuyMode;
    private Shop _shop;
    private void Awake()
    {
        Instance = this;
    }
    public void SetShop(Shop shop)
    {
        _shop = shop;
    }
    public void SetBuyMode(bool active)
    {
        _isInBuyMode = active;
        foreach (ItemSlotUI slot in _shopSlots)
        {
            slot.UpdateButtonMode(_isInBuyMode);
        }
    }
    public void SetItemsData(List<Item> items)
    {
        if (_shopSlots != null)
        {
            foreach (ItemSlotUI slot in _shopSlots)
            {
                Destroy(slot.gameObject);
            }
            _shopSlots.Clear();
        }

        foreach (Item item in items)
        {
            GameObject newItemSlotObject = Instantiate(_itemTemplatePrefab, _container.transform);
            ItemSlotUI newItemSlot = newItemSlotObject.GetComponent<ItemSlotUI>();            
            newItemSlot.SetData(item);
            if (newItemSlot != null)
                newItemSlot.OnButtonClick += HandleItemSlotButtonClick;

            _shopSlots.Add(newItemSlot);
        }
    }
    private void HandleItemSlotButtonClick(Item item, bool isBuying)
    {
        if (isBuying)
        {
            _shop.SellItemToPlayer(item, _shop.PlayerInventory);
        }
        else
        {
            _shop.BuyItemFromPlayer(item, _shop.PlayerInventory);
        }
    }
    public void OpenShopActionList()
    {
        _shopBackground.SetActive(true);
        _shopActionList.SetActive(true);
        _shopHeader.SetActive(true);
        SetProximityMessage(false);        
    }
    public void OpenShopUI(List<Item> items, bool isBuying)
    {
        _shopActionList.SetActive(false);
        _itemShopList.SetActive(true);
        _shopHeader.SetActive(true);
        SetItemsData(items);
        SetBuyMode(isBuying);        
    }
    public void LeaveShop()
    {
        _shopBackground.SetActive(false);
        _shopActionList.SetActive(false);
        _itemShopList.SetActive(false);
        _shopHeader.SetActive(false);
        SetProximityMessage(true);
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
}