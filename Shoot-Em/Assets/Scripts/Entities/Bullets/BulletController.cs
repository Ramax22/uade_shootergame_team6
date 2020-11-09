using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _timeToDispawn;

    private void Awake()
    {
        Destroy(gameObject, _timeToDispawn);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }
}