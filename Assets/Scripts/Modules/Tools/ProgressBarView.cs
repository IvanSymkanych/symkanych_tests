using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Tools
{
    [RequireComponent(typeof(Slider))]
    public class ProgressBarView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI progressText;

        private Slider _slider;
        private Sequence _sequence;

        public void Initialize() =>
            _slider = GetComponent<Slider>();

        public void SetProgressInstantly(float value)
        {
            progressText.SetText(GetPercentText(value));
            _slider.value = value;
        }

        public void SetProgressAnimated(float value)
        {
            const float duration = 0.7f;

            _sequence?.Kill();
            _sequence = DOTween.Sequence()
                .Append(progressText.DOText(GetPercentText(value), duration))
                .Join(_slider.DOValue(value, duration));
        }

        private string GetPercentText(float value)
        {
            var percent = value * 100;
            return $"{percent}%";
        }
    }
}