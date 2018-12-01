using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    public GameObject Target;
    public GameObject turretCannon;
    public GameObject missilePrefab;
    public float ShootCooldown;
    float _shoottimer;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        _shoottimer += Time.deltaTime;
        turretCannon.transform.forward = (Target.transform.position - turretCannon.transform.position).normalized;
        if (_shoottimer > ShootCooldown && Vector3.Distance(Target.transform.position, turretCannon.transform.position) < 5f)
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
        bullet.GetComponent<Misil>().Target = Target;
    }
}
