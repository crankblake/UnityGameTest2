using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;

    [SerializeField]
    private int maxLives = 3;
    private static int _remainingLives;
    public static int RemainingLives
    {
        get { return _remainingLives; }
    }
    private void Awake()
    {
       // Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        if (gm == null)
        {
            //gm = GameObject.FindGameObjectWithTag("Player").GetComponent<GameMaster>();
            //Debug.Log(gm);
            // gm.RespawnPlayer();
            gm = this;
        }
      
    }
    public Transform playerPrefab;
    public string spawnSoundName;
    public Transform spawnPoint;
    public int spawnDelay = 3;
    public Transform spawnPrefab;

    public CameraShake camerShake;

    [SerializeField]
    private GameObject gameOverUI;

    //cache
    private AudioManager audioManager;
    private void Start()
    {
        _remainingLives = maxLives;
        if (camerShake == null)
        {
            Debug.LogError("No camera shake referenced in GameMaster");
        }
        //caching
        audioManager = AudioManager.instance;

        if (audioManager == null)
        {
            Debug.LogError("FREAK OUT! No AudioManager found in the scene");
        }
    }
    public void _RespawnPlayer()
    {
        audioManager.PlaySound(spawnSoundName);
        StartCoroutine(JankyPause());
        //yield return new WaitForSeconds(5);
        //GameObject.Find("Player").transform.position = new Vector3(0, 0, 0);
        //GameObject.Find("Player").transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0);
        //GameObject.Find("Player").transform.
    }
    public void EndGame()
    {
        Debug.Log("GAME OVER");
        Cursor.visible = true;
        gameOverUI.SetActive(true);
    }
    public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        _remainingLives--;
        if (_remainingLives <= 0)
        {
            gm.EndGame();
        }
        else
        {
            gm._RespawnPlayer();
        }
        
        //Cinemachine.CinemachineVirtualCamera.m_follow
    }
    public static void KillEnemy(Enemy enemy)
    {
        gm._KillEnemy(enemy);
        //Destroy(enemy.gameObject);
        //gm.RespawnPlayer();
        //Cinemachine.CinemachineVirtualCamera.m_follow
    }
    public void _KillEnemy(Enemy _enemy)
    {
        Transform _clone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as Transform;
        Destroy(_clone.gameObject, 5f);
        camerShake.Shake(_enemy.shakeAmt, _enemy.shakeLength);
        Destroy(_enemy.gameObject);
    }
    IEnumerator JankyPause()
    {
        print(Time.time);
        yield return new WaitForSecondsRealtime(spawnDelay);
        
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
        //Destroy(this.gameObject, 3f);
        print(Time.time);
    }
  


}


