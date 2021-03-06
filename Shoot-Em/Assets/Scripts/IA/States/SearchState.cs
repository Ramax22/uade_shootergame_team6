﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchState<T> : FSMState<T>
{
    //Variables
    List<Node> _nodes;
    Transform _body;
    LayerMask _obstaclesMask;
    Node _nearestNode;
    Node _destiny;
    List<Node> _path;
    int _pointer;
    EntityController _controller;
    Avoid _avoid;
    LayerMask _avoidableObjects;

    //Pathfinding
    BFS<Node> _bfs = new BFS<Node>();

    //Constructor
    public SearchState(List<Node> nodes, Transform body, LayerMask obstacles, EntityController controller, Transform targetTransform, LayerMask avoidableObjects)
    {
        //Asigno las variables
        _nodes = nodes;
        _body = body;
        _obstaclesMask = obstacles;
        _controller = controller;
        _avoidableObjects = avoidableObjects;
        _avoid = new Avoid(_controller.transform, 2.5f, 0.8f, _avoidableObjects);
    }

    //Sobreescribo la función Awake de la clase FSMState
    public override void Awake()
    {
        //Busco mi nodo mas cercano, y tam
        _nearestNode = FindNearestNode();
        _destiny = GetRandomNode();

        //Agarro un camino
        _path = _bfs.Run(_nearestNode, Satisfies, GetNeighbours);

        //Inicializo el pointer
        _pointer = 0;
    }

    //Sobreescribo la funcion de Execute de la clase FSMState
    public override void Execute()
    {
        if (_pointer < _path.Count)
        {
            //Agarro la posicion del vector
            _avoid.SetTarget(_path[_pointer].transform);
            _controller.Move(_avoid.GetDir());
            _controller.Look(_path[_pointer].transform.position);

            Vector3 target = _path[_pointer].transform.position;

            Vector3 diff = target - _body.transform.position;
            float distance = diff.magnitude;

            if (distance <= 1) _pointer++;
        } 
        else
        {
            _controller.ExecuteTreeFromSleep();
        }
    }

    //Sobreescribo la funcion de Sleep de la clase FSMState
    public override void Sleep()
    {

    }

    #region ~~~ FUNCTIONS ~~~
    //Busca el nodo mas cercano donde tengamos vision directa
    Node FindNearestNode()
    {
        //Declaro variables
        float currentDistance = float.PositiveInfinity;
        Node nearestNode = null;

        //Recorro todos los nodos
        foreach (var item in _nodes)
        {
            //Calculo la distancia
            Vector3 diff = item.transform.position - _body.transform.position;
            float dist = diff.magnitude;
            //Verifico si no hay obstaculos entre medio y si la distancia es menor a la distancia del nodo anterior
            bool isFree = Physics.Raycast(_body.transform.position, diff.normalized, dist, _obstaclesMask);
            if (dist < currentDistance && !isFree) {
                currentDistance = dist;
                nearestNode = item; 
            }
        }
        return nearestNode;
    }

    //Retorna un nodo aleatorio de la lista
    Node GetRandomNode()
    {
        return _nodes[Random.Range(0, _nodes.Count - 1)];
    }

    //Funcion que determina si un nodo me satisface o no
    bool Satisfies(Node curr)
    {
        return curr == _destiny;
    }

    //Funcion que retorna los vecinos de un nodo
    List<Node> GetNeighbours(Node curr)
    {
        var list = new List<Node>();
        for (int i = 0; i < curr.neightbourds.Count; i++)
        {
            if (curr.neightbourds[i].isTrap) continue;
            list.Add(curr.neightbourds[i]);
        }
        return list;
    }
    #endregion
}