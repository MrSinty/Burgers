using UnityEngine;
using Ingredients;

namespace Ingredients
{
    public class CollisionDetection : MonoBehaviour
    {
        Collider2D _col = null;

        public delegate void ColliderEnter(Collider2D collision, Side side);
        public static event ColliderEnter colliderEnter;

        private void OnEnable()
        {
            _col = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Vector2 botCenter = new Vector2(collision.bounds.center.x, collision.bounds.center.y - collision.bounds.extents.y);
            Vector2 topCenter = new Vector2(collision.bounds.center.x, collision.bounds.center.y + collision.bounds.extents.y);
            if (collision.CompareTag("Draggable"))
            {
                if (_col.bounds.Contains(botCenter))
                {
                    colliderEnter?.Invoke(collision, Side.BOTTOM);
                }
                if (_col.bounds.Contains(topCenter))
                {
                    colliderEnter?.Invoke(collision, Side.TOP);
                }
            }
        }
    }
}
