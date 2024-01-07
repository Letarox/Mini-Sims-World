using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int _gold = 300;
    [SerializeField] private EquippedItem _equippedHead, _equippedChest;
    [SerializeField] private List<Sprite> _defaultHeadGear, _defaultChestGear;
    [SerializeField] private List<Item> _inventory = new();
    private UIManager _uiManager;
    private bool _isBuying = false;
    public List<Item> Inventory => _inventory;
    public int Gold => _gold;

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
        //Press I to open the Inventory
        if (!_isBuying && Input.GetKeyDown(KeyCode.I))
        {
            _uiManager.OpenInventoryManagement();
        }
        //Press ESC to close the inventory
        if(Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.CurrentActionState == ActionState.Inventory)
        {
            _uiManager.LeaveInventory();
        }
    }
    #region Store Management
    public bool CanSellItem(Item item)
    {
        return _inventory.Contains(item);
    }

    public bool BuyItem(Item item)
    {
        //check if we have the gold to buy it, them deduct the gold and grant the item
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
        //check if we have the item, them remove and add the gold
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
    #endregion
    #region Equip Items
    public void UpdatePlayerEquipment(EquippedItem equippedItem, List<Sprite> newSprites)
    {
        //Null check for return
        if (equippedItem == null || newSprites == null || newSprites.Count == 0)
        {
            return;
        }

        //Minimum count between the VisualReferences and newSprites lists so we can iterate through
        int minCount = Mathf.Min(equippedItem.VisualReferences.Count, newSprites.Count);

        for (int i = 0; i < minCount; i++)
        {
            equippedItem.VisualReferences[i].sprite = newSprites[i];
        }
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

    public void CheckAndEquipItem(ref EquippedItem equippedItem, Item newItem, List<Sprite> defaultSprite)
    {
        if (equippedItem == null)
        {
            equippedItem = new EquippedItem();
        }

        // Check if the item is null (nothing equipped) or if it's different
        if (equippedItem.ItemEquipped != newItem)
        {
            equippedItem.ItemEquipped = newItem;
            UpdatePlayerEquipment(equippedItem, newItem.BodyParts);
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
    #endregion
}
