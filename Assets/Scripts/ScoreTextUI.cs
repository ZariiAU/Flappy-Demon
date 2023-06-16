using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ScoreTextUI : MonoBehaviour
{
    TMP_Text textBox;
    public ScoreUIType scoreType;

    // Start is called before the first frame update
    void Start()
    {
        textBox = GetComponent<TMP_Text>();
    }

    public void ChangeText(string _text)
    {
        textBox.text = _text;
    }
    public void ChangeText(int _text)
    {
        textBox.text = _text.ToString();
    }
}
public enum ScoreUIType
{
    CurrentScore,
    Highscore
}

