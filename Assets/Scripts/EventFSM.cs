using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventFSM<TFeed> { // es una clase generica ( tfeed es el valor generico )

    public State<TFeed> current; // estado actual
    public void Update() // en el update llama al update del estado actual
    {
        current.OnUpdate();
    } 
    public void Feed(TFeed feed)  // esta alimentando al pescadito 
    {
        var next = current.GetTransition(feed); // se fija si el estado transiciona a otro y hace la transicion
        if(next != null)
        {
            current.OnExit();
            current = next;
            current.OnEnter();
        }
    }
    public EventFSM (State<TFeed> initialState) // CONSTRUCTOR - asigna el estado inicial y lo inicia
    {
        current = initialState;
        current.OnEnter();
    }
}
public class State<TFeed> { // estos son los estados, es una clase adentro de la State Machine

    public string name;
    public Action OnEnter = delegate { }; // que hace cuando entra al estado
    public Action OnUpdate = delegate { }; // que hace durante el estado
    public Action OnExit = delegate { }; // que hace al salir del estado

    public Dictionary<TFeed, State<TFeed>> transitions = new Dictionary<TFeed, State<TFeed>>(); // diccionario de transiciones

    public void AddTransition(TFeed feed, State<TFeed> next)
    {
        transitions[feed] = next; // para agregar una transicion, conecta un "feed" con otro "feed"
    }
    public State<TFeed> GetTransition(TFeed feed) // se fija si existe una transicion desde ese "feed"
    {
        if (transitions.ContainsKey(feed)) return transitions[feed];
        return null;
    }
    public State(string name) // CONSTRUCTOR - le pone un nombre al estado
    {
        this.name = name;
    }
}