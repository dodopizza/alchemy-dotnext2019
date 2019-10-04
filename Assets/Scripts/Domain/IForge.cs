using System.Threading.Tasks;

namespace Domain
{
    internal interface IForge
    {
        void AddElement(Element element);

        void Clear();
        
        Task<MixResult> GetMixResult();
    }
}