using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttributeView : MonoBehaviour
{
    private TMP_Text _text;
    private string _startText;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _startText = _text.text;
    }

    public void Change(int value)
    {
        _text.text = _startText + value;
    }
}
