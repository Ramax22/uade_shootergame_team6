using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeState<T> : FSMState<T>
{
    //Variables
    Evade _evade;
    EntityController _controller;
    Transform _target;
    Rigidbody _rb;

    //Constructor
    public EscapeState(EntityController controller, Transform targetTransform, Rigidbody rb)
    {
        _controller = controller;
        _target = targetTransform;
        _rb = rb;
        _evade = new Evade(_controller.transform, _target, _rb, 0.5f);
    }

    //Sobreescribo la función Awake de la clase FSMState
    public override void Awake()
    {

    }

    //Sobreescribo la funcion de Execute de la clase FSMState
    public override void Execute()
    {
        _controller.Move(_evade.GetDir());
    }

    //Sobreescribo la funcion de Sleep de la clase FSMState
    public override void Sleep()
    {

    }
}