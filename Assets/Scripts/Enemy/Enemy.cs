using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	
	public GameObject target;
	public GameObject bulletPrefab;

	Vector3 _direction = Vector3.Zero;
	float _timeOfPrediction = 4;
	public float speed;
	public float rotationSpeed;	

	SphereCollider _collider;

    public EventController controller;
    private EventFSM<Event> stateMachine;

    private void Start()
    {
		_collider = GetComponent<ShpereCollider>();
		
        // 1 - Creacion de los estados
        var patrol = new State<Event>("PATROL");
        var flee = new State<Event>("FLEE");
        var chase = new State<Event>("CHASE");
        var attack = new State<Event>("ATTACK");

        // 2 - Seteo de las transiciones
        patrol.AddTransition(Event.onAttack, attack);

        attack.AddTransition(Event.onPatrol, patrol);
        attack.AddTransition(Event.onChase, chase);

        chase.AddTransition(Event.onAttack, attack);
        chase.AddTransition(Event.onFlee, flee);

        flee.AddTransition(Event.onChase, chase);

        // 3 - Seteo de los estados
        patrol.OnEnter += () => {};
        attack.OnEnter += () => {};
		attack.OnUpdate += () => {
			/*if(target){
				GameObject bullet = GameObject.Instantiate(bulletPrefab);
				bullet.transform.position = this.transform.position;
				bullet.transform.forward = this.transform.forward;
			}
			else stateMachine.Feed(Event.onPatrol);
			*/
		};
        chase.OnEnter += () => {};
		chase.OnUpdate += () => {
			/*
				if(target) _direction = target.transform.position + target.transform.forward * target.speed * _timeOfPrediction;
				else if(!target) stateMachine.Feed(Event.onPatrol);
				transform.forward = Vector3.Lerp(transform.forward, _direction - transform.position, rotationSpeed * Time.deltaTime);
				transform.position += transform.forward * speed * Time.deltaTime;
			*/
		}
        flee.OnEnter += () => {};
		flee.OnUpdate += () => {
			/*
				if(hp <= 5){
					_direction = -(target.transform.position - transform.position);
					transform.forward = Vector3.Lerp(transform.forward, _direction, rotationSpeed * Time.deltaTime);
					transform.position += transform.forward * speed * Time.deltaTime;
				}
				else stateMachine.Feed(Event.onPatrol);
			*/	
		};

        // 4 - Creacion de la FSM y asignacion del controlador de eventos/inputs
        stateMachine = new EventFSM<Event>(patrol);
        controller.OnEvent += evento => stateMachine.Feed(evento);
    }
    private void Update() { stateMachine.Update(); }

	GameObject SetTarget(){
		_collider.
	}
}
