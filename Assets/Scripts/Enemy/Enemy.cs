using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : Entity {

    public GameManager gamemanager;
	public GameObject target;
    public GameObject bulletPrefab;

	Vector3 _direction = Vector3.zero;
	float _timeOfPrediction = 4;
	// float speed;
    public float rotationSpeed;
    public float ShootCooldown;
    float shoottimer;

	SphereCollider _collider;

    public EventController controller;
    private EventFSM<Event> stateMachine;

    void Awake()
    {
        life = 10;
    }

    private void Start()
    {
		
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
                transform.position += transform.forward * 5 * Time.deltaTime;
            }
            else stateMachine.Feed(Event.onPatrol);
        };
		flee.OnUpdate += () => {
            Debug.Log("Flee");
            var allthreats2 = gamemanager.players
                         .Select(x => x.gameObject) // transformo los players en gameobjects
                         .Concat(gamemanager.players
                                .SelectMany(x => x.myturrets) // agarro todas las torretas
                                .Select(x => x.gameObject)) // las transformo en gameobjects y las adhiero a la lista con el concat
                         .Concat(gamemanager.players
                                .SelectMany(x => x.myturrets) // agarro todas las torretas
                                .SelectMany(x => x.mymissiles) // de ahi, agarro todas los misiles
                                .Select(x => x.gameObject)) // las transformo en gameobjects y las adhiero a la lista con el concat
                                // hasta aca lo unico que hice fue armar una lista con todos los Character, Turrets y Misiles
                                // transformados en gameobjects para que me deje meterlos todos en una misma lista
                         .OrderBy(x => Vector3.Distance(x.transform.position, transform.position)) // los ordeno por distancia
                         .TakeWhile(x => Vector3.Distance(x.transform.position, transform.position) < 20f); // tomo hasta k esten demasiado lejos

            var pointtoflee = allthreats2
                         .Aggregate(new Vector3(0, 0, 0), (a, b) => a + b.transform.position) // sumo todos los vectores de los "peligros"
                         / allthreats2.Count(); // los divido por el count para sacar el punto medio

            var speedtoflee = allthreats2
                         .Aggregate(0f, (a, b) => a + b.GetComponent<Entity>().life) // sumo todas las Life de los "peligros"
                         / 2; // lo divido x dos asi es un poquito mas lento y lo puedo alcanzar

            transform.forward = -(pointtoflee - transform.position).normalized; // mi direccion es la opuesta al "PointToFlee"
            transform.position += transform.forward * speedtoflee * Time.deltaTime; // mi velocidad para escapar es "SpeedToFlee"
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
}
