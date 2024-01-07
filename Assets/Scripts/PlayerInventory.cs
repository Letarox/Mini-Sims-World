using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int _gold = 300;

    [SerializeField] private List<Item> _inventory = new();
    private UIManager _uiManager;
    private bool _isBuying = false;
    [SerializeField] private Item _equippedHeadItem, _equippedChestItem, _equippedFeetItem;

    public List<Item> Inventory => _inventory;

    private void OnEnable()
    {
        UIManager.OnItemAction += EquipItem;
    }
    private void OnDestroy()
    {
        UIManager.OnItemAction -= EquipItem;
    }

    private void Start()
    {
        _uiManager = UIManager.Instance;
        _uiManager.SetPlayer(this);
        _uiManager.UpdatePlayerGoldDisplay(_gold);
    }
    private void Update()
    {
        if(!_isBuying && Input.GetKeyDown(KeyCode.I))
        {
            _uiManager.OpenInventoryManagement(_inventory);
        }
    }
    public bool CanSellItem(Item item)
    {
        return _inventory.Contains(item);
    }
    public bool BuyItem(Item item)
    {
        if (_gold >= item.CostAmount)
        {
            _gold -= item.CostAmount;
            _inventory.Add(item);
            _uiManager.UpdatePlayerGoldDisplay(_gold);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SellItem(Item item)
    {
        if (_inventory.Contains(item))
        {
            _gold += item.SellAmount;
            _inventory.Remove(item);
            _uiManager.UpdatePlayerGoldDisplay(_gold);
        }
    }

    public void SetBuyStatus(bool active)
    {
        _isBuying = active;
    }

    public void UpdatePlayerColor(Color newColor)
    {
        gameObject.GetComponent<SpriteRenderer>().color = newColor;
    }

    public void EquipItem(Item item)
    {
        switch (item.EquipmentType)
        {
            case EquipmentType.Head:
                CheckAndEquipItem(ref _equippedHeadItem, item);
                break;
            case EquipmentType.Chest:
                CheckAndEquipItem(ref _equippedChestItem, item);
                break;
            case EquipmentType.Feet:
                CheckAndEquipItem(ref _equippedFeetItem, item);
                break;
        }
    }

    public void CheckAndEquipItem(ref Item equippedItem, Item newItem)
    {
        if (equippedItem == null)
        {
            equippedItem = newItem;
            UpdatePlayerColor(equippedItem.MyColor);
            UIManager.Instance.UpdateButtonText(GameManager.Instance.CurrentActionState);
        }
        else
        {
            if (equippedItem == newItem)
            {
                equippedItem = null;
                UpdatePlayerColor(Color.white);
                UIManager.Instance.UpdateButtonText(GameManager.Instance.CurrentActionState);
            }
            else
            {
                equippedItem = newItem;
                UpdatePlayerColor(equippedItem.MyColor);
                UIManager.Instance.UpdateButtonText(GameManager.Instance.CurrentActionState);
            }
        }
    }

    public bool CheckIfItemEquipped(Item item)
    {
        return item.EquipmentType switch
        {
            EquipmentType.Head => item == _equippedHeadItem,
            EquipmentType.Chest => item == _equippedChestItem,
            EquipmentType.Feet => item == _equippedFeetItem,
            _ => false,
        };
    }
}
