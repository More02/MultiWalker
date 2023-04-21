using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Movement
{
    public class ThirdPersonController : MonoBehaviour
    {
        public Transform _target;
        [SerializeField] private float _distance = 5f;
        [SerializeField] private float _height = 2f;
        [SerializeField] private float _rotationSpeed = 5f;
        private float _currentRotationX = 0f;
        private float _currentRotationY = 0f;
        private Vector3 _offset;

        private void Start()
        {
            _offset = (transform.position - _target.position);
        }

        private void LateUpdate()
        {
            _currentRotationX += Input.GetAxis("Mouse X") * _rotationSpeed;
            _currentRotationY -= Input.GetAxis("Mouse Y") * _rotationSpeed;
            _currentRotationY = Mathf.Clamp(_currentRotationY, -90f, 90f);
            var rotation = Quaternion.Euler(_currentRotationY, _currentRotationX, 0f);
            //Vector3 position = rotation * new Vector3(0f, height, -distance) + target.position;
            transform.rotation = rotation;
            transform.position = _target.position + _offset;
        }
    }
}
