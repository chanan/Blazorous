using System.Threading.Tasks;

namespace Blazorous
{
    public interface IBlazorousInterop
    {
        Task<string> Css(string css);
        Task<string> Css(string css, string debug);
        Task<string> Fontface(string fontface, string debug);
        Task<string> Keyframes(string keyframes, string debug);
        Task<string> PolishedMixin(string mixin, string debug);
    }
}