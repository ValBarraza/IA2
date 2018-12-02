using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bullet : MonoBehaviour {

    public float autodestroytimer;
    public float _speed;
    GameObject owner;

    // Update is called once per frame
    void Update ()
    {
        autodestroytimer += Time.deltaTime;  // autodestruccion
        if (autodestroytimer > 5f) Destroy(gameObject); // autodestruccion

        transform.position += transform.forward * _speed * Time.deltaTime; // movimiento

        var postargets = new List<Collider>();
        foreach (var postarget in Physics.OverlapSphere(transform.position, 0.5f)) // obtencion de posibles golpeados
        {
            postargets.Add(postarget);
        }

        var target = postargets
            .Where(x => x != owner && x.GetComponent<Entity>()) // para que no se detecte a si mismo
            .OrderBy(x => Vector3.Distance(x.gameObject.transform.position, transform.position)) // ordena por distancia
            .FirstOrDefault(); // agarra al mas cercano

        if (target) // si encontro un target
        {
            target.GetComponent<Entity>().life -= 1 ; // baja la vida
            Destroy(gameObject); // se autodestruye
        }
    }
}
