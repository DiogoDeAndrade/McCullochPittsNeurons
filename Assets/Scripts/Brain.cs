using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    static public Brain Get()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<Brain>();
        }
        return instance;
    }

    static Brain instance;

    public LineRenderer lineRendererPrefab;
    public Gradient     excitatoryGradient;
    public Gradient     inhibitoryGradient;

    void Awake()
    {
        if ((instance) && (instance != this))
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Update()
    {
        
    }

    public void RunSimulationStep()
    {
        Neuron[] neurons = FindObjectsOfType<Neuron>();

        foreach (var n in neurons)
        {
            n.ComputeOutput();
        }

        foreach (var n in neurons)
        {
            n.UpdateInputs();
        }
    }
}
