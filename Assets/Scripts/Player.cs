using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [System.Serializable]
    public class PlayerStats
    {
        public int maxHealth = 100;
        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }
        public void Init()
        {
            curHealth = maxHealth;
        }

    }
    public PlayerStats stats = new PlayerStats();
    public int fallBoundary = -20;

    [SerializeField]
    private StatusIndictator statusIndictator;

    private void Start()
    {
        stats.Init();
        if (statusIndictator == null)
        {
            Debug.LogError("No status indicator referenced on Player");
            //statusIndictator.SetHealth(stats.curHealth, stats.maxHealth);
        }
        else
        {
            statusIndictator.SetHealth(stats.curHealth, stats.maxHealth);
        }
    }

    private void Update()
    {
        if(transform.position.y <= fallBoundary)
        {
            DamagePlayer(999);
        }
        if (statusIndictator == null)
        {
            statusIndictator.SetHealth(stats.curHealth, stats.maxHealth);
        }
    }
    public void DamagePlayer(int damage)
    {
        stats.curHealth -= damage;
        if (stats.curHealth <= 0)
        {
            Debug.Log("KILL PLAYER");
            GameMaster.KillPlayer(this);
        }
        statusIndictator.SetHealth(stats.curHealth, stats.maxHealth);
    }
}
