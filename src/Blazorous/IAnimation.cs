using System;

namespace Blazorous
{
    public interface IAnimation
    {
        IAnimation AddKeyframe(string keyframe, Action<ICss> css);
    }
}
