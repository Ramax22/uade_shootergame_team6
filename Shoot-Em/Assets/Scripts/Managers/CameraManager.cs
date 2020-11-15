using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform _entityToFollow;
    [SerializeField] float _zOffset;
    [SerializeField] float _xOffset;
    [SerializeField] float _yOffset;


    private void Update()
    {
        if (_entityToFollow == null) return;
        Vector3 pos = _entityToFollow.position;
        pos.x -= _xOffset;
        pos.z -= _zOffset;
        pos.y -= _yOffset;
        transform.position = pos;
    }
}