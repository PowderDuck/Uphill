using DG.Tweening;
using UnityEngine;

namespace Uphill.Scripts.Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private Transform _text2D = default!;
        [SerializeField] private Vector3 _initialScale = Vector3.one;
        [SerializeField] private float _scaleDuration = 0.75f;

        [SerializeField] private Transform _options = default!;
        [SerializeField] private float _optionsHeight = 60f;
        [SerializeField] private float _optionsDuration = 0.5f;

        private void Start()
        {
            var previousScale = _text2D.localScale;
            _text2D.localScale = _initialScale;

            DOVirtual.DelayedCall(1f, () =>
            {
                _text2D.gameObject.SetActive(true);
                _text2D
                    .DOScale(previousScale, _scaleDuration)
                    .SetEase(Ease.InExpo)
                    .OnComplete(() =>
                    {
                        _options
                            .DOMoveY(_optionsHeight, _optionsDuration)
                            .SetEase(Ease.OutBounce);
                    });

            });
        }
    }
}
