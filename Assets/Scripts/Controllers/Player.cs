using System;
using UnityEngine;
using Zenject;

namespace test_sber
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private GameObject _head;
        
        private CharacterController _controller;
        private Vector3 _rotation;
        private Settings _settings;

        private Vector3 _viewportTarget = new Vector3(0.5F, 0.5F, 0);
        
        [Serializable]
        public class Settings
        {
            public float speed;
            public float jumpForce;
            public float mouseSensitivity;
            public float gravity;
            public float maxDiscoverDistance;
        }
        
        private bool _isOnGround;
        private float _verticalVelocity; 
        
        [Inject]
        public void Init(Settings settings)
        {
            _settings = settings;
            _controller = GetComponent<CharacterController>();
            _controller.Move(Vector3.zero);
        }
    
        private void Update()
        {
            Move();
            Rotate();
            TryDiscoverEnemy();
        }

        private void Move()
        {
            _isOnGround = _controller.isGrounded;

            _verticalVelocity += _settings.gravity * Time.deltaTime; 
            
            if (_isOnGround && _verticalVelocity < 0)
            {
                _verticalVelocity = _settings.gravity; 
            }
    
            if (Input.GetButtonDown("Jump") && _isOnGround)
            {
                _verticalVelocity += _settings.jumpForce;
            }

            var movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            var resultingMovement = transform.TransformDirection(movement) * (_settings.speed * Time.deltaTime) 
                                    + Vector3.up * _verticalVelocity;

            _controller.Move(resultingMovement);
        }

        private void Rotate()
        {
            _rotation.x += Input.GetAxis("Mouse X") * _settings.mouseSensitivity;
            _rotation.y -= Input.GetAxis("Mouse Y") * _settings.mouseSensitivity;
            _rotation.x = Mathf.Repeat(_rotation.x, 360);
            _rotation.y = Mathf.Clamp(_rotation.y, -80, 80);
            transform.eulerAngles = new Vector3(0, _rotation.x, 0);
            _head.transform.eulerAngles = new Vector3(_rotation.y, _rotation.x, 0);
        }

        private void TryDiscoverEnemy()
        {
            RaycastHit hit;
            var ray = Camera.main.ViewportPointToRay(_viewportTarget); //todo убрать Camera.main

            if (Physics.Raycast(ray, out hit, _settings.maxDiscoverDistance, 1 << LayerMask.NameToLayer("Enemy")))
            {
                Debug.Log("enemy hit");
            }
        }
    }
}
