using System;
using UnityEngine;
using Zenject;

namespace test_sber
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        [Inject]
        private Settings _settings;

        [Serializable]
        public class Settings
        {
            public float speed;
            public float jumpForce;
            public float mouseSensitivity;
            public float gravity;
        }
        
        [SerializeField] 
        private GameObject _head;

        private CharacterController _controller;
        private Vector3 _rotation;
        private bool _isOnGround;
        private float _verticalVelocity;

        [Inject]
        public void Init()
        {
            _controller = GetComponent<CharacterController>();
            _controller.Move(Vector3.zero);
        }

        private void Update()
        {
            Move();
            Rotate();
        }

        private void Move()
        {
            _isOnGround = _controller.isGrounded;

            _verticalVelocity += _settings.gravity * Time.deltaTime;

            if (_isOnGround && _verticalVelocity < 0) _verticalVelocity = _settings.gravity;

            if (Input.GetButtonDown("Jump") && _isOnGround) _verticalVelocity += _settings.jumpForce;

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
    }
}