using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElementsBook
{
    public class LocalElementProvider : IElementProvider
    {
        public Task<List<ElementData>> GetElements()
        {
            return Task.FromResult(new List<ElementData>
            {
                new ElementData { id = 1, spriteName = "Fire"},
                new ElementData { id = 2, spriteName = "Water"}
            });
        }
    }
}