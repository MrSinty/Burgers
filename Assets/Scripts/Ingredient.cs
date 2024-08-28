using UnityEngine;

namespace Ingredients
{
    public enum Side
    {
        TOP,
        BOTTOM
    };

    public class Ingredient : MonoBehaviour
    {
        private float cost;
        public delegate void MouseEnter();
        public static event MouseEnter mouseEnter;
        public delegate void MouseOut();
        public static event MouseOut mouseOut;

        private void OnMouseEnter()
        {
            mouseEnter?.Invoke();
        }

        private void OnMouseExit()
        {
            mouseOut?.Invoke();
        }
    }
}