using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misil : MonoBehaviour {

    public GameObject Target;
    public float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.forward = (Target.transform.position - transform.position).normalized;
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;
        if (Vector3.Distance(Target.transform.position, transform.position) < 0.7f || !Target)
        {
            Destroy(gameObject);
        }
	}
}
