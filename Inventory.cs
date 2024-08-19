using System.Collections.Generic;
using UnityEngine;
using Player;

namespace PlayerInventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private GameObject _inventoryUI;
        [Space]
        [SerializeField] private List<InventoryCellInfo> _itemsInInventory = new List<InventoryCellInfo>();
        [SerializeField] private int _maxItemsInStack;

        [System.Serializable]
        private struct InventoryCellInfo 
        {
            public Item Item;
            public bool Stackable;
            public int ItemsInStack;
            public InventoryCell Cell;
        }

        private PlayerInput _playerInput;
        private CellsManipulations _cellsManipulations;
        private bool _inventoryOpen;
        public bool InventoryOpen => _inventoryOpen;

        private void Start()
        {
            _playerInput = FindObjectOfType<PlayerInput>();
            _cellsManipulations = GetComponent<CellsManipulations>();
        }

        private void Update()
        {
            OpenInventoryUI();
        }

        private void OpenInventoryUI()
        {
            if (!_playerInput.OpenInvKey) return;

            if (_inventoryUI != null)
            {
                _inventoryUI.SetActive(!_inventoryUI.activeInHierarchy);
                _inventoryOpen = _inventoryUI.activeInHierarchy;
            }
            
            _playerInput.OpenInvKey = false;
        }

        public void AddItem(Item item)
        {
            if (_itemsInInventory.Count != 0)
            {
                bool itemFound = false;
                for (int i = 0; i < _itemsInInventory.Count; i++)
                {
                    if (_itemsInInventory[i].Item == item && _itemsInInventory[i].Stackable && _itemsInInventory[i].ItemsInStack < _maxItemsInStack)
                    {
                        UpdateItem(i);
                        itemFound = true;
                        break;
                    }
                }

                if (!itemFound)
                {
                    NewItem(item);
                }
            }
            else
            {
                NewItem(item);
            }
        }

        private void NewItem(Item item)
        {
            InventoryCellInfo newItem = new InventoryCellInfo();
            newItem.Item = item;
            newItem.Stackable = item.Stackable;
            newItem.ItemsInStack++;

            newItem.Cell = _cellsManipulations.ReserveCell();
            newItem.Cell.SetItemSprite(newItem.Item.ItemSprite);
            newItem.Cell.ChangeCount(newItem.ItemsInStack);

            _itemsInInventory.Add(newItem);
        }

        private void UpdateItem(int index)
        {
            InventoryCellInfo updateItem = _itemsInInventory[index];
            updateItem.ItemsInStack++;
            updateItem.Cell.ChangeCount(updateItem.ItemsInStack);
            _itemsInInventory[index] = updateItem;
        }

        public bool ChangeCell(InventoryCell oldCell, InventoryCell newCell)
        {
            for (int i = 0; i < _itemsInInventory.Count; i++)
            {
                if (_itemsInInventory[i].Cell == oldCell)
                {
                    InventoryCellInfo swapCell = _itemsInInventory[i];
                    swapCell.Cell = newCell;
                    _itemsInInventory[i] = swapCell;
                    UpdateCellVisual(i);
                    return true;
                }
            }
            return false;
        }

        private void UpdateCellVisual(int index)
        {
            _itemsInInventory[index].Cell.IsEmpty = false;
            _itemsInInventory[index].Cell.ChangeCount(_itemsInInventory[index].ItemsInStack);
            _itemsInInventory[index].Cell.SetItemSprite(_itemsInInventory[index].Item.ItemSprite);
        }

        public Item GetItemFromInventoryCell(InventoryCell cell)
        {
            for (int i = 0; i < _itemsInInventory.Count; i++)
            {
                if (_itemsInInventory[i].Cell == cell)
                {
                    return _itemsInInventory[i].Item;
                }
            }

            return null;
        }
    }
}