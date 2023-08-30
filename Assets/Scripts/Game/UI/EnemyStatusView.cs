using UnityEngine;
using UnityEngine.UI;

namespace test_sber
{
    public class EnemyStatusView : MonoBehaviour
    {
        [SerializeField] 
        private Enemy _enemy;
        
        [SerializeField] 
        private Image _imageFill;
        [SerializeField] 
        private Color _colorNotFilled;
        [SerializeField] 
        private Color _colorFilled;
        
        private void Update()
        {
            var filledAmount = _enemy.TotalSpottedTime / _enemy.EnemySettings.spottingTime;
            _imageFill.fillAmount = filledAmount;
            _imageFill.color = Color.Lerp(_colorNotFilled, _colorFilled, filledAmount);
        }
    }
}