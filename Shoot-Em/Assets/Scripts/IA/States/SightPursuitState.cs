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
    //Contructor 
    public SightPursuitState(Transform target, EntityController controller, LayerMask obstacleMask)
    {
        _target = target;
        _controller = controller;
        _obstacleMask = obstacleMask;
    }

    //Sobreescribo la función Awake de la clase FSMState
    public override void Awake()
    {

    }

    //Sobreescribo la funcion de Execute de la clase FSMState
    public override void Execute()
    {
        //Le digo que mire hacia el target y que vaya hacia él
        Vector3 positionToGo = _target.position;
        positionToGo.y = _controller.transform.position.y;

        _controller.transform.LookAt(positionToGo);

        Vector3 move = _controller.transform.forward;

        _controller.Move(move);

        //Me guardo la última posición donde ví al target
        _targetLastPosition = positionToGo;

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
