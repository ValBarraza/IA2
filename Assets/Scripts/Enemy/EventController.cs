using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// esta clase maneja los triggers para pasar de un estado a otro basicamente, o sea aca deberian las funciones de LINQ que
// hacen que se triggeree un estado

public class EventController : MonoBehaviour {

    public event Action<Event> OnEvent = delegate { };
    public List<Collider> targets = new List<Collider>();

    void Update () {

        targets.Clear();
        foreach (var postarget in Physics.OverlapSphere(transform.position, 10f)) 
        {
            if (postarget.GetComponent<Entity>() && !postarget.GetComponent<Enemy>())
            targets.Add(postarget);
        }
        Debug.Log(GetComponent<Entity>().life);
        if (GetComponent<Enemy>().life <= 5) { OnEvent(Event.onFlee); Debug.Log("entro a flee"); }
        if (targets.Count > 0)
        {
            Debug.Log("debug1");
            var closetargets = targets.Where(x => Vector3.Distance(x.transform.position, transform.position) < 5f).ToList();
            if (closetargets.Count > 0) OnEvent(Event.onAttack);
            else OnEvent(Event.onChase);
        }
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
