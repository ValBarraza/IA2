using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    public float _speed;
    public GameObject _bulletPrefab;

    public InputController controller;
    private EventFSM<InputF> stateMachine;

    private void Awake()
    {
        life = 10;
    }
    private void Start()
    {
        // 1 - Creacion de los estados
        var idle = new State<InputF>("IDLE");
        var move = new State<InputF>("MOVE");
        var shoot = new State<InputF>("SHOOT");

        // 2 - Seteo de las transiciones
        idle.AddTransition(InputF.onMove, move);
        idle.AddTransition(InputF.onShoot, shoot);

        move.AddTransition(InputF.offMove, idle);
        move.AddTransition(InputF.onShoot, shoot);

        shoot.AddTransition(InputF.offShoot, idle);

        // 3 - Seteo de los estados
        idle.OnUpdate += () => 
        {
            /* aca iria la animacion de idle */
            Debug.Log("No estoy haciendo nada");
        };

        move.OnUpdate += () =>
        {
            if (Input.GetKey(KeyCode.W)) transform.position += transform.forward * _speed * Time.deltaTime;
            if (Input.GetKey(KeyCode.S)) transform.position -= transform.forward * _speed * Time.deltaTime;
            if (Input.GetKey(KeyCode.D)) transform.position += transform.right * _speed * Time.deltaTime;
            if (Input.GetKey(KeyCode.A)) transform.position -= transform.right * _speed * Time.deltaTime;
        };

        shoot.OnEnter += () =>
        {
            GameObject bullet = Instantiate(_bulletPrefab) as GameObject;
            bullet.transform.position = transform.position;
            bullet.transform.forward = transform.forward;
            stateMachine.Feed(InputF.offShoot); // spawnea la bullet y automaticamente le manda la señal a la FSM para k salga del estado
        };

        // 4 - Creacion de la FSM y asignacion del controlador de eventos/inputs
        stateMachine = new EventFSM<InputF>(idle);
        controller.OnInput += Input => stateMachine.Feed(Input);
    }
    private void Update() { stateMachine.Update(); }
}
