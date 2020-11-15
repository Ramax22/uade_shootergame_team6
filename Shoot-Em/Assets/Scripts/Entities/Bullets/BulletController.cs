using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _timeToDispawn;
    [SerializeField] float _damage;

    private void Awake()
    {
        Destroy(gameObject, _timeToDispawn);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            other.transform.GetComponent<EntityModel>().ChangeLife(_damage);
        }
        Destroy(gameObject);
    }
}