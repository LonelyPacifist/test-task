using UnityEngine;

namespace test_sber
{
    public class BillboardFx : MonoBehaviour
    {
        private Transform _cameraTransform;
        
        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            transform.forward = _cameraTransform.position - transform.position;
        }
    }
}