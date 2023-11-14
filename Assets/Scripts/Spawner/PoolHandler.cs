using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[Serializable]
public class PoolHandler<T> : IReleaser<T> where T : InteractionObject
{
    [SerializeField, SerializeReference] private T _object;
    [SerializeField] private int _maxSpawnCount;
    [SerializeField] private float _spawnDelay;

    private Stack<int> _engagedSpawnPointIndecies;
    private ObjectPool<T> _pool;
    private float _timeLeft;

    public void Init(Transform parent)
    {
        _engagedSpawnPointIndecies = new Stack<int>();

        _pool = new ObjectPool<T>
        (
            createFunc: () => UnityEngine.Object.Instantiate(_object, parent),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => UnityEngine.Object.Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 5,
            maxSize: 10
        );
    }

    public void TrySpawn(Vector3[] spawnPoints, Transform fieldCenter)
    {
        if (_timeLeft >= _spawnDelay)
        {
            if (_maxSpawnCount > 1)
                SpawnMultiply(spawnPoints, fieldCenter);
            else
                SpawnSingle(spawnPoints, fieldCenter);

            _timeLeft -= _spawnDelay;
        }

        _timeLeft += Time.deltaTime;
    }

    public void Release(T obj)
    {
        _pool.Release(obj);
    }

    private void SpawnMultiply(Vector3[] spawnPoints, Transform fieldCenter)
    {
        int spawnCount = UnityEngine.Random.Range(1, _maxSpawnCount);

        for (int i = 0; i < spawnCount; i++)
        {
            int spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Length - 1);

            while (_engagedSpawnPointIndecies.Contains(spawnPointIndex))
                spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Length - 1);

            _engagedSpawnPointIndecies.Push(spawnPointIndex);
            Vector3 spawnPoint = spawnPoints[spawnPointIndex];

            _pool.Get().Reset(spawnPoint, fieldCenter);
        }

        _engagedSpawnPointIndecies.Clear();
    }

    private void SpawnSingle(Vector3[] spawnPoints, Transform fieldCenter)
    {
        int spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Length - 1);
        Vector3 spawnPoint = spawnPoints[spawnPointIndex];
        _pool.Get().Reset(spawnPoint, fieldCenter);
    }
}
