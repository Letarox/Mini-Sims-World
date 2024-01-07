using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private List<Item> _shopItems = new();
    private UIManager _uiManager;
    private bool _isPlayerNear = false;
    private PlayerInventory _playerInventory;
    public PlayerInventory PlayerInventory => _playerInventory;
    private void Start()
    {
        _uiManager = UIManager.Instance;
        _uiManager.SetShop(this);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && _isPlayerNear)
        {
            _uiManager.OpenShopActionList();
        }
    }

    public void OpenBuyShop()
    {
        _uiManager.OpenShopUI(_shopItems, ActionState.Buy);
        _playerInventory.SetBuyStatus(true);
    }

    public void OpenSellShop()
    {
        _uiManager.OpenShopUI(_playerInventory.Inventory, ActionState.Sell);
        _playerInventory.SetBuyStatus(true);
    }
    public void HandleItemButtonClick(Item item, bool isBuying)
    {
        if (isBuying)
        {
            BuyItemFromPlayer(item, _playerInventory);
        }
        else
        {
            SellItemToPlayer(item, _playerInventory);
        }
    }
    public void SellItemToPlayer(Item item, PlayerInventory player)
    {
        if (_shopItems.Contains(item))
        {
            if (player.BuyItem(item))
            {
                _shopItems.Remove(item);
                _uiManager.SetShopItemsData(_shopItems);
                _uiManager.UpdateButtonText(GameManager.Instance.CurrentActionState);
                string message = $"You bought {item.Name}!";
                StartCoroutine(_uiManager.ActionItemRoutine(message));
            }
            else
            {
                string message = $"You can't buy {item.Name}!";
                StartCoroutine(_uiManager.ActionItemRoutine(message));
            }
        }
    }
    public void BuyItemFromPlayer(Item item, PlayerInventory player)
    {
        if (player.CanSellItem(item))
        {
            player.SellItem(item);
            _shopItems.Add(item);
            _uiManager.SetShopItemsData(_playerInventory.Inventory);
            _uiManager.UpdateButtonText(GameManager.Instance.CurrentActionState);
            string message = $"You sold {item.Name}!";
            StartCoroutine(_uiManager.ActionItemRoutine(message));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNear = true;
            _playerInventory = other.gameObject.GetComponent<PlayerInventory>();
            _uiManager.SetProximityMessage(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNear = false;
            _playerInventory = null;
            _uiManager.SetProximityMessage(false);
        }
    }
}
