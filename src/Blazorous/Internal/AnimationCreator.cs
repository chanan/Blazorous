namespace Blazorous.Internal
{
    internal class AnimationCreator : IAnimationCreator
    {
        private readonly IBlazorousInterop _blazorousInterop;
        private readonly ICssCreator _cssCreator;

        internal AnimationCreator(IBlazorousInterop blazorousInterop, ICssCreator cssCreator)
        {
            _blazorousInterop = blazorousInterop;
            _cssCreator = cssCreator;
        }

        public IAnimation CreateNew()
        {
            return new Animation(_blazorousInterop, _cssCreator);
        }
    }
}
