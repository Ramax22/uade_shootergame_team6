using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightPursuitState<T> : FSMState<T>
{
    //Variables
    Transform _target;
    EntityController _controller;
    Vector3 _targetLastPosition;
    LayerMask _obstacleMask;
    Pursit _pursuit;
    Rigidbody _targetRb;

    //Contructor 
    public SightPursuitState(Transform target, EntityController controller, LayerMask obstacleMask)
    {
        _target = target;
        _controller = controller;
        _obstacleMask = obstacleMask;
        _targetRb = target.GetComponent<Rigidbody>();
        _pursuit = new Pursit(_controller.transform, _target, _targetRb, 0.5f);
    }

    //Sobreescribo la función Awake de la clase FSMState
    public override void Awake()
    {

    }

    //Sobreescribo la funcion de Execute de la clase FSMState
    public override void Execute()
    {

        Vector3 positionToGo = _pursuit.GetDir();
        _controller.Move(positionToGo);
        _controller.Look(_target.position);

        //Me guardo la última posición donde ví al target
        _targetLastPosition = _target.position;

        //Calculo la distancia
        Vector3 diff = _target.position - _controller.transform.position;
        float dist = diff.magnitude;

        //Chequeo si no hay algo que corte la vision entre la entidad y el objetivo
        bool obstacle = Physics.Raycast(_controller.transform.position, diff.normalized, dist, _obstacleMask);

        //Chequeo si esta en la distancia minima, o si su vision esta siendo obstruida por algun elemento del enviroment
        if (obstacle || dist < 2) _controller.ExecuteTree();
    }

    //Sobreescribo la funcion de Sleep de la clase FSMState
    public override void Sleep()
    {
        _controller.SaveTargetLastPosition(_targetLastPosition);
    }
}
