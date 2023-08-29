using System;
using UnityEngine;
using Zenject;

namespace test_sber
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameObject _head;

        private CharacterController _controller;
        private Vector3 _rotation;
        [Inject]
        private Settings _settings;
        [Inject]
        private GameSettingsInstaller.BaseSettings _baseSettings;

        private Vector3 ViewportTarget => new Vector3(_baseSettings.viewportAimCoords.x, _baseSettings.viewportAimCoords.y, 0); 
        private int _enemyLayerMask;
        private Camera _camera;

        [Serializable]
        public class Settings
        {
            public float speed;
            public float jumpForce;
            public float mouseSensitivity;
            public float gravity;
            public float maxDiscoverDistance;
            public string enemyLayerTag;
        }

        private bool _isOnGround;
        private float _verticalVelocity;

        [Inject]
        public void Init()
        {
            _controller = GetComponent<CharacterController>();
            _controller.Move(Vector3.zero);
            _enemyLayerMask = 1 << LayerMask.NameToLayer(_settings.enemyLayerTag);
            _camera = Camera.main;
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

        private void TryDiscoverEnemy()
        {
            RaycastHit hit;
            var ray = _camera.ViewportPointToRay(ViewportTarget);

            if (Physics.Raycast(ray, out hit, _settings.maxDiscoverDistance, _enemyLayerMask))
            {
                var enemy = hit.collider.GetComponentInParent<ISpottable>();
                if (enemy != null)
                {
                    enemy.TrySpot(Time.deltaTime);
                }
            }
        }
    }
}