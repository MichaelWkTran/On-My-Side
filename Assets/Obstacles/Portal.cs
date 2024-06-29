using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal m_gotoPortal;
    public LayerMask m_includeMask;

    public void WarpToThis(Rigidbody2D _rigidbody)
    {
        _rigidbody.position = transform.position;
    }

    void OnTriggerEnter2D(Collider2D _collision)
    {
        //Only teleport rigidbodies
        if (_collision.attachedRigidbody != null) return;

        //Only teleport objects part of the following layer mask
        if ((m_includeMask & (1 << _collision.gameObject.layer)) == 0) return;

        //Warp rigidbody
        m_gotoPortal.WarpToThis(_collision.attachedRigidbody);
    }
}
