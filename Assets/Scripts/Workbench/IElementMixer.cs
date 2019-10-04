using System.Collections.Generic;
using System.Threading.Tasks;

namespace Workbench
{
    public interface IElementMixer
    {
        Task<MixResultOld> MixElements(IEnumerable<ElementData> elementsData);
    }
}