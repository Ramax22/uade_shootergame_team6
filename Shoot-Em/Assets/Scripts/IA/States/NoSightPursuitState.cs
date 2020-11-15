using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoSightPursuitState<T> : FSMState<T>
{
    //Variables
    Vector3 _lastPosition;
    EntityController _controller;
    Avoid _avoid;

    //Constructor
    public NoSightPursuitState(EntityController controller, LayerMask avoidableObstacles)
    {
        _controller = controller;
        _avoid = new Avoid(controller.transform, 2.5f, 0.65f, avoidableObstacles);
    }

    //Sobreescribo la función Awake de la clase FSMState
    public override void Awake()
    {
        _lastPosition = _controller.GetTargetLastPosition();
    }

    //Sobreescribo la funcion de Execute de la clase FSMState
    public override void Execute()
    {
        //Calculo la distancia
        Vector3 diff = _lastPosition - _controller.transform.position;
        float dist = diff.magnitude;
        _avoid.SetTargetByVector(_lastPosition);

        //Voy hacia el target
        //_lastPosition.y = _controller.transform.position.y;
        //_controller.transform.LookAt(_lastPosition);
        //_controller.Move(_controller.transform.forward);
        _controller.Move(_avoid.GetDir());
        _controller.Look(_lastPosition);

        //Chequeo si esta cerca. En caso de estarlo, cambia de estado
        if (dist < 4) _controller.ExecuteTree();
    }

    //Sobreescribo la funcion de Sleep de la clase FSMState
    public override void Sleep()
    {
        //Llamo a la funcion para resetear el sistema del sight
        _controller.ResetSight();
    }
}