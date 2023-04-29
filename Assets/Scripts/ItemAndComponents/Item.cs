using System;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Dictionary<CompType, int> Components;

    private const int defaultVariant = -1;

    private void Awake()
    {
        InitializeComponentsDictionary();
    }

    private void OnEnable()
    {
        Events.UpdateItemComponent += UpdateComponent;
    }

    private void OnDisable()
    {
        Events.UpdateItemComponent -= UpdateComponent;
    }

    private void UpdateComponent(CompType compType, int variant)
    {
        Components[compType] = variant;

        DisplayComponents();
    }

    private void InitializeComponentsDictionary()
    {
        Components = new Dictionary<CompType, int>();

        foreach (CompType type in Enum.GetValues(typeof(CompType)))
            Components.Add(type, defaultVariant);
    }

    private void DisplayComponents()
    {
        string result = string.Empty;

        foreach (KeyValuePair<CompType, int> component in Components)
            result += component.Key + " : " + component.Value + " | ";

        Debug.Log(result);
    }
}
