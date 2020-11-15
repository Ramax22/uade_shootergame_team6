﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avoid : ISteeringBehaviour
{
    Transform _npc;
    Transform _target;
    Vector3 _targetV3;
    float _radius;
    LayerMask _mask;
    float _avoidWeight;
    public Avoid(Transform npc, float radius, float avoidWeight, LayerMask mask)
    {
        _mask = mask;
        _npc = npc;
        _radius = radius;
        _avoidWeight = avoidWeight;
    }

    public void SetTarget(Transform target) { _target = target; }
    public void SetTargetByVector(Vector3 target) { _targetV3 = target; }

    public Vector3 GetDir()
    {
        //Obtenemos los obstaculos
        Collider[] obstacles = Physics.OverlapSphere(_npc.position, _radius, _mask);
        Transform obsSave = null;
        var count = obstacles.Length;

        //Recorremos los obstaculos y determinos cual es el mas cercano
        for (int i = 0; i < count; i++)
        {
            var currObs = obstacles[i].transform;
            if (obsSave == null)
            {
                obsSave = currObs;
            }
            else if (Vector3.Distance(_npc.position, obsSave.position) > Vector3.Distance(_npc.position, currObs.position))
            {
                obsSave = currObs;
            }
        }
        Vector3 target;

        if (_target == null) target = _targetV3;
        else target = _target.position;

        Vector3 dirToTarget = (target - _npc.position).normalized;

        //Si hay un obstaculo, le agregamos a nuestra direccion una direccion de esquive
        if (obsSave != null)
        {
            Vector3 dirObsToNpc = (_npc.position - obsSave.position).normalized * _avoidWeight;
            dirToTarget += dirObsToNpc;
        }
        //retornamos la direccion final
        return dirToTarget.normalized;
    }
}
