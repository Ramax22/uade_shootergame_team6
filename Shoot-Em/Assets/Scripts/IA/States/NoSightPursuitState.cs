﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoSightPursuitState<T> : FSMState<T>
{
    //Sobreescribo la función Awake de la clase FSMState
    public override void Awake()
    {

    }

    //Sobreescribo la funcion de Execute de la clase FSMState
    public override void Execute()
    {
        Debug.Log("Execute - NoSight");
    }

    //Sobreescribo la funcion de Sleep de la clase FSMState
    public override void Sleep()
    {

    }
}