using System;

namespace Modules.Tools
{
    public interface IProgressBarController
    {
        event Action OnValueMinimum;
        int MaxValue { get; }
        bool IsMaxValue { get; }
        void SetValueAnimated(int value);
        void AddValueAnimated(int value);
        float GetPercentValue();
    }
}