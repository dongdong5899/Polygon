using System;
using System.Collections.Generic;
using UnityEngine;
using YH.Enemy;

public struct WaveInfo
{
    public int commonEnemyCount;
    public int eliteEnemyCount;
}

public class WaveManager : MonoSingleton<WaveManager>
{
    public int waveCount = 1;
    public List<Enemy> enemyList { get; set; }

    private int _currentWaveTotalEnemyCount = 0;
    public event Action OnWaveStart;

    private void Awake()
    {
        enemyList = new List<Enemy>();
    }

    public void SetWaveEnemyCount(int count)
    {
        _currentWaveTotalEnemyCount = count;
    }
    public void RemoveEnemyCount()
    {
        _currentWaveTotalEnemyCount--;
        if (_currentWaveTotalEnemyCount < 0) _currentWaveTotalEnemyCount = 0;
    }
    public bool IsClearWave()
        => _currentWaveTotalEnemyCount == 0;

    public bool CheckEnemyWave()
    {
        if (enemyList.Count == 0) 
            return true;
        else 
            return false;
    }


    public WaveInfo CreateWave()
    {
        WaveInfo wave = new WaveInfo();
        wave.commonEnemyCount = waveCount *4;
        wave.eliteEnemyCount = Mathf.CeilToInt(Mathf.Log(PlayerManager.Instance.CurrentPlayerLevel,2));
        if (wave.eliteEnemyCount == 0)
        {
            wave.eliteEnemyCount = 1;
        }
        Debug.Log(wave.eliteEnemyCount);
        waveCount++;
        OnWaveStart?.Invoke();
        return wave;
    }
}
