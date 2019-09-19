using System.Collections.Generic;
using UnityEngine;

namespace ElementProvider
{
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
                new ElementData { color = Color.magenta },
                new ElementData { color = Color.yellow },
            };
        }
    }
}