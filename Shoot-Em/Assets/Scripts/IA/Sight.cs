using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    //Variables
    [SerializeField] Transform _target;
    [SerializeField] float _range;
    [SerializeField] float _angle;
    [SerializeField] LayerMask _mask;
    EntityController _controller;
    bool _sawTargetOnce; //esta varaible determina si ya vio al player en su tiempo de vida
    bool _sawTarget; //Esta variable determina si esta viendo al player en este preciso momento
    Vector3 _lastPoint; //Esta variable se usa para que el codigo guarde donde fue la última vez que vio al jugador
    bool _alreadyCalled;

    private void Awake()
    {
        _sawTarget = false;
        _sawTargetOnce = false;
        _lastPoint = Vector3.zero;
        _controller = GetComponent<EntityController>();
        _alreadyCalled = false;
    }

    private void Update()
    {
        IsInSight();
    }

    #region ~~~ CHEQUEOS ~~~
    //Funcion que chequea si el target esta a la vista
    void IsInSight()
    {
        if (_target == null) return;

        _sawTarget = false;
        if (!CheckDistance()) return; //Chequeo distancia
        if (!CheckAngle()) return; //Chequeo angulo
        if (!CheckObstacles()) return;//Chequeo si hay obstaculos o no


        
        _sawTarget = true;
        _sawTargetOnce = true;
        _lastPoint = _target.transform.position;
        if (!_alreadyCalled)
        {
            _controller.ExecuteTree();
            _alreadyCalled = true;
        }
    }

    //Funcion que chequea que el target este en la distancia de vision de la entidad
    bool CheckDistance()
    {
        //Calculamos la distancia entre esta entidad y el objetivo
        Vector3 diff = _target.position - transform.position;
        float distance = diff.magnitude;
        if (distance > _range) return false;
        else return true;
    }

    //Funcion que calcula si esta en el angulo de vision
    bool CheckAngle()
    {
        //Calculamos si esta en el angulo de vision de la entidad
        Vector3 diff = _target.position - transform.position;
        float angleToTarget = Vector3.Angle(transform.forward, diff.normalized);
        if (angleToTarget > _angle / 2) return false;
        else return true;
    }

    //Funcion que chequea si no hay obstaculos
    bool CheckObstacles()
    {
        //Calculamos que no haya un osbtaculo entre la entidad y el objetivo
        Vector3 diff = _target.position - transform.position;
        float distance = diff.magnitude;
        if (Physics.Raycast(transform.position, diff.normalized, distance, _mask)) return false;
        else return true;
    }
    #endregion

    #region ~~~ FUNCIONES ~~~
    //Funcion que dice si esta viendo o no al target
    public bool SawTarget()
    {
        return _sawTarget;
    }

    //Funcion que dice si ya vio al target en algun momento o no
    public bool SawTargetOnce()
    {
        return _sawTargetOnce;
    }

    public void ResetSawTargetOnce()
    {
        _sawTargetOnce = false;
        _alreadyCalled = false;
    }

    //Funcion que devuelve la ultima posicion donde vio al target
    public Vector3 LastPoint()
    {
        return _lastPoint;
    }

    //Funcion que es llamada para resetear los datos de la entidad, esto se llama
    //cuando la misma ya reviso el ultimo punto donde esta el jugador, y no lo encuentra.
    public void ResetSight()
    {
        _lastPoint = Vector3.zero;
        _sawTarget = false;
        _sawTargetOnce = false;
    }

    //Funcion que establece un target
    public void SetTarget(Transform target)
    {
        _target = target;
    }
    #endregion
}