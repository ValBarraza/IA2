using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    public Vector3 dirtolook;
    public float distancetoplayer;
	// Use this for initialization
	void Start () {
        distancetoplayer = Vector3.Distance(player.transform.position, transform.position);
        dirtolook = (player.transform.position - transform.position).normalized;
	}

	void Update () {
        transform.position = Vector3.Lerp(transform.position, player.transform.position - dirtolook * distancetoplayer, 0.5f);
	}
}
