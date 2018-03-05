using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using GC;

public class EnemyManager : MonoBehaviour {

    private int wave_id;
    private List<BasicEnemy> active_enemies;
    private List<BasicEnemy> destroy_queue;

    [SerializeField]
    private GameObject basic_E_prefab;
    [SerializeField]
    private GameObject explosive_E_prefab;
    [SerializeField]
    private GameObject wave_E_prefab;

    private GameObject player;

    private Dictionary<int, List<GameObject>> wave_directory;

    private Text waveText;

    public AudioSource source;
    public AudioClip clearClip;

    private TaskManager tm = new TaskManager();

	// Use this for initialization
	void Start () {
        EventManager.Instance.AddHandler<WaveClearEvent>(OnWaveCleared);
        waveText = GameObject.Find("WaveTracker").GetComponent<Text>();
        InitList();
        player = GameObject.Find("Ship");
        wave_id = 1;
        InitNewWave();
	}

    void OnWaveCleared(WaveClearEvent e)
    {
       
        waveText.text = "Wave: " + e.waveNumber;
        
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.R)) //if you restart clear handler
        {
            EventManager.Instance.RemoveHandler<WaveClearEvent>(OnWaveCleared);
        }

        if (active_enemies.Count == 0)
        {
            DoNewWave();
            EventManager.Instance.Fire(new WaveClearEvent(wave_id));

        }

        tm.Update();

        DestructionCleaning();
	}

    private void IncrementID()
    {
        wave_id++;
    }

    private void DoNewWave()
    {
        tm.Do(new ActionTask(IncrementID))
            .Then(new ActionTask(InitNewWave));
    }

    private void InitList()
    {
        active_enemies = new List<BasicEnemy>();
        destroy_queue = new List<BasicEnemy>();
    }

    private void InitNewWave()
    {
        Debug.Log(wave_id);
        switch (wave_id)
        {
            case 1:
                SpawnEnemyPrefab(basic_E_prefab);
                SpawnEnemyPrefab(basic_E_prefab);
                SpawnEnemyPrefab(basic_E_prefab);
                break;

            case 2:
                SpawnEnemyPrefab(basic_E_prefab);
                SpawnEnemyPrefab(basic_E_prefab);
                SpawnEnemyPrefab(basic_E_prefab);
                SpawnEnemyPrefab(wave_E_prefab);
                break;

            case 3:
                SpawnEnemyPrefab(explosive_E_prefab);
                SpawnEnemyPrefab(explosive_E_prefab);
                SpawnEnemyPrefab(explosive_E_prefab);
                SpawnEnemyPrefab(explosive_E_prefab);
                break;

            case 4:
                SpawnEnemyPrefab(basic_E_prefab);
                SpawnEnemyPrefab(basic_E_prefab);
                SpawnEnemyPrefab(explosive_E_prefab);
                SpawnEnemyPrefab(explosive_E_prefab);
                SpawnEnemyPrefab(explosive_E_prefab);
                break;

            case 5:
                SpawnEnemyPrefab(wave_E_prefab);
                SpawnEnemyPrefab(wave_E_prefab);
                SpawnEnemyPrefab(wave_E_prefab);
                SpawnEnemyPrefab(basic_E_prefab);
                SpawnEnemyPrefab(basic_E_prefab);
                break;

            default:
                SpawnEnemyPrefab(explosive_E_prefab);
                SpawnEnemyPrefab(explosive_E_prefab);
                SpawnEnemyPrefab(explosive_E_prefab);
                SpawnEnemyPrefab(wave_E_prefab);
                SpawnEnemyPrefab(wave_E_prefab);
                SpawnEnemyPrefab(wave_E_prefab);
                break;
        }
    }

    public void SpawnEnemyPrefab(GameObject enemy)
    {
        Vector2 spawn_pos = RandomSpawnLocation(new Vector2(6f,6f));
        GameObject enemy_ship = Instantiate(enemy, (Vector3)spawn_pos, Quaternion.identity);
        enemy_ship.GetComponent<BasicEnemy>().SetManager(this);
        active_enemies.Add(enemy_ship.GetComponent<BasicEnemy>());
    }

    public void QueueDestruction(BasicEnemy enemy)
    {
        active_enemies.Remove(enemy);
        enemy.SpawnDeathPart();  
    }

    public void DestructionCleaning()
    {
        foreach(BasicEnemy enemy in destroy_queue)
        {
            active_enemies.Remove(enemy);
        }
    }

    private Vector2 RandomSpawnLocation(Vector2 range)
    {
        try {
            return player.transform.position + new Vector3(0f,6f) + new Vector3((UnityEngine.Random.value - 0.5f) * range.x, (UnityEngine.Random.value - 0.5f) * range.y);
        }
        catch (Exception e)
        {
            return Vector2.zero;
        }
    }
}
