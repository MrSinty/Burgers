using Ingredients;
using UnityEngine;

public class MousePoint : MonoBehaviour
{
    [SerializeField] private Vector2 startPoint = new Vector2(-20f, -20f);
    [SerializeField] private float zCoord = 1f;
    [SerializeField] private float impulseForce = 15f;

    private Camera _cam;
    private HingeJoint2D _joint;
    private Collider2D dragCollider = null;
    private Rigidbody2D _rb = null;
    private Rigidbody2D dragRb = null;

    private Vector2 tempPreviousPosition;
    private Vector2 previousDragPosition;
    private Vector2 currentDragPosition;
    private Vector2 centerOffset;

    private bool _isDragging = false;
    private bool _isOnIngredient = false;

    private void OnEnable()
    {
        Ingredient.mouseEnter += EnterIngredient;
        Ingredient.mouseOut += ExitIngredient;

        _cam = Camera.main;
        _joint = GetComponent<HingeJoint2D>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.isKinematic = true;
    }

    private void Start()
    {
        _rb.MovePosition(new Vector3 (startPoint.x, startPoint.y, zCoord));
    }

    private void Update()
    {
        if (_isOnIngredient)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                int layerObject = 0;
                Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = zCoord;
                Vector2 ray = (Vector2)mousePos;
                RaycastHit2D hit = Physics2D.Raycast(ray, ray, layerObject);

                if (hit.collider != null && hit.collider.CompareTag("Draggable") && !hit.collider.isTrigger)
                {
                    dragCollider = hit.collider;
                    dragRb = hit.collider.attachedRigidbody;
                    _isDragging = true;

                    currentDragPosition = dragRb.position;
                    centerOffset = dragCollider.bounds.center - mousePos;

                    ConnectToJoint();
                }
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (dragCollider)
            {
                Vector2 direction = currentDragPosition - previousDragPosition;
                float distance = Vector2.Distance(previousDragPosition, currentDragPosition);
                dragRb.velocity = new Vector2(0f, 0f);
                dragRb.AddForce(distance * impulseForce * direction, ForceMode2D.Impulse);
            }

            ReturnToStartState();
        }

        if (_isDragging)
        {
            Vector3 tempPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            tempPos.z = zCoord;
            transform.position = tempPos;


            /*Vector2 direction = (Vector2)_cam.ScreenToWorldPoint(Input.mousePosition) - _rb.position + centerOffset;
            float distance = Vector2.Distance(previousDragPosition, currentDragPosition);
            _rb.AddForceAtPosition(direction * distance, 
                (Vector2)_cam.ScreenToWorldPoint(Input.mousePosition) + centerOffset);*/

            tempPreviousPosition = currentDragPosition;
            currentDragPosition = dragRb.position;
            if (currentDragPosition != tempPreviousPosition)
            {
                previousDragPosition = tempPreviousPosition;
            }
        }
    }

    private void ConnectToJoint()
    {
        _joint.connectedBody = dragCollider.attachedRigidbody;
        _joint.connectedAnchor = dragRb.transform.InverseTransformPoint(dragRb.position - centerOffset);
    }

    private void ReturnToStartState()
    {
        _rb.MovePosition(new Vector3(startPoint.x, startPoint.y, zCoord));
        dragCollider = null;
        dragRb = null;
        _joint.connectedBody = null;
        _isDragging = false;
    }

    private void EnterIngredient()
    {
        _isOnIngredient = true;
    }

    private void ExitIngredient()
    {
        _isOnIngredient = false;
    }
}
