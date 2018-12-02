using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Enemy[] enemies;
    public Character[] players;

    void Awake()
    {
        enemies = FindObjectsOfType<Enemy>();
        players = FindObjectsOfType<Character>();
        for (int i = 0; i < enemies.Length; i++) enemies[i].gamemanager = this;
        for (int i = 0; i < players.Length; i++) players[i].gamemanager = this;
    }
}
