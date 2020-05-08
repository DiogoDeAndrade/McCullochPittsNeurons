using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    Neuron neuron;
    Image  image;

    void Start()
    {
        neuron = GetComponent<Neuron>();
        neuron.manualValue = true;
        neuron.SetValue(0);

        image = GetComponent<Image>();
        image.color = Color.red;
    }

    void Update()
    {
        
    }

    public void Pulse(int val)
    {
        neuron.SetValue(val);

        if (val == 0) image.color = Color.red;
        else image.color = Color.green;
    }
}
