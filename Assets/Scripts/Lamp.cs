using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lamp : MonoBehaviour
{
    Image   image;
    Neuron  neuron;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        neuron = GetComponent<Neuron>();
    }

    // Update is called once per frame
    void Update()
    {
        image.color = (neuron.GetValue() > 0) ? (Color.green) : (Color.red);
    }
}
