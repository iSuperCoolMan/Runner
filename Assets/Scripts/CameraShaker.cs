using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private float _shakeTime;
    [SerializeField] private float _shakeForce;

    private Vector3 _startPosition;
    private float _timeLeft;

    private void Awake()
    {
        _startPosition = transform.position;
        _shakeForce /= 100;
    }

    private void Update()
    {
        if (_timeLeft < _shakeTime)
        {
            transform.position = _startPosition + 
                new Vector3(GetRandomForce(), GetRandomForce()) *
                (_shakeTime - _timeLeft) / _shakeTime;
            _timeLeft += Time.deltaTime;
        }
        else
        {
            enabled = false;
            transform.position = _startPosition;
        }
    }

    public void StartShake()
    {
        enabled = true;
        _timeLeft = 0;
    }

    private float GetRandomForce()
    {
        return Random.Range(-_shakeForce, _shakeForce);
    }
}
