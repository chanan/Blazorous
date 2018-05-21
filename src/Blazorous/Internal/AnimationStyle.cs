using System;

namespace Blazorous.Internal
{
    internal class AnimationStyle
    {
        public string Duration { get; set; }
        public Action<Animation> Animation { get; set; }
    }
}
