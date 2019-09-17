using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ElementProviderFactory
{
    public static IElementProvider Provider { get; } = new LocalElementProvider();
}

public interface IElementProvider
{
    List<ElementData> GetElements();
}

public class LocalElementProvider : IElementProvider
{
    public List<ElementData> GetElements()
    {
        return new List<ElementData>
        {
            new ElementData { color = Color.blue },
            new ElementData { color = Color.red },
            new ElementData { color = Color.black },
            new ElementData { color = Color.cyan },
            new ElementData { color = Color.gray },
            new ElementData { color = Color.green },
        };
    }
}