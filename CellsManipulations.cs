using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlayerInventory
{
    public class CellsManipulations : MonoBehaviour
    {
        [SerializeField] private List<InventoryCell> _inventoryCells = new List<InventoryCell>();

        [Header("Canvas")]
        [SerializeField] private GraphicRaycaster _mRaycaster;
        [SerializeField] private EventSystem _mEventSystem;
        [SerializeField] private RectTransform _canvasTransform;

        private PlayerInput _playerInput;
        private Inventory _inventory;
        private PointerEventData _mPointerEventData;
        private GameObject _dragedCell;
        private InventoryCell _oldCell;
        private HandItem _handItem;

        private void Start()
        {
            _inventory = GetComponent<Inventory>();
            _playerInput = FindObjectOfType<PlayerInput>();
            _handItem = FindObjectOfType<HandItem>();
        }

        private void Update()
        {
            DragItem();
        }

        public InventoryCell ReserveCell()
        {
            for (int i = 0; i < _inventoryCells.Count; i++)
            {
                if (_inventoryCells[i].IsEmpty)
                {
                    _inventoryCells[i].IsEmpty = false;
                    return _inventoryCells[i];
                }
            }

            return null;
        }

        private void DragItem()
        {
            if (_playerInput.LMBKey && _inventory.InventoryOpen)
            {

                _mPointerEventData = new PointerEventData(_mEventSystem);
                _mPointerEventData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                _mRaycaster.Raycast(_mPointerEventData, results);

                for (int i = 0; i < results.Count; i++)
                {
                    InventoryCell cell = results[i].gameObject.GetComponent<InventoryCell>();
                    if (cell != null)
                    {
                        if (_dragedCell == null && !cell.IsEmpty)
                        {
                            _oldCell = cell;

                            _dragedCell = Instantiate(new GameObject("DragingCell"), _mPointerEventData.position, Quaternion.identity);
                            
                            Item itemInsideCell = _inventory.GetItemFromInventoryCell(cell);
                            Image cellImage = _dragedCell.AddComponent<Image>();
                            
                            cellImage.sprite = itemInsideCell.ItemSprite;

                            _dragedCell.transform.SetParent(_canvasTransform);
                            
                            RectTransform rectTransform = _dragedCell.GetComponent<RectTransform>();
                            rectTransform.sizeDelta = new Vector2(60f, 60f);
                            
                            Image image = _dragedCell.GetComponent<Image>();
                            image.raycastTarget = false;

                            cell.ClearCell();
                            break;
                        }
                        else if (cell.IsEmpty)
                        {
                            Item itemInsideCell = _inventory.GetItemFromInventoryCell(_oldCell);
                            if (cell.HandCell && !itemInsideCell.CanBeInHand) return;
                            

                            if (_inventory.ChangeCell(_oldCell, cell))
                            {
                                if (itemInsideCell.CanBeInHand) _handItem.GetItemPrefabsFromCell(); ;
                                Destroy(_dragedCell.gameObject);
                                _dragedCell = null;
                                _oldCell = null;
                            }
                            break;
                        }
                    }
                }

                _playerInput.LMBKey = false;
            }

            if (_dragedCell != null) _dragedCell.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
        }
    }
}