using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class InteractionObject : MonoBehaviour
{
    [SerializeField] Transform _fieldCenter;
    [SerializeField] private float _moveSpeed;

    private float _sizeMultiplier = 1.15f;

    protected abstract void Update();
    public abstract void OnInteraction(Player player);

    public virtual void Reset(Vector3 spawnPoint, Transform fieldCenter)
    {
        _fieldCenter = fieldCenter;

        transform.position = spawnPoint;
        transform.up = transform.position - _fieldCenter.position;
        ChangeSize();
    }

    protected void Move()
    {
        transform.position = Vector3.MoveTowards
        (
            transform.position,
            transform.position + transform.up,
            _moveSpeed * Time.deltaTime
        );

        ChangeSize();
    }

    private void ChangeSize()
    {
        transform.localScale = Vector3.Distance(_fieldCenter.position, transform.position) / _sizeMultiplier * Vector3.one;
    }
}
