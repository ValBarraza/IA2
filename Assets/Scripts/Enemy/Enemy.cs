using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : Entity {
	
	public GameObject target;
	public GameObject bulletPrefab;

	Vector3 _direction = Vector3.zero;
	float _timeOfPrediction = 4;
	// float speed;
    public float rotationSpeed;
    public float ShootCooldown;
    float shoottimer;

    //public List<GameObject> targets = new List<GameObject>();
    //public List<Collider> postargets = new List<Collider>();

	SphereCollider _collider;

    public EventController controller;
    private EventFSM<Event> stateMachine;

    void Awake()
    {
        life = 10;
    }

    private void Start()
    {
		//_collider = GetComponent<ShpereCollider>();
		
        // 1 - Creacion de los estados
        var patrol = new State<Event>("PATROL");
        var flee = new State<Event>("FLEE");
        var chase = new State<Event>("CHASE");
        var attack = new State<Event>("ATTACK");

        // 2 - Seteo de las transiciones
        patrol.AddTransition(Event.onAttack, attack);
        patrol.AddTransition(Event.onChase, chase);
        patrol.AddTransition(Event.onFlee, flee);

        attack.AddTransition(Event.onPatrol, patrol);
        attack.AddTransition(Event.onChase, chase);
        attack.AddTransition(Event.onFlee, flee);

        chase.AddTransition(Event.onAttack, attack);
        chase.AddTransition(Event.onFlee, flee);

        flee.AddTransition(Event.onChase, chase);

        // 3 - Seteo de los estados
        patrol.OnEnter += () => {};

        attack.OnEnter += () => 
        {
            var postargets = new List<Collider>();
            foreach (var postarget in Physics.OverlapSphere(transform.position, 10f))
            {
                postargets.Add(postarget);
            }
            var targets = postargets
                .Where(x =>
                            x.GetComponent<Misil>() // si es un misil
                            || x.GetComponent<Turret>() // si es una torreta
                            || x.GetComponent<Character>()) // si es el jugador
                .OrderBy(x =>
                            Vector3.Distance(x.gameObject.transform.position, transform.position)) // ordena por distancia
                .Select(x =>
                            x.gameObject) // toma los gameobject de los collider
                .ToList();
            target = targets
                .Where(x =>
                            x.GetComponent<Entity>().life < x.GetComponent<Entity>().life / 2 // si esta en menos de mitad de vida
                            || x.GetComponent<Entity>().life == 1) // o si tiene 1 de vida ( o sea si es un misil )
                .FirstOrDefault(); // agarra el mas primero de la lista, o sea el mas cercano.

            if (!target && targets.Count > 0) // si no encontro un target copado, pero aun asi hay objetivos posibles
                    target = targets[Random.Range(0, targets.Count)].gameObject; // Si no encontro un buen target, elije uno random
        };
		attack.OnUpdate += () => 
        {
            if (shoottimer > ShootCooldown && target) // si le da el cooldown para disparar y encontro algun target
            {
                transform.forward = (target.transform.position - transform.position).normalized;
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.transform.position = transform.position + transform.forward * 1.5f;
                bullet.transform.forward = transform.forward;
                shoottimer = 0;
            }
			stateMachine.Feed(Event.onPatrol); // al final, independientemente de lo que hizo, vuelve a patrol
		};

        chase.OnEnter += () => {
            Debug.Log("persiguiendo");
            var postargets = new List<Collider>();
            foreach (var postarget in Physics.OverlapSphere(transform.position, 10f))
            {
                postargets.Add(postarget);
            }
            var targets = postargets
                .Where(x =>
                            x.GetComponent<Turret>()
                            || x.GetComponent<Character>())
                .OrderBy(x =>
                            Vector3.Distance(x.gameObject.transform.position, transform.position))
                .Select(x =>
                            x.gameObject)
                .ToList();

            target = targets
                .Where(x =>
                            x.GetComponent<Entity>().life < x.GetComponent<Entity>().life / 2)
                .FirstOrDefault();

            if (!target && targets.Count > 0)
                    target = targets[Random.Range(0, targets.Count)].gameObject;
        };
        chase.OnUpdate += () =>
        {            
            if (target)
            {
                Debug.Log("persigo");
                transform.forward = (target.transform.position - transform.position).normalized;
                //_direction = target.transform.position + target.transform.forward * target.GetComponent<Entity>().speed * _timeOfPrediction;
                //transform.forward = Vector3.Lerp(transform.forward, _direction - transform.position, rotationSpeed * Time.deltaTime);
                transform.position += transform.forward * 5 * Time.deltaTime;
            }
            else stateMachine.Feed(Event.onPatrol);
        };
        flee.OnEnter += () => {};
		flee.OnUpdate += () => {
            Debug.Log("Flee");
            transform.forward = -(target.transform.position - transform.position).normalized;
            transform.position += transform.forward * 5 * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        };

        // 4 - Creacion de la FSM y asignacion del controlador de eventos/inputs
        stateMachine = new EventFSM<Event>(patrol);
        controller.OnEvent += evento => stateMachine.Feed(evento);
    }
    private void Update()
    {
        if (life <= 0) Destroy(gameObject);
        shoottimer += Time.deltaTime;
        stateMachine.Update();
    }

	//GameObject SetTarget(){
	//	_collider.
	//}
}
