﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// esta clase maneja los triggers para pasar de un estado a otro basicamente, o sea aca deberian las funciones de LINQ que
// hacen que se triggeree un estado

public class EventController : MonoBehaviour {

    public event Action<Event> OnEvent = delegate { };
    public GameObject triggertest;

    void Update () {
        if (Vector3.Distance(transform.position, triggertest.transform.position) < 1) OnEvent(Event.onAttack);
        else if (Vector3.Distance(transform.position, triggertest.transform.position) < 3) OnEvent(Event.onChase);
        else if (Vector3.Distance(transform.position, triggertest.transform.position) < 5) OnEvent(Event.onFlee);
        else OnEvent(Event.onPatrol);
    }
}
public enum Event // eventos para los estados ( si fuera un jugador usaria inputs )
{
    onPatrol,
    onFlee,
    onAttack,
    onChase
}