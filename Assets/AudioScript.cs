using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioScript : MonoBehaviour
{
    Slider slider;
    [SerializeField] string channelName;
    [SerializeField] AudioMixer audioMixer;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetVolume(float soundLevel)
    {
        audioMixer.SetFloat(channelName, soundLevel);
    }
}
