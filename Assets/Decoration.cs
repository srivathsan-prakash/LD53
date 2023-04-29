using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoration : MonoBehaviour
{
    public enum Type
    {
        Wrap,
        Box,
        Ribbon
    }

    public static Action<Type, int> UpdateItem;

    [SerializeField] private Type type;
    [SerializeField] private int variation;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (collision.tag.Equals("Player"))
            {
                Debug.LogFormat("Space pressed on decoration: {0}, variation: {1}", type, variation);
                UpdateItem?.Invoke(type, variation);
            }
        }
    }
}
