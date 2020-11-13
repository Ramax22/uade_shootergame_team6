using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    //Referencia a la FSM
    FSM<string> _fsm;

    //Keys de los estados de la fsm
    const string _idle = "idle";
    const string _search = "search";
    const string _sightPursuit = "sightPursuit";
    const string _noSightPursuit = "noSightPursuit";
    const string _escape = "escape";
    const string _attack = "attack";
    const string _sleep = "sleep";

    //Question Nodes
    QuestionNode _sawTarget;
    QuestionNode _isSeeingTarget;
    QuestionNode _hasPointToGo;
    QuestionNode _isInDistance;

    //Roulette
    Roulette<string> _actionRoulette;
    Dictionary<string, int> _statesRoulette;

    //Variables
    Sight _sight; //Referencia al componente sight
    [SerializeField] List<Node> _list; //Lista de todos los nodos del mapa
    EntityModel _model;
    [SerializeField] LayerMask _mask; //mask de los obstaculos
    [SerializeField] Transform _entityTarget;
    [SerializeField] EntityModel _targetModel;

    //Variables de la Pseudo-blackboard
    Vector3 _targetLastPos;

    private void Awake()
    {
        _model = GetComponent<EntityModel>(); //Agarro el componente EntityModel
        _sight = GetComponent<Sight>(); //Agarro el componente sight

        InitFSM(); //Init de todo lo relacionado con la FSM
        InitDesicionTree(); //Init del Desicion Tree
        InitRoulette(); //Init de la Roulette
    }

    private void Update()
    {
        _fsm.OnUpdate();
    }

    #region ~~~ FSM ~~~
    //Funcion que inicializa todo lo relacionado con la FSM en la IA
    void InitFSM()
    {
        //Inicializo la FSM y creo los estados
        _fsm = new FSM<string>();

        //Creo los distintos estados 
        IdleState<string> idleState = new IdleState<string>(this);
        SearchState<string> searchState = new SearchState<string>(_list, transform, _mask, this);
        SightPursuitState<string> sightPursuitState = new SightPursuitState<string>(_entityTarget, this, _mask);
        NoSightPursuitState<string> noSightPursuitState = new NoSightPursuitState<string>(this);
        EscapeState<string> escapeState = new EscapeState<string>();
        AttackState<string> attackState = new AttackState<string>(_targetModel, this);
        SleepState<string> sleepState = new SleepState<string>(this);

        //Creo las transiciones de cada estado
        idleState.AddTransition(_search, searchState);
        idleState.AddTransition(_sightPursuit, sightPursuitState);
        idleState.AddTransition(_noSightPursuit, noSightPursuitState);
        idleState.AddTransition(_escape, escapeState);
        idleState.AddTransition(_attack, attackState);
        idleState.AddTransition(_sleep, sleepState);

        searchState.AddTransition(_idle, idleState);
        searchState.AddTransition(_sightPursuit, sightPursuitState);
        searchState.AddTransition(_noSightPursuit, noSightPursuitState);
        searchState.AddTransition(_escape, escapeState);
        searchState.AddTransition(_attack, attackState);
        searchState.AddTransition(_sleep, sleepState);

        sightPursuitState.AddTransition(_idle, idleState);
        sightPursuitState.AddTransition(_search, searchState);
        sightPursuitState.AddTransition(_noSightPursuit, noSightPursuitState);
        sightPursuitState.AddTransition(_attack, attackState);
        sightPursuitState.AddTransition(_escape, escapeState);
        sightPursuitState.AddTransition(_sleep, sleepState);

        noSightPursuitState.AddTransition(_idle, idleState);
        noSightPursuitState.AddTransition(_sightPursuit, sightPursuitState);
        noSightPursuitState.AddTransition(_search, searchState);
        noSightPursuitState.AddTransition(_escape, escapeState);
        noSightPursuitState.AddTransition(_attack, attackState);
        noSightPursuitState.AddTransition(_sleep, sleepState);

        attackState.AddTransition(_idle, idleState);
        attackState.AddTransition(_search, searchState);
        attackState.AddTransition(_sightPursuit, sightPursuitState);
        attackState.AddTransition(_noSightPursuit, noSightPursuitState);
        attackState.AddTransition(_escape, escapeState);
        attackState.AddTransition(_sleep, sleepState);

        sleepState.AddTransition(_idle, idleState);
        sleepState.AddTransition(_search, searchState);
        sleepState.AddTransition(_escape, escapeState);
        sleepState.AddTransition(_attack, attackState);
        sleepState.AddTransition(_sightPursuit, sightPursuitState);
        sleepState.AddTransition(_noSightPursuit, noSightPursuitState);

        //Inicializo la FSM
        //_fsm.SetInitialState(idleState);
        //_fsm.SetInitialState(searchState);
        _fsm.SetInitialState(sightPursuitState);
    }

    //Funciones para transiciones
    public void GoToIdleState() { _fsm.Transition(_idle); } //Ir a Idle
    public void GoToSearchState() { _fsm.Transition(_search); } //Ir a Search
    public void GoToSightPursuitState() { _fsm.Transition(_sightPursuit); } //Ir a Sight Pursuit
    public void GoToNoSightPursuitState() { _fsm.Transition(_noSightPursuit); } //Ir a No Sight Pursuit
    public void GoToEscapeState() { _fsm.Transition(_escape); } //Ir a Escape
    public void GoToAttackState() { _fsm.Transition(_attack); } //Ir al estado de Ataque
    public void GoToRandomState() { _fsm.Transition(ExecuteRoulette()); } //Ingresa a un estado al azar
    #endregion

    #region ~~~ Desicion Tree ~~~
    //Inicializa la desicion tree
    void InitDesicionTree()
    {
        //Creo los Action Nodes
        ActionNode _sightPursuitNode = new ActionNode(GoToSightPursuitState);
        ActionNode _noSightPursuitNode = new ActionNode(GoToNoSightPursuitState);
        ActionNode _randomState = new ActionNode(GoToRandomState);
        ActionNode _attackNode = new ActionNode(GoToAttackState);

        _isInDistance = new QuestionNode(IsInAttackDistance, _attackNode, _sightPursuitNode);
        _hasPointToGo = new QuestionNode(CanGoToLastPoint, _noSightPursuitNode, _randomState);
        _isSeeingTarget = new QuestionNode(_sight.SawTarget, _isInDistance, _hasPointToGo);
        _sawTarget = new QuestionNode(_sight.SawTargetOnce, _isSeeingTarget, _randomState);
    }

    //Funcion para ejecutar el arbol de desiciones
    public void ExecuteTree()
    {
        _sawTarget.Execute();
    }
    #endregion

    #region ~~~ Roulette ~~~
    //Inicializa la roulette
    void InitRoulette()
    {
        //Inicializo la roulette
        _actionRoulette = new Roulette<string>();

        //Diccionario de todos los estados para la roulette
        _statesRoulette = new Dictionary<string, int>();
        _statesRoulette.Add(_idle, 50);
        _statesRoulette.Add(_search, 30);
        _statesRoulette.Add(_sleep, 20);
    }

    //Funcion para ejecutar el roulette
    string ExecuteRoulette()
    {
        return _actionRoulette.Run(_statesRoulette);
    }
    #endregion

    #region  ~~~ AI FUNCTIONS ~~~
    //Funcion que determina si la entidad puede ir al ultimo punto donde vio a su objetivo
    bool CanGoToLastPoint()
    {
        float distance = Vector3.Distance(_sight.LastPoint(), transform.position);
        return distance > 2;
    }

    //Funcion que determina si esta o no a distancia de ataque
    bool IsInAttackDistance()
    {
        Vector3 diff = _entityTarget.position - transform.position;
        float dist = diff.magnitude;
        return dist < 2;
    }
    #endregion

    #region ~~~ MODEL FUNCTIONS ~~~
    public void Move(Vector3 dir)
    {
        _model.Move(dir);
    }
    #endregion

    #region ~~~ PSEUDO-BLACKBOARD ~~~
    public void SaveTargetLastPosition(Vector3 position)
    {
        _targetLastPos = position;
    }

    public Vector3 GetTargetLastPosition()
    {
        return _targetLastPos;
    }

    public void ResetSight()
    {
        _sight.ResetSawTargetOnce();
    }
    #endregion
}