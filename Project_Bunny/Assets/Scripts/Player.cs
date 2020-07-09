using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
   [System.Serializable] public class PlayerStats
    {
        public int Health = 100;
    }

    public PlayerStats playerStats = new PlayerStats();

    public int fallBoundary = -10;

    private void Update()
    {
        if(transform.position.y <= fallBoundary)
        {
            DamagePlayer(200);
        }
    }

    public void DamagePlayer (int damage)
    {
        playerStats.Health -= damage;
        if (playerStats.Health <= 0)
        {
            Debug.Log("Muerto");
        }
    }
}
