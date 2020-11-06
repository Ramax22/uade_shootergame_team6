using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] BulletController _bullet;
    [SerializeField] Transform _shootPoint;
    [SerializeField] float _shootingCooldownTime;
    [SerializeField] LayerMask target;
    float _actualCooldown;
    bool _canShoot;

    private void Awake()
    {
        _actualCooldown = _shootingCooldownTime;
        _canShoot = false;
    }

    public void Shoot()
    {
        if (_canShoot)
        {
            GameObject.Instantiate(_bullet, _shootPoint.position, _shootPoint.rotation);
            _bullet.target = target;
            _canShoot = false;
            _actualCooldown = 0;
        }
    }

    private void Update()
    {
        _actualCooldown += Time.deltaTime;

        if (_actualCooldown >= _shootingCooldownTime) _canShoot = true;
    }
}