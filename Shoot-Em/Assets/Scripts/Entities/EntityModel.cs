using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityModel : MonoBehaviour
{
    [SerializeField] GunController _gunController;
    [SerializeField] public float _speed;
    [SerializeField] public float _ogspeed;
    [SerializeField] float _initialhealth;

    Rigidbody _rb;
    HealthComponent _health;

    private void Start()
    {
        _ogspeed = _speed;
        _rb = GetComponent<Rigidbody>();
        _health = new HealthComponent(_initialhealth, Death);
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

    public void Shoot()
    {
        _gunController.Shoot();
    }

    public void ChangeLife(float amount)
    {
        _health.ChangeLife(amount);
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    //Funciones de interacciones con componentes
    public float GetHealth()
    {
        return _health.GetLife();
    }
}