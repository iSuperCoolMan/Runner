using System;
using UnityEngine;

[Serializable]
public class Attribute
{
    [SerializeField] private int _startValue;
    [SerializeField] private AttributeView _view;

    public int Value { get; private set; }

    public void Init()
    {
        Value = _startValue;
        _view.Change(Value);
    }

    public void ChangeValue(int changes)
    {
        Value += changes;
        _view.Change(Value);
    }
}
