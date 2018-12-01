using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Entity {

    public GameObject Target;
    public GameObject turretCannon;
    public GameObject missilePrefab;
    public float ShootCooldown;
    float _shoottimer;

    private void Awake()
    {
        life = 2;
    }

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        _shoottimer += Time.deltaTime;
        turretCannon.transform.forward = (Target.transform.position - turretCannon.transform.position).normalized;
        if (_shoottimer > ShootCooldown && Vector3.Distance(Target.transform.position, turretCannon.transform.position) < 8f)
        {
            _shoottimer = 0;
            Shoot();
        }
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(missilePrefab) as GameObject;
        bullet.transform.position = turretCannon.transform.position + turretCannon.transform.forward * 1.5f;
        bullet.transform.forward = transform.forward;
        bullet.GetComponent<Misil>().owner = gameObject;
        bullet.GetComponent<Misil>().Target = Target;
    }
}
