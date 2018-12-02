using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Turret : Entity {

    public Character owner;
    public List<Misil> mymissiles;
    public GameObject Target;
    public GameObject turretCannon;
    public GameObject missilePrefab;
    public float ShootCooldown;
    float _shoottimer;

    private void Awake()
    {
        life = 2;
    }

    // Update is called once per frame
    void Update()
    {
        _shoottimer += Time.deltaTime;
        GetTarget();
        if (Target)
        {
            turretCannon.transform.forward = (Target.transform.position - turretCannon.transform.position).normalized;
            if (_shoottimer > ShootCooldown && Vector3.Distance(Target.transform.position, turretCannon.transform.position) < 8f)
            {
                _shoottimer = 0;
                Shoot();
            }
        }
    }
    void Shoot()
    {
        GameObject missile = Instantiate(missilePrefab) as GameObject;
        missile.transform.position = turretCannon.transform.position + turretCannon.transform.forward * 1.5f;
        missile.transform.forward = transform.forward;
        missile.GetComponent<Misil>().owner = this;
        missile.GetComponent<Misil>().target = Target;
        mymissiles.Add(missile.GetComponent<Misil>());
    }
    void GetTarget()
    {
        var postargets = new List<Collider>();
        foreach (var postarget in Physics.OverlapSphere(transform.position, 10f))
        {
            postargets.Add(postarget);
        }
        Target = postargets
            .Where(x => x.GetComponent<Enemy>()) // agarra solo a los enemigos
            .OrderBy(x => Vector3.Distance(x.gameObject.transform.position, transform.position)) // ordena por distancia
            .Select(x => x.gameObject)
            .FirstOrDefault(); // agarra al mas cercano
    }
    void Die()
    {
        owner.myturrets.Remove(this);
    }
}
