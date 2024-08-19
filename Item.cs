using UnityEngine;
using UnityEngine.UI;

namespace PlayerInventory
{

    [CreateAssetMenu(fileName = "Item", menuName = "Item", order = 0)]
    public class Item : ScriptableObject
    {
        public Sprite ItemSprite;
        public bool Stackable;
        [Space]
        public bool CanBeInHand;
        public GameObject ItemPrefab;
    }
}