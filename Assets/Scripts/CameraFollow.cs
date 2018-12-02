using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    public Vector3 dirtolook;
    public float distancetoplayer;

	void Update () {
        transform.position = player.transform.position - player.transform.forward * 15 + new Vector3(0,20,0);
        transform.forward = (player.transform.position - transform.position).normalized;
    }
}
