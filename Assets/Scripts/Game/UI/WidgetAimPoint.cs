using UnityEngine;
using Zenject;

namespace test_sber
{
    [RequireComponent(typeof(RectTransform))]
    public class WidgetAimPoint : MonoBehaviour
    {
        [Inject] 
        private Spotter.Settings _settings;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.anchoredPosition = new Vector2(Screen.width * _settings.viewportAimCoords.x,
                Screen.height * _settings.viewportAimCoords.y);
        }
    }
}