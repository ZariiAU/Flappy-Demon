using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SlerpRandomiser : MonoBehaviour
{
    public float minIntensity = 1f;
    public float maxIntensity = 2f;
    Light2D light;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        light.intensity = minIntensity;
        yield return new WaitForSeconds(Random.RandomRange(1f, 2));
        light.intensity = maxIntensity;
    }
}
