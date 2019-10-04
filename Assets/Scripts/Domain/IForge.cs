using System.Threading.Tasks;

namespace Domain
{
    internal interface IForge
    {
        void AddElement(Element element);
        Task<MixResult> GetMixResult();
    }
}