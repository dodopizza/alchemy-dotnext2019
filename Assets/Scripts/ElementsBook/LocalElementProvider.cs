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
                new ElementData { r = Color.blue.r, g = Color.blue.g, b = Color.blue.b },
                new ElementData { r = Color.red.r, g = Color.red.g, b = Color.red.b },
                new ElementData { r = Color.black.r, g = Color.black.g, b = Color.black.b },
                new ElementData { r = Color.cyan.r, g = Color.cyan.g, b = Color.cyan.b },
                new ElementData { r = Color.gray.r, g = Color.gray.g, b = Color.gray.b },
                new ElementData { r = Color.magenta.r, g = Color.magenta.g, b = Color.magenta.b },
                new ElementData { r = Color.yellow.r, g = Color.yellow.g, b = Color.yellow.b },
            });
        }
    }
}