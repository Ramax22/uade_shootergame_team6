using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform _entityToFollow;
    [SerializeField] float _zOffset;
    [SerializeField] float _xOffset;


    private void Update()
    {
        if (_entityToFollow == null) return;
        Vector3 pos = _entityToFollow.position;
        pos.x -= _xOffset;
        pos.z -= _zOffset;
        pos.y = transform.position.y;
        transform.position = pos;
    }
}