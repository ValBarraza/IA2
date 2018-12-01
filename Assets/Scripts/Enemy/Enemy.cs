using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public EventController controller;
    private EventFSM<Event> stateMachine;

    private void Start()
    {
        // 1 - Creacion de los estados
        var patrol = new State<Event>("PATROL");
        var flee = new State<Event>("FLEE");
        var chase = new State<Event>("CHASE");
        var attack = new State<Event>("ATTACK");

        // 2 - Seteo de las transiciones
        patrol.AddTransition(Event.onAttack, attack);

        attack.AddTransition(Event.onPatrol, patrol);
        attack.AddTransition(Event.onChase, chase);

        chase.AddTransition(Event.onAttack, attack);
        chase.AddTransition(Event.onFlee, flee);

        flee.AddTransition(Event.onChase, chase);

        // 3 - Seteo de los estados
        patrol.OnEnter += () => Debug.Log("Entre en PATROL");
        attack.OnEnter += () => Debug.Log("Entre en ATTACK");
        chase.OnEnter += () => Debug.Log("Entre en CHASE");
        flee.OnEnter += () => Debug.Log("Entre en FLEE");

        // 4 - Creacion de la FSM y asignacion del controlador de eventos/inputs
        stateMachine = new EventFSM<Event>(patrol);
        controller.OnEvent += evento => stateMachine.Feed(evento);
    }
    private void Update() { stateMachine.Update(); }
}
