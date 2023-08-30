using System;
using UnityEngine;
using Zenject;

namespace test_sber
{
    public class Spotter : MonoBehaviour
    {
        [Inject]
        private Settings _settings;
        
        [Serializable]
        public class Settings
        {
            public Vector2 viewportAimCoords;
            public float maxDiscoverDistance;
            public string spottableLayerTag;
        }
        
        private Vector3 ViewportTarget => new Vector3(_settings.viewportAimCoords.x, _settings.viewportAimCoords.y, 0); 
        private Camera _camera;
        private int _targetLayerMask;
        
        [Inject]
        public void Init()
        {
            _camera = Camera.main;
            _targetLayerMask = 1 << LayerMask.NameToLayer(_settings.spottableLayerTag);
        }

        private void Update()
        {
            var ray = _camera.ViewportPointToRay(ViewportTarget);

            if (Physics.Raycast(ray, out var hit, _settings.maxDiscoverDistance, _targetLayerMask))
            {
                var spottable = hit.collider.GetComponentInParent<ISpottable>();
                if (spottable != null)
                {
                    spottable.TrySpot(Time.deltaTime);
                }
            }
        }
    }
}