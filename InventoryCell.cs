using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PlayerInventory
{
    public class InventoryCell : MonoBehaviour
    {
        public bool IsEmpty = true;
        public bool HandCell;
        [SerializeField] private TMP_Text _itemCountDisplay;
        [SerializeField] private Image _itemSprite;

        public void SetItemSprite(Sprite sprite, bool show = true)
        {
            _itemSprite.sprite = sprite;
            _itemSprite.gameObject.SetActive(show);
        }

        public void ChangeCount(int count) => _itemCountDisplay.text = count.ToString();

        public void ClearCell()
        {
            IsEmpty = true;
            SetItemSprite(null, false);
            _itemCountDisplay.text = "";
        }
    }
}