using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int _gold = 300;

    [SerializeField] private List<Item> _inventory = new List<Item>();
    private UIManager _uiManager;
    private bool _isBuying = false;

    [SerializeField] private EquippedItem _equippedHead;
    [SerializeField] private EquippedItem _equippedChest;

    [SerializeField] private Sprite _defaultHeadGear;
    [SerializeField] private Sprite _defaultChestGear;

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
        if (!_isBuying && Input.GetKeyDown(KeyCode.I))
        {
            _uiManager.OpenInventoryManagement(_inventory);
        }

        if(Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.CurrentActionState == ActionState.Inventory)
        {
            _uiManager.LeaveInventory();
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

    public void UpdatePlayerEquipment(EquippedItem equippedItem, Sprite newSprite)
    {
        equippedItem.VisualReference.sprite = newSprite;
    }

    public void EquipItem(Item item)
    {
        switch (item.EquipmentType)
        {
            case EquipmentType.Head:
                CheckAndEquipItem(ref _equippedHead, item, _defaultHeadGear);
                break;
            case EquipmentType.Chest:
                CheckAndEquipItem(ref _equippedChest, item, _defaultChestGear);
                break;
        }
    }

    public void CheckAndEquipItem(ref EquippedItem equippedItem, Item newItem, Sprite defaultSprite)
    {
        //Check if the item is null (nothing equipped) or if its different
        if (equippedItem == null || equippedItem.ItemEquipped != newItem)
        {
            equippedItem.ItemEquipped = newItem;
            UpdatePlayerEquipment(equippedItem, newItem.Icon);
        }
        else
        {
            equippedItem.ItemEquipped = null;
            UpdatePlayerEquipment(equippedItem, defaultSprite);
        }

        UIManager.Instance.UpdateButtonText(GameManager.Instance.CurrentActionState);
    }

    public bool CheckIfItemEquipped(Item item)
    {
        //returns true if the item equipped matches the provided item
        return item.EquipmentType switch
        {
            EquipmentType.Head => item == _equippedHead.ItemEquipped,
            EquipmentType.Chest => item == _equippedChest.ItemEquipped,
            _ => false,
        };
    }
}
