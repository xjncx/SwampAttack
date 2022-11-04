using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<Wave> _waves;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Player _player;

    private Wave _currentWave;
    private int _currentWaveNumber = 0;
    private float _timeAfterLastSpawn;
    private int _spawnedEnemies;

    public event UnityAction AllEnemySpawned;
    public event UnityAction<int, int> EnemyCountChanged;

    private void Start()
    {
        SetWave(_currentWaveNumber);
    }

    private void Update()
    {
        if (_currentWave == null)
            return;

        _timeAfterLastSpawn += Time.deltaTime;

        if (_timeAfterLastSpawn >= _currentWave.Delay)
        {
            InstantiateEnemy();
            _spawnedEnemies++;
            _timeAfterLastSpawn = 0;
            EnemyCountChanged?.Invoke(_spawnedEnemies, _currentWave.Count);
        }

        if (_currentWave.Count <= _spawnedEnemies)
        {
            if (_waves.Count > _currentWaveNumber + 1)
            {
                AllEnemySpawned?.Invoke();
            }

            _currentWave = null;
        }
    }

    public void NextWave()
    {
        SetWave(++_currentWaveNumber);
        _spawnedEnemies = 0;
    }

    private void InstantiateEnemy()
    {
        for (int i = 0; i < _currentWave.EnemyTemplates.Count; i++)
        {
            Enemy enemy = Instantiate(_currentWave.EnemyTemplates[i], _spawnPoint.position, _spawnPoint.rotation, _spawnPoint).GetComponent<Enemy>();
            enemy.Init(_player);
            enemy.Dying += OnEnemyDying;
        }
    }

    private void SetWave(int index)
    {
        _currentWave = _waves[index];
        EnemyCountChanged?.Invoke(0, 1);
    }

    private void OnEnemyDying(Enemy enemy)
    {
        enemy.Dying -= OnEnemyDying;
        _player.AddMoney(enemy.Reward);
    }
}
[System.Serializable]
public class Wave
{
    public List<GameObject> EnemyTemplates;
    public float Delay;
    public int Count;
}

//private void InstantiateEnemy()
//{
//    Enemy enemy = Instantiate(_currentWave.EnemyTemplate, _spawnPoint.position, _spawnPoint.rotation, _spawnPoint).GetComponent<Enemy>();
//    enemy.Init(_player);
//    enemy.Dying += OnEnemyDying;
//}