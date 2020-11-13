using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoSightPursuitState<T> : FSMState<T>
{
    //Variables
    Vector3 _lastPosition;
    EntityController _controller;

    //Constructor
    public NoSightPursuitState(EntityController controller)
    {
        _controller = controller;
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
        UnityEngine.Debug.Log("Target last pos in no sight is " + _lastPosition);

        //Voy hacia el target
        _lastPosition.y = _controller.transform.position.y;
        _controller.transform.LookAt(_lastPosition);
        _controller.Move(_controller.transform.forward);

        //Chequeo si esta cerca. En caso de estarlo, cambia de estado
        if (dist < 2) _controller.ExecuteTree();
    }

    //Sobreescribo la funcion de Sleep de la clase FSMState
    public override void Sleep()
    {
        //Llamo a la funcion para resetear el sistema del sight
        _controller.ResetSight();
    }
}