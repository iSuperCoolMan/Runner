using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obtacle : InteractionObject
{
    [SerializeField] private float _flashingTime;
    [SerializeField] private int _flashingCount;
    [SerializeField] private float _flashingAlpha;

    private int _damage = 1;
    private SpriteRenderer[] _renderers;
    private float _startAlpha;
    private int _flashingDoneCount = 0;
    private float _timeLeft;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<SpriteRenderer>();
        _startAlpha = _renderers[0].color.a;
    }

    protected override void Update()
    {
        if (_timeLeft >= _flashingTime)
        {
            Move();
        }
        else
        {
            Flashing();
            _timeLeft += Time.deltaTime;
        }
    }

    public override void OnInteraction(Player player)
    {
        player.Damage(_damage);
    }

    public override void Reset(Vector3 spawnPoint, Transform fieldCenter)
    {
        base.Reset(spawnPoint, fieldCenter);
        _timeLeft = 0;
        _flashingDoneCount = 0;
    }

    private void Flashing()
    {
        if (_timeLeft / _flashingTime >= _flashingDoneCount / (float)_flashingCount)
        {
            _flashingDoneCount++;

            if (_renderers[0].color.a != 1)
                foreach (SpriteRenderer r in _renderers)
                    r.color = new Color(r.color.r, r.color.g, r.color.b, _startAlpha);
            else
                foreach (SpriteRenderer r in _renderers)
                    r.color = new Color(r.color.r, r.color.g, r.color.b, _flashingAlpha);
        }
    }
}
