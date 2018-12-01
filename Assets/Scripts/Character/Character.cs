using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    public float _speed;
    public GameObject _bulletPrefab;
    public GameObject _turretPrefab;
    public float turretCooldown;
    public float shootCooldown;
    float turrettimer;
    float shoottimer;

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
        var deployturret = new State<InputF>("DEPLOYTURRET");

        // 2 - Seteo de las transiciones
        idle.AddTransition(InputF.onMove, move);
        idle.AddTransition(InputF.onShoot, shoot);
        idle.AddTransition(InputF.onDeployT, deployturret);

        move.AddTransition(InputF.offMove, idle);
        move.AddTransition(InputF.onShoot, shoot);
        move.AddTransition(InputF.onDeployT, deployturret);

        shoot.AddTransition(InputF.offShoot, idle);

        deployturret.AddTransition(InputF.offDeployT, idle);

        // 3 - Seteo de los estados
        idle.OnUpdate += () => 
        {
            /* aca iria la animacion de idle */
        };

        move.OnUpdate += () =>
        {
            if (Input.GetKey(KeyCode.W)) transform.position += transform.forward * _speed * Time.deltaTime;
            if (Input.GetKey(KeyCode.S)) transform.position -= transform.forward * _speed * Time.deltaTime;
            if (Input.GetKey(KeyCode.E)) transform.position += transform.right * _speed * Time.deltaTime;
            if (Input.GetKey(KeyCode.Q)) transform.position -= transform.right * _speed * Time.deltaTime;
            if (Input.GetKey(KeyCode.A)) transform.Rotate(0, -2, 0);
            if (Input.GetKey(KeyCode.D)) transform.Rotate(0, 2, 0);
        };

        shoot.OnEnter += () =>
        {
            if (shoottimer > shootCooldown)
            {
                GameObject bullet = Instantiate(_bulletPrefab) as GameObject;
                bullet.transform.position = transform.position + transform.forward * 1.5f;
                bullet.transform.forward = transform.forward;
                shoottimer = 0;
            }
            stateMachine.Feed(InputF.offShoot); // spawnea la bullet y automaticamente le manda la señal a la FSM para k salga del estado
        };

        deployturret.OnEnter += () =>
        {
            if (turrettimer > turretCooldown)
            {
                GameObject turret = Instantiate(_turretPrefab) as GameObject;
                turret.transform.position = transform.position + transform.forward * 1.5f;
                turret.transform.forward = transform.forward;
                turret.transform.Rotate(-90, 0, 0);
                turrettimer = 0;
            }
            stateMachine.Feed(InputF.offDeployT); // spawnea la bullet y automaticamente le manda la señal a la FSM para k salga del estado
        };

        // 4 - Creacion de la FSM y asignacion del controlador de eventos/inputs
        stateMachine = new EventFSM<InputF>(idle);
        controller.OnInput += Input => stateMachine.Feed(Input);
    }
    private void Update()
    {
        shoottimer += Time.deltaTime;
        turrettimer += Time.deltaTime;
        stateMachine.Update();
    }
}
