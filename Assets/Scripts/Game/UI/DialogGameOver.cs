using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace test_sber
{
    public class DialogGameOver : DialogBase
    {
        [Inject] 
        private LabelsResolver _resolver;
        [Inject] 
        private UIController _uiController;
        
        [SerializeField]
        private Transform _containerEnemiesStatus; 
        [SerializeField]
        private TMP_Text _labelResult; 
        
        // todo LabelsResolver закладывается под локализацию
        public void Build(bool playerKilled, IEnumerable<bool> enemiesSpottedStatus)
        {
            _labelResult.SetText(playerKilled ? _resolver.Resolve("lose") : _resolver.Resolve("win"));
            foreach (var enemyStatus in enemiesSpottedStatus)
            {
                var widget = _uiController.CreateWidget<WidgetEnemyFinalStatus>(_containerEnemiesStatus);
                widget.Build(enemyStatus);
            }
        }
    }
}