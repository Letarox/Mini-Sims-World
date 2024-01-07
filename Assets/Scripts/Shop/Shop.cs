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
        //Press E to open the shop (when near)
        if(Input.GetKeyDown(KeyCode.E) && _isPlayerNear && GameManager.Instance.CurrentActionState == ActionState.None)
        {
            _uiManager.OpenShopActionList(ActionState.Browsing);
        }
        //Press ESC to close the shop while either browsing or buying/selling
        if(Input.GetKeyDown(KeyCode.Escape) && (GameManager.Instance.CurrentActionState != ActionState.None && GameManager.Instance.CurrentActionState != ActionState.Inventory))
        {
            _uiManager.LeaveShop();
        }
    }

    public void OpenBuyShop()
    {
        _uiManager.OpenShopUI(_shopItems, ActionState.Buy);
        if(_playerInventory != null)
            _playerInventory.SetBuyStatus(true);
    }

    public void OpenSellShop()
    {
        _uiManager.OpenShopUI(_playerInventory.Inventory, ActionState.Sell);
        if (_playerInventory != null)
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
        //check if we have the item on our list, them buy that item for the player, remove the item from our list and update the visuals
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
        //check if the player has the item their inventory, them buy that item from the player, remove the item from their inventory and update the visuals
        if (player.CanSellItem(item))
        {
            player.SellItem(item);
            _shopItems.Add(item);
            if (_playerInventory != null)
                _uiManager.SetShopItemsData(_playerInventory.Inventory);
            _uiManager.UpdateButtonText(GameManager.Instance.CurrentActionState);
            string message = $"You sold {item.Name}!";
            StartCoroutine(_uiManager.ActionItemRoutine(message));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Checks when the player gets near our NPC
        if (other.CompareTag("Player"))
        {
            _isPlayerNear = true;
            _playerInventory = other.gameObject.GetComponent<PlayerInventory>();
            _uiManager.SetProximityMessage(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Check if the player left our NPC range
        if (other.CompareTag("Player"))
        {
            _isPlayerNear = false;
            _playerInventory = null;
            _uiManager.SetProximityMessage(false);
        }
    }
}
