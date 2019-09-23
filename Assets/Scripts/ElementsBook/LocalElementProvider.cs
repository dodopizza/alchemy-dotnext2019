using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ElementsBook
{
    public class LocalElementProvider : IElementProvider
    {
        public Task<List<ElementData>> GetElements()
        {
            return Task.FromResult(new List<ElementData>
            {
                new ElementData { color = Color.blue },
                new ElementData { color = Color.red },
                new ElementData { color = Color.black },
                new ElementData { color = Color.cyan },
                new ElementData { color = Color.gray },
                new ElementData { color = Color.magenta },
                new ElementData { color = Color.yellow },
            });
        }
    }
}