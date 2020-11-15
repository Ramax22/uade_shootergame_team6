using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState<T> : FSMState<T>
{
    //Variables
    EntityModel _targetModel;
    EntityController _entityController;
    const float _attackCooldown = 3f;
    const float _damage = 10f;
    float _currentCooldown;

    //Constructor
    public AttackState(EntityModel targetModel, EntityController entityController)
    {
        _targetModel = targetModel;
        _entityController = entityController;
    }

    //Sobreescribo la función Awake de la clase FSMState
    public override void Awake()
    {
        _currentCooldown = 3;
    }

    //Sobreescribo la funcion de Execute de la clase FSMState
    public override void Execute()
    {
        if (_targetModel == null)
        {
            _entityController.ResetSight();
            _entityController.ExecuteTree();
            return;
        }

        //Sumo al cooldown de ataque
        _currentCooldown += Time.deltaTime;

        //Si puedo atacar, ataca
        if (_currentCooldown >= _attackCooldown)
        {
            _targetModel.ChangeLife(_damage);
            _currentCooldown = 0;
        }

        //Calculo la distancia entre la entidad y el target
        Vector3 diff = _entityController.transform.position - _targetModel.transform.position;
        float dist = diff.magnitude;

        //Si se alejó, cambio de estado
        if (dist > 2) _entityController.ExecuteTree();
    }

    //Sobreescribo la funcion de Sleep de la clase FSMState
    public override void Sleep()
    {
        _currentCooldown = 3;
    }
}