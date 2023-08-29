using TMPro;
using UnityEngine;
using Zenject;

namespace test_sber
{
    public class WidgetEnemyFinalStatus : WidgetBase
    {
        [Inject] 
        private LabelsResolver _resolver;
        
        [SerializeField]
        private TMP_Text _labelResult; 
        
        public void Build(bool spotted)
        {
            _labelResult.SetText(spotted ? _resolver.Resolve("spotted") : _resolver.Resolve("not_spotted"));
        }
    }
}