using System.Collections.Generic;
using UnityEngine;

public static class ComponentExtensions
{
    public static T[] GetComponentsInAllChildren<T>(this GameObject gameObject)
    {
        List<T> components = new List<T>();
        GetComponentsInAllChildren<T>(gameObject.transform, components);

        return components.ToArray();
    }

    public static T[] GetComponentsInAllChildren<T>(this Component component)
    {
        List<T> components = new List<T>();
        GetComponentsInAllChildren<T>(component.transform, components);

        return components.ToArray();
    }

    private static void GetComponentsInAllChildren<T>(Transform transform, List<T> components)
    {
        T component = transform.GetComponent<T>();
        if (component != null)
        {
            components.Add(component);
        }

        foreach (Transform child in transform)
        {
            GetComponentsInAllChildren<T>(child, components);
        }
    }
}
