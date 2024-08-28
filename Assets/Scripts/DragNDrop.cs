using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    [SerializeField] private float impulseForce = 15f;

    private Camera _cam;
    private Rigidbody2D _rb;
    private BoxCollider2D _bCol;

    private Vector2 tempPrevPos;
    private Vector2 previousDragPosition;
    private Vector2 currentDragPosition;
    private Vector2 centerOffset;

    private void Start()
    {
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
        _bCol = GetComponent<BoxCollider2D>();
    }

    private void OnMouseDown()
    {
        //_rb.isKinematic = true;
        centerOffset = _bCol.bounds.center - _cam.ScreenToWorldPoint(Input.mousePosition);
        currentDragPosition = _rb.position;
    }

    private void OnMouseDrag()
    {
        _rb.AddForceAtPosition(-_rb.position + (Vector2)_cam.ScreenToWorldPoint(Input.mousePosition) + centerOffset, 
            (Vector2)_cam.ScreenToWorldPoint(Input.mousePosition) + centerOffset);
        //_rb.MovePosition((Vector2)_cam.ScreenToWorldPoint(Input.mousePosition) + centerOffset);
        tempPrevPos = currentDragPosition;
        currentDragPosition = _rb.position;
        if (currentDragPosition != tempPrevPos)
        {
            previousDragPosition = tempPrevPos;
        }
    }

    private void OnMouseUp()
    {
        Vector2 direction = currentDragPosition - previousDragPosition;
        float distance = Vector2.Distance(previousDragPosition, currentDragPosition);
        //_rb.isKinematic = false;
        _rb.velocity = new Vector2(0f, 0f);
        _rb.AddForce(direction * distance * impulseForce, ForceMode2D.Impulse);
    }
}
