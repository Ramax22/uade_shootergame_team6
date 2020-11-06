using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _timeToDispawn;
    [SerializeField] private float distance;
    [SerializeField] private float damage;
    [SerializeField] public LayerMask layerMask;

    private void Awake()
    {
        Destroy(gameObject, _timeToDispawn);

    }

    private void Update()
    {

        RaycastHit hitInfo = Physics.Raycast(transform.position, Vector3.forward, distance, layerMask);
        if (hitInfo.collider != null)
        {
            Debug.Log("Found Collider");

            if (hitInfo.collider.CompareTag("Enemy"))
            {
                Debug.Log("Found Enemy");
                hitInfo.collider.GetComponent<EntityModel>().ChangeLife(damage);
                Destroy(gameObject);
                Debug.Log("Hit Target");
            }

            if (hitInfo.collider.CompareTag("Player"))
            {
                Debug.Log("Found Player");
                hitInfo.collider.GetComponent<EntityModel>().ChangeLife(damage);
                Destroy(gameObject);
                Debug.Log("Hit Target");
            }
        }
        transform.Translate(Vector2.right * (_speed * Time.deltaTime));
    }
}