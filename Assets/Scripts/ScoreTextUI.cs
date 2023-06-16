using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}
public enum ScoreUIType
{
    CurrentScore,
    Highscore
}

