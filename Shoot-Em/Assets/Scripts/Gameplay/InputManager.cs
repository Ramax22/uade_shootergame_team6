﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    EntityModel _model;
    Camera main;

    private void Awake()
    {
        _model = GetComponent<EntityModel>();
        main = Camera.main;
    }

    private void FixedUpdate()
    {
        _model.Move(GetMovementInput());
        _model.Look(GetMousePosition());

        if (Input.GetButton("Fire1")) _model.Shoot();
    }

    Vector3 GetMovementInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _model._speed = _model._ogspeed * 1.5f;
        }

        else
        {
            _model._speed = _model._ogspeed;
        }

        // es un getAxisRaw para que la reaccion sea mas rapida
        var xDir = Input.GetAxisRaw("Horizontal");
        var zDir = Input.GetAxisRaw("Vertical");
        return new Vector3(xDir, 0f, zDir);
    }

    Vector3 GetMousePosition()
    {
        Ray cameraRay = main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (ground.Raycast(cameraRay, out rayLength))
        {
            Vector3 point = cameraRay.GetPoint(rayLength);
            return point;
        }

        return Vector3.zero;
    }
}