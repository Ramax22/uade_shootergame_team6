using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent
{
    float _maxHealth;
    float _actualHealth;
    public delegate void DeathDelegate();
    DeathDelegate _myDelegate;

    public HealthComponent(float initialhealth, DeathDelegate deathEvent)
    {
        _maxHealth = initialhealth;
        _actualHealth = _maxHealth;
        _myDelegate = deathEvent;
    }

    public void ChangeLife(float modifier)
    {
        _actualHealth -= modifier;
        _actualHealth = Mathf.Clamp(_actualHealth, 0, _maxHealth);

        if (_actualHealth == 0) _myDelegate.Invoke();
    }
}