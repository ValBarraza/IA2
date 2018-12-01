using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// esta clase maneja los triggers para pasar de un estado a otro basicamente, o sea aca deberian las funciones de LINQ que
// hacen que se triggeree un estado

public class InputController : MonoBehaviour {

    public event Action<InputF> OnInput = delegate { };

    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)
          || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            OnInput(InputF.onMove);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnInput(InputF.onShoot);
        }
        if ( !Input.anyKey ) OnInput(InputF.offMove);
    }
}
public enum InputF // eventos para los estados ( si fuera un jugador usaria inputs )
{
    onMove,
    onShoot,
    offMove,
    offShoot
}
