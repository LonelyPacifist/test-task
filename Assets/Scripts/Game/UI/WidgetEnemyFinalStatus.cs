using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace test_sber
{
    public class WidgetEnemyFinalStatus : WidgetBase
    {
        [Inject] 
        private LabelsResolver _resolver;
        
        [SerializeField]
        private TMP_Text _labelResult; 
        [SerializeField]
        private Image _imageBgr; 
        [SerializeField]
        private Color _colorSpotted; 
        [SerializeField]
        private Color _colorNotSpotted; 
        
        public void Build(bool spotted)
        {
            _labelResult.SetText(spotted 
                ? _resolver.Resolve(Labels.EnemyStatusSpotted) 
                : _resolver.Resolve(Labels.EnemyStatusNotSpotted));
            _imageBgr.color = spotted ? _colorSpotted : _colorNotSpotted;
        }
    }
}