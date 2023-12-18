using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class LoadingScreenBehaviour : MonoBehaviour
    {
        private const float FadeDuration = 1f;

        [SerializeField] private Image loadingImage;
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private AnimationCurve loadingTextAnimationCurve;

        private CanvasGroup _canvasGroup;
        private Tween _fadeTween;
        private Tween _loadingTween;
        private Sequence _loadingTextSequence;

        public void Initialize()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.interactable = false;
        }

        public void Show()
        {
            _canvasGroup.blocksRaycasts = true;

            _fadeTween?.Kill();
            _fadeTween = _canvasGroup.DOFade(1, FadeDuration);

            _loadingTween?.Kill();
            _loadingTween = loadingImage
                .DOFillAmount(1, 3)
                .From(0)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);

            PlayLoadingTextAnimation();
        }

        public void Hide()
        {
            _fadeTween?.Kill();
            _loadingTween?.Kill();
            _loadingTextSequence?.Kill();
            _canvasGroup.blocksRaycasts = false;
            _fadeTween = _canvasGroup.DOFade(0, FadeDuration);
        }

        private void PlayLoadingTextAnimation()
        {
            _loadingTextSequence?.Kill();
            
            var animator = new DOTweenTMPAnimator(loadingText);

            for (var i = 0; i < animator.textInfo.characterCount; i++)
            {
                var endValue = animator.GetCharOffset(i) + new Vector3(0, Random.Range(40, 60), 0);

                _loadingTextSequence = DOTween.Sequence()
                    .SetLoops(-1, LoopType.Restart)
                    .SetUpdate(UpdateType.Normal, true)
                    .Append(animator.DOOffsetChar(i, endValue, Random.Range(0.4f, 1))
                        .SetDelay(Random.Range(1, 3))
                        .SetEase(loadingTextAnimationCurve));
            }
        }
    }
}