using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int _gold = 100;

    [SerializeField] private List<Item> _inventory = new();
    private bool _isBuying = false;

    public List<Item> Inventory => _inventory;


    private void Start()
    {
        UIManager.Instance.UpdatePlayerGoldDisplay(_gold);
    }
    private void Update()
    {
        if(!_isBuying && Input.GetKeyDown(KeyCode.I))
        {

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
            UIManager.Instance.UpdatePlayerGoldDisplay(_gold);
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
            UIManager.Instance.UpdatePlayerGoldDisplay(_gold);
        }
    }
}
