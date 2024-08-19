using UnityEngine;

namespace PlayerInventory
{
    public class PickableItem : MonoBehaviour
    {
        [SerializeField] private Item _item;
        
        private Inventory _inventory;

        private void Start()
        {
            _inventory = FindObjectOfType<Inventory>();
        }

        public void PickUp()
        {
            _inventory.AddItem(_item);
            Destroy(gameObject);
        }
    }
}