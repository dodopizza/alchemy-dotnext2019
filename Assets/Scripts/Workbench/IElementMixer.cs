using System.Collections.Generic;
using System.Threading.Tasks;

namespace Workbench
{
    public interface IElementMixer
    {
        Task<MixResult> MixElements(IEnumerable<ElementData> elementsData);
    }
}