using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Attribute _health;
    [SerializeField] private Attribute _coins;
    [SerializeField] private float _speed;
    [SerializeField] private float _delayAfterDamage;
    [SerializeField] private Transform _paatroolPositionsContainer;
    [SerializeField] private Transform _pathFinder;
    [SerializeField] private CameraShaker _cameraShaker;
    [SerializeField] private AnimationCurve _animationCurve;

    private int _direction;
    private Vector3[] _patroolPositions;
    private int _currentPositionIndex;
    private int _lastPositionIndex;
    private bool _isMoving;

    private float _timeLeft;

    public bool CanTakeDamage { get; private set; } = true;

    private void Awake()
    {
        _patroolPositions = new Vector3[_paatroolPositionsContainer.childCount];

        for (int i = 0; i < _paatroolPositionsContainer.childCount; i++)
            _patroolPositions[i] = _paatroolPositionsContainer.GetChild(i).position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out InteractionObject obj))
            obj.OnInteraction(this);
    }

    private void Start()
    {
        _health.Init();
        _coins.Init();
    }

    public void Update()
    {
        ReadDirection();

        if (_pathFinder.position != _patroolPositions[_currentPositionIndex])
            Move();
        else if (_isMoving == true)
            StopMove();

        if (CanTakeDamage == false)
        {
            if (_timeLeft >= _delayAfterDamage)
            {
                CanTakeDamage = true;
                _timeLeft = 0;
            }
            else
            {
                _timeLeft += Time.deltaTime;
            }
        }
    }

    public void Damage(int damage)
    {
        CanTakeDamage = false;
        _health.ChangeValue(-damage);
        _cameraShaker.StartShake();

        if (_health.Value <= 0)
            Die();
    }

    public void AddHealth(int health)
    {
        _health.ChangeValue(health);
    }

    public void AddCoins(int amount)
    {
        _coins.ChangeValue(amount);
    }

    private void Move()
    {
        float distanceToLast = Vector2.Distance
        (
            _pathFinder.position,
            _patroolPositions[_lastPositionIndex]
        );

        float distanceBetweenPositions = Vector2.Distance
        (
            _patroolPositions[_currentPositionIndex],
            _patroolPositions[_lastPositionIndex]
        );

        float curveX = distanceToLast / distanceBetweenPositions;

        _pathFinder.position = Vector3.MoveTowards
        (
            _pathFinder.position,
            _patroolPositions[_currentPositionIndex],
            _speed * Time.deltaTime
        );

        transform.position = -_animationCurve.Evaluate(curveX) * transform.up + _pathFinder.transform.position;
        transform.up = Vector3.zero - transform.position;
    }

    private void StopMove()
    {
        _isMoving = false;
        _lastPositionIndex = GetCorrectIndex(_currentPositionIndex);
        TryStartMovingToNextPosition();
    }

    private void ReadDirection()
    {
        ReadButton(KeyCode.A, -1);
        ReadButton(KeyCode.D, 1);
    }

    private void ReadButton(KeyCode key, int value)
    {
        if (Input.GetKeyDown(key))
        {
            _direction += value;
            TryStartMovingToNextPosition();
        }
        else if (Input.GetKeyUp(key))
        {
            _direction -= value;
        }
    }

    private void TryStartMovingToNextPosition()
    {
        if
        (
            _direction != 0 &&
            (_currentPositionIndex == _lastPositionIndex && _isMoving == false ||
            GetCorrectIndex(_currentPositionIndex + _direction) == _lastPositionIndex)
        )
        {
            _lastPositionIndex = GetCorrectIndex(_currentPositionIndex);
            _currentPositionIndex = GetCorrectIndex(_currentPositionIndex + _direction);
            _isMoving = true;
        }
    }

    private int GetCorrectIndex(int newIndex)
    {
        if (newIndex == _patroolPositions.Length)
            return 0;
        else if (newIndex < 0)
            return _patroolPositions.Length - 1;

        return newIndex;
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
