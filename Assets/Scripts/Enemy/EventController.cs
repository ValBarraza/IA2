using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// esta clase maneja los triggers para pasar de un estado a otro basicamente, o sea aca deberian las funciones de LINQ que
// hacen que se triggeree un estado

public class EventController : MonoBehaviour {

    public event Action<Event> OnEvent = delegate { };
    //public GameObject triggertest;
    public List<Collider> targets = new List<Collider>();

    void Update () {

        targets.Clear();
        //targets = new List<Collider>();
        foreach (var postarget in Physics.OverlapSphere(transform.position, 10f)) 
        {
            if (postarget.GetComponent<Entity>() && !postarget.GetComponent<Enemy>())
            targets.Add(postarget);
        }
        if (GetComponent<Entity>().life <= GetComponent<Entity>().life / 4) OnEvent(Event.onFlee); 
        if (targets.Count > 0)
        {
            Debug.Log("debug1");
            var closetargets = targets.Where(x => Vector3.Distance(x.transform.position, transform.position) < 5f).ToList();
            if (closetargets.Count > 0) OnEvent(Event.onAttack);
            else OnEvent(Event.onChase);
        }
        else OnEvent(Event.onPatrol);
        //if (Vector3.Distance(transform.position, triggertest.transform.position) <= 10) { OnEvent(Event.onChase); Debug.Log("chase"); }
        //if (targets.Count > 0 && Vector3.Distance(transform.position, triggertest.transform.position) <= 5) OnEvent(Event.onAttack);
        //else 
        //else if (Vector3.Distance(transform.position, triggertest.transform.position) < 5) OnEvent(Event.onFlee);
        
    }
}
public enum Event // eventos para los estados ( si fuera un jugador usaria inputs )
{
    onPatrol,
    onFlee,
    onAttack,
    onChase
}
