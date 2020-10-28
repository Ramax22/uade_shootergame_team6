using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityModel : MonoBehaviour
{
    [SerializeField] float _speed;
    Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    //FUNCIONES DE ACCIONES
    public void Move(Vector3 dir)
    {
        var moveVelocity = dir * _speed;
        _rb.velocity = moveVelocity;
    }

    public void Look(Vector3 point)
    {
        transform.LookAt(new Vector3(point.x, transform.position.y, point.z));
    }
}