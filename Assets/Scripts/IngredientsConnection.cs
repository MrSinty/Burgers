using System.Collections;
using UnityEngine;
using Ingredients;
using System;

public class IngredientsConnection : MonoBehaviour
{
    Collider2D _col = null;
    Collider2D _connectionTrigger = null;
    Collider2D connectedCollider = null;
    FixedJoint2D _joint = null;
    FixedJoint2D _masterJoint = null;
    public FixedJoint2D MasterJoint { 
        get { return _masterJoint; }
        set { _masterJoint = value; }
    }

    bool _connectedToSomeone = false;
    bool _containingSomeone = false;
    public bool ConnectedToSomeone {
        get => _connectedToSomeone;
        set { _connectedToSomeone = value; }
    }
    bool _removing = false;

    private void OnEnable()
    {
        CollisionDetection.colliderEnter += CollisionEnter;

        _joint = GetComponent<FixedJoint2D>();
        _joint.enabled = false;
        Collider2D[] colls = GetComponents<Collider2D>();
        _col = colls[0];
        _connectionTrigger = colls[1];
    }

    private void Update()
    {
        /*if (Input.GetButtonDown("Fire1"))
        {
            int layerObject = 8;
            Vector2 ray = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(ray, ray, layerObject);
            if (hit.collider != null && hit.collider == _connectionTrigger)
            {
                if (hit.collider.CompareTag("Draggable") && connectedCollider != hit.collider)
                {
                    connectedCollider = hit.collider;
                    _ = StartCoroutine(nameof(ConnectionEvent));
                }
            }
        }*/
    }

    private void OnMouseOver()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (_joint != null)
            {
                _ = StartCoroutine(nameof(DisconnectiongEvent));
            }
        }
    }

    private void RemoveConnection()
    {
        if (_removing)
        {
            Debug.Log("Disconnected");
            if (_joint.connectedBody != null)
                _joint.connectedBody.GetComponent<IngredientsConnection>().ConnectedToSomeone = false;
            if (connectedCollider != null)
                connectedCollider.GetComponent<IngredientsConnection>().MasterJoint = null;
            _joint.connectedBody = null;
            _joint.enabled = false;
            _containingSomeone = false;
            connectedCollider = null;
        }
    }

    private void MakeConnection()
    {
        Debug.Log("Connected!");
        _joint.enabled = true;
        _joint.connectedBody = connectedCollider.attachedRigidbody;
        _joint.connectedAnchor = transform.InverseTransformPoint(_joint.connectedBody.position);
        connectedCollider.GetComponent<IngredientsConnection>().ConnectedToSomeone = true;
        connectedCollider.GetComponent<IngredientsConnection>().MasterJoint = _joint;
        _containingSomeone = true;
    }

    private IEnumerator DisconnectiongEvent() 
    {
        _removing = true;
        RemoveConnection();
        yield return new WaitForSeconds(3f);
        Debug.Log("Stop disconnecting!");
        _removing = false;
    }

    private IEnumerator ConnectionEvent()
    {
        if (!_removing)
        {
            Debug.Log("Connecting");
            yield return new WaitForSeconds(2f);
            MakeConnection();
        }
    }
    private void CollisionEnter(Collider2D collision, Side side)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Draggable") && connectedCollider != collision)
        {
            connectedCollider = collision;
            _ = StartCoroutine(nameof(ConnectionEvent));
        }
    }
}
