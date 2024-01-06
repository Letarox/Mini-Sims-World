using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private List<Item> _shopItems = new();
    private bool _isPlayerNear = false;
    private PlayerInventory _playerInventory;
    public PlayerInventory PlayerInventory => _playerInventory;
    private void Start()
    {
        UIManager.Instance.SetShop(this);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && _isPlayerNear)
        {
            UIManager.Instance.OpenShopActionList();
        }
    }

    public void OpenBuyShop()
    {
        UIManager.Instance.OpenShopUI(_shopItems, true);
    }

    public void OpenSellShop()
    {
        UIManager.Instance.OpenShopUI(_playerInventory.Inventory, false);
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
                UIManager.Instance.SetItemsData(_shopItems);
                UIManager.Instance.SetBuyMode(UIManager.Instance.IsInBuyMode);
                string message = $"You bought {item.Name}!";
                StartCoroutine(UIManager.Instance.ActionItemRoutine(message));
            }
            else
            {
                string message = $"You can't buy {item.Name}!";
                StartCoroutine(UIManager.Instance.ActionItemRoutine(message));
            }
        }
    }
    public void BuyItemFromPlayer(Item item, PlayerInventory player)
    {
        if (player.CanSellItem(item))
        {
            player.SellItem(item);
            _shopItems.Add(item);
            UIManager.Instance.SetItemsData(_playerInventory.Inventory);
            UIManager.Instance.SetBuyMode(UIManager.Instance.IsInBuyMode);
            string message = $"You sold {item.Name}!";
            StartCoroutine(UIManager.Instance.ActionItemRoutine(message));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNear = true;
            _playerInventory = other.gameObject.GetComponent<PlayerInventory>();
            UIManager.Instance.SetProximityMessage(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNear = false;
            _playerInventory = null;
            UIManager.Instance.SetProximityMessage(false);
        }
    }
}
