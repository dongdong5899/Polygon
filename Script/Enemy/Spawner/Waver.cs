using ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YH.Enemy;

public class Waver : MonoBehaviour
{
    private Transform[] _spawnPoints;
    private List<PoolingType> _enemyList;
    private float _lastWaveTime = 0;
    [SerializeField] private float _waveDelayTime;
    private bool _isStart;
    private List<int> _spawnPointIdxs;

    private void Awake()
    {
        _spawnPoints = GetComponentsInChildren<Transform>();
        _enemyList = new List<PoolingType>();
        _spawnPointIdxs = new List<int>();
    }

    

    private void Update()
    {
        if (!_isStart)
        {
            StartWave();
            _isStart = true;
        }

        if (WaveManager.Instance.IsClearWave())
        {
            StartWave();
        }
    }

    private void StartWave()
    {
        SetWave();
        StartCoroutine(SpawnWave());
        _lastWaveTime = Time.time;
    }

    private void SetWave()
    {
        _enemyList.Clear();
        WaveInfo wave = WaveManager.Instance.CreateWave();
        WaveManager.Instance.SetWaveEnemyCount(wave.commonEnemyCount + wave.eliteEnemyCount);
        for (int i = 0; i< wave.commonEnemyCount; ++i)
        {
            PoolingType type = (PoolingType)Random.Range(6, 9);
            _enemyList.Add(type);
        }
        for (int i = 0; i < wave.eliteEnemyCount; ++i)
        {
            PoolingType type = (PoolingType)Random.Range(9, 11);
            _enemyList.Add(type);
        }

    }
    private IEnumerator SpawnWave()
    {
        int count = 10;
        int currnet = 0;
        for(int i = 0; i < _enemyList.Count; i++)
        {
            currnet++;
            if (currnet >= count)
            {
                currnet = 0;
                yield return new WaitForSeconds(Mathf.Clamp(7f - Mathf.Log(1.3f, WaveManager.Instance.waveCount), 0, 10)/1.5f);
            }
            int pointIdx = Random.Range(1, _spawnPoints.Length);
            Enemy enemy = PoolManager.Instance.Pop(_enemyList[i]) as Enemy;
            enemy.transform.position = _spawnPoints[pointIdx].position;
            WaveManager.Instance.enemyList.Add(enemy);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
