using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class Neuron : MonoBehaviour
{
    public enum DendriteType { Excitatory, Inhibitory };

    [System.Serializable]
    public class Dendrite
    {
        public DendriteType type;
        public Neuron       neuron;
        public int          value;
    }

    public int              theta = 1;
    public List<Dendrite>   dendrites;
    [Header("Display")]
    public TextMeshProUGUI  thetaTextRef;
    public Image            chargeFill;
    public float            radius = 0.5f;

    class Line
    {
        public LineRenderer lineRenderer;
        public Vector3      prevPos;
    }

    List<Line>      lines;
    Vector3         prevPos = Vector3.zero;    

    int     currentValue = 0;

    [HideInInspector]   public  bool    manualValue = false;

    void Start()
    {
        currentValue = 0;
        UpdateTheta();
    }

    void Update()
    {
        if (Vector3.Distance(prevPos, transform.position) > 0.0005f)
        {
            UpdateLines(true);
            prevPos = transform.position;
        }
        else UpdateLines(false);
        UpdateCharge();
    }

    private void OnValidate()
    {
        UpdateLines(true, true);
        UpdateTheta();
    }

    void UpdateTheta()
    {
        if (thetaTextRef)
        {
            thetaTextRef.text = "" + theta;
        }
    }

    void UpdateCharge()
    {
        if (chargeFill)
        {
            chargeFill.fillAmount = 0.5f * ((float)currentValue / (float)theta);
        }
    }

    void UpdateLines(bool forceUpdate, bool onValidate = false)
    {
        if (lines == null) lines = new List<Line>();

        LineRenderer[] lr = GetComponentsInChildren<LineRenderer>();
        foreach (var l1 in lr)
        {
            bool found = false;
            foreach (var l2 in lines)
            {
                if (l2.lineRenderer == l1)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Line l = new Line();
                l.lineRenderer = l1;
                l.prevPos = new Vector3(-999999.0f, 0, 0);
                lines.Add(l);
            }
        }

        if (Brain.Get() == null) return;

        LineRenderer prefab = Brain.Get().lineRendererPrefab;
        if (prefab == null) return;

        int     nSegs = 20;

        for (int i = lines.Count; i < dendrites.Count; i++)
        {
            var l = new Line();
            l.lineRenderer = Instantiate(prefab, transform);
            l.lineRenderer.transform.position = transform.position + Vector3.left * radius;
            l.lineRenderer.positionCount = nSegs;
            l.prevPos = new Vector3(-999999.0f, 0, 0);

            lines.Add(l);
        }

        for (int i = dendrites.Count; i < lines.Count; i++)
        {
            if (onValidate) Destroy(lines[i].lineRenderer);
            else DestroyImmediate(lines[i].lineRenderer);
        }
        lines.RemoveAll((x) => x.lineRenderer == null);

        float tangentOffset = 2f;

        float aInc = 20.0f;
        float angle = 180.0f - (dendrites.Count - 1) * 0.5f * aInc;

        for (int i = 0; i < dendrites.Count; i++)
        {
            Dendrite d = dendrites[i];

            if (i >= lines.Count)
            {
                break;
            }

            if (d.neuron == null) continue;

            Vector3 p1 = d.neuron.transform.position + Vector3.right * d.neuron.radius;
            Vector3 p2 = p1 + Vector3.right * tangentOffset;

            Vector3 dir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);
            Vector3 p4 = transform.position + dir * radius;
            Vector3 p3 = p4 + dir * tangentOffset;

            if (d.neuron == this)
            {
                p2 += Vector3.up * tangentOffset;
                p3 += Vector3.up * tangentOffset;
            }

            angle += aInc;

            if ((Vector3.Distance(lines[i].prevPos, p4) > 0.0005f) || (forceUpdate))
            {
                for (int j = 0; j < nSegs; j++)
                {
                    float t = (float)j / (float)(nSegs - 1);
                    lines[i].lineRenderer.SetPosition(j, EvaluateBezier(t, p1, p2, p3, p4));
                }

                lines[i].prevPos = p1;
            }

            lines[i].lineRenderer.colorGradient = (d.type == DendriteType.Excitatory) ? (Brain.Get().excitatoryGradient) : (Brain.Get().inhibitoryGradient);
        }
    }

    void AddDendrite(Neuron neuron)
    { 
        // See if we have to make a new dendrite
        foreach (var a in dendrites)
        {
            if (a.neuron == this)
            {
                return;
            }
        }

        foreach (var a in dendrites)
        {
            if (a.neuron == null)
            {
                a.neuron = this;
                return;
            }
        }

        Dendrite d = new Dendrite();
        d.type = DendriteType.Excitatory;
        d.neuron = this;
        dendrites.Add(d);
    }

    void RemoveDendrite(Neuron neuron)
    {
        foreach (var a in dendrites)
        {
            if (a.neuron == this)
            {
                a.neuron = null;
            }
        }
    }

    public void SetValue(int val)
    {
        currentValue = val;
    }

    public int GetValue()
    {
        return (currentValue >= theta)?(1):(0);
    }

    public void ComputeOutput()
    {
        if (manualValue) return;

        currentValue = 0;
        foreach (var d in dendrites)
        {
            if (d.neuron)
            {
                if (d.type == DendriteType.Excitatory)
                {
                    currentValue += d.value;
                }
                else if (d.type == DendriteType.Inhibitory)
                {
                    if (d.value == 1)
                    {
                        currentValue = 0;
                        break;
                    }
                }
            }
        }
    }

    public void UpdateInputs()
    {
        foreach (var d in dendrites)
        {
            if (d.neuron)
            {
                d.value = d.neuron.GetValue();
            }
            else
            {
                d.value = 0;
            }
        }
    }

    static Vector3 EvaluateBezier(float t, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        float t2 = t * t;
        float t3 = t2 * t;
        float it = (1 - t);
        float it2 = it * it;
        float it3 = it2 * it;

        return it3 * p1 + 3 * it2 * t * p2 + 3 * it * t2 * p3 + t3 * p4;
    }
}
