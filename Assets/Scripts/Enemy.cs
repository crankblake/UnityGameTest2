using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;
        private int _curHealth;
        public int CurHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }
        public int damage = 40;
        public float shakeAmt = 0.1f;
        public float shakeLength = 0.1f;
        public void Init()
        {
            CurHealth = maxHealth;
        }
    }
    public EnemyStats stats = new EnemyStats();

    public Transform deathParticles;
    public float shakeAmt = 0.1f;
    public float shakeLength = 0.1f;

    [Header("Optional")]
    [SerializeField]
    private StatusIndictator statusIndictator;

    private void Start()
    {
        stats.Init();
        if (statusIndictator != null)
        {
            statusIndictator.SetHealth(stats.CurHealth, stats.maxHealth);
        }
        if (deathParticles == null)
        {
            Debug.LogError("No death particles referenced on enemy");
        }
    }

    public void DamageEnemy(int damage)
    {
        stats.CurHealth -= damage;
        if (stats.CurHealth <= 0)
        {
            Debug.Log("KILL ENEMY");
            GameMaster.KillEnemy(this);
        }
        if (statusIndictator != null)
        {
            statusIndictator.SetHealth(stats.CurHealth, stats.maxHealth);
        }
    }
    private void OnCollisionEnter2D(Collision2D _colInfo)
    {
        Player _player = _colInfo.collider.GetComponent<Player>();
        if (_player != null)
        {
            _player.DamagePlayer(stats.damage);
            DamageEnemy(999);
        }
    }
}
