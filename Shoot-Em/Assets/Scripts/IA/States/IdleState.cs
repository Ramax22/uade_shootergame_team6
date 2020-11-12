using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class IdleState<T> : FSMState<T>
{
    //variables
    float _timer;
    EntityController _controller;

    //Constructor
    public IdleState(EntityController controller)
    {
        _controller = controller;
    }

    //Sobreescribo la función Awake de la clase FSMState
    public override void Awake()
    {
        _timer = Random.Range(1, 5);
    }

    //Sobreescribo la funcion de Execute de la clase FSMState
    public override void Execute()
    {
        //Resto el timer
        if (_timer > 0) _timer -= Time.deltaTime;
        else _controller.ExecuteTree();
    }

    //Sobreescribo la funcion de Sleep de la clase FSMState
    public override void Sleep()
    {
        _timer = Random.Range(1, 5);
    }
}