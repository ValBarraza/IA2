using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Misil : Entity {

    public GameObject Target;
    public float speed;
    public GameObject owner;

    private void Awake()
    {
        life = 1;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.forward = (Target.transform.position - transform.position).normalized;
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;

        var damagables = new List<Collider>();
        foreach (var damagable in Physics.OverlapSphere(transform.position, 0.6f)) // obtencion de posibles golpeados
        {
            if (damagable != owner.gameObject && damagable.GetComponent<Entity>())
                damagables.Add(damagable);
        }
        if (damagables.Count > 1)
        {
            for (int i = 0; i < damagables.Count; i++)
            {
                damagables[i].GetComponent<Entity>().life -= 2;
            }
            Destroy(gameObject);
        }
        if (life <= 0) Destroy(gameObject);
    }
}
