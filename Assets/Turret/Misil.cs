using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Misil : Entity {

    public GameObject target;
    public Turret owner;

    private void Awake()
    {
        life = 1;
        speed = 4.5f;
    }

    // Update is called once per frame
    void Update()
    {

        if (life <= 0 || !target) Die(); // y tambien se destruye si algo le saco vida
        transform.forward = (target.transform.position - transform.position).normalized;
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;

        var damagables = new List<Collider>();
        foreach (var damagable in Physics.OverlapSphere(transform.position, 0.6f)) // agarra todo en un cierto rango
        {
            if (damagable != owner.gameObject && damagable.GetComponent<Entity>()) // filtra para que sean Entities y no sea su torreta ( owner )
                damagables.Add(damagable); // si paso los filtros lo agrega a la lista
        }
        if (damagables.Count > 1) // si en ese area existe otro objeto aparte de si mismo, o sea el count de Damagables es 2 o +
        {
            for (int i = 0; i < damagables.Count; i++)
            {
                damagables[i].GetComponent<Entity>().life -= 2; // explota y saca vida
            }
            Die();
        }

    }
    void Die()
    {
        owner.mymissiles.Remove(this);
        Destroy(gameObject);
    }
}
