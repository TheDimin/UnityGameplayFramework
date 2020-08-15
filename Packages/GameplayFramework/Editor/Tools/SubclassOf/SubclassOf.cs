using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SubclassOf<T> where T : class
{
    private Type classType;
    [SerializeField]
    private string typeName;

    SubclassOf(Type type)
    {
        if (!type.IsSubclassOf(typeof(T)))
            throw new InvalidCastException("");

        classType = type;
        typeName = type.FullName;
    }


    T CreateInstance(params object[] parameters)
    {
        return (T)Activator.CreateInstance(GetType(), parameters);
    }

    public new Type GetType()
    {
        if (classType == null)
            classType = Type.GetType(typeName);

        return classType;
    }
}
