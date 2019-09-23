using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElementsBook
{
    public interface IElementProvider
    {
        Task<List<ElementData>> GetElements();
    }
}