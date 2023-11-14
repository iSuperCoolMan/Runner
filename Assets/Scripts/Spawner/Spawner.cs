using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;

    [SerializeField] private Transform _spawnPointsContainer;
    [SerializeField] private Transform _center;
    [SerializeField] private PoolHandler<Obtacle> _obtaclesPoolHandler;
    [SerializeField] private PoolHandler<Coin> _coinsPoolHandler;
    [SerializeField] private PoolHandler<HealingObject> _heartsPoolHandler;

    private Vector3[] _spawnPoints;

    public IReleaser<Obtacle> ObtaclesPoolHandler => _obtaclesPoolHandler;
    public IReleaser<Coin> CoinsPoolHandler => _coinsPoolHandler;
    public IReleaser<HealingObject> HeartsPoolHandler => _heartsPoolHandler;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        _obtaclesPoolHandler.Init(transform);
        _coinsPoolHandler.Init(transform);
        _heartsPoolHandler.Init(transform);

        _spawnPoints = new Vector3[_spawnPointsContainer.childCount];

        for (int i = 0; i < _spawnPointsContainer.childCount; i++)
            _spawnPoints[i] = _spawnPointsContainer.GetChild(i).position;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Obtacle obtacle))
            _obtaclesPoolHandler.Release(obtacle);
        else if (collision.TryGetComponent(out Coin coin))
            _coinsPoolHandler.Release(coin);
        else if (collision.TryGetComponent(out HealingObject heart))
            _heartsPoolHandler.Release(heart);
    }

    private void Update()
    {
        _obtaclesPoolHandler.TrySpawn(_spawnPoints, _center);
        _coinsPoolHandler.TrySpawn(_spawnPoints, _center);
        _heartsPoolHandler.TrySpawn(_spawnPoints, _center);
    }
}
