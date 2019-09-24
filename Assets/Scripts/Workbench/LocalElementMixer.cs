using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Workbench
{
    public class LocalElementMixer : IElementMixer
    {
        public Task<MixResult> MixElements(IEnumerable<ElementData> elementsData)
        {
            if (!elementsData.Any())
            {
                return null;
            }

            return Task.FromResult(
                new MixResult
                {
                    success = true,
                    result = new ElementData
                    {
                        r = 0.5f,
                        g = 0.1f,
                        b = 0.4f
                    }
                });
        }
    }
}