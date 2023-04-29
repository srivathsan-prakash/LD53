using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int wrapVariant = 0;
    public int sizeVariant = 0;
    public int ribbonVariant = 0;

    [SerializeField] private int defaultVariant = -1;

    private void OnEnable()
    {
        Decoration.UpdateItem += UpdateDecor;
    }

    private void OnDisable()
    {
        Decoration.UpdateItem -= UpdateDecor;
    }

    private void UpdateDecor(Decoration.Type decorType, int variant)
    {
        switch (decorType)
        {
            case Decoration.Type.Wrap:
                if (wrapVariant == variant)
                    wrapVariant = defaultVariant;
                else
                    wrapVariant = variant;
                break;
            case Decoration.Type.Box:
                if (sizeVariant == variant)
                    sizeVariant = defaultVariant;
                else
                    sizeVariant = variant;
                break;
            case Decoration.Type.Ribbon:
                if (ribbonVariant == variant)
                    ribbonVariant = defaultVariant;
                else
                    ribbonVariant = variant;
                break;
        }

        Debug.LogFormat("Current Item - Wrap: {0}, BoxSize: {1}, Ribbon: {2}", wrapVariant, sizeVariant, ribbonVariant);
    }
}
