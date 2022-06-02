using System;
using System.Collections.Generic;

public static class ServicesLocator
{
    private static Dictionary<Type, object> servicesList = new Dictionary<Type, object>();

    public static void AddService<T>(T service)
    {
        servicesList[typeof(T)] = service;
    }

    public static T GetService<T>()
    {
        return (T)servicesList[typeof(T)];
    }

}

