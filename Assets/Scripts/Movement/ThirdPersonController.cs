using UnityEngine;

namespace Movement
{
    public class ThirdPersonController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        private const float Distance = 5f;
        private const float Height = 2f;
        private const float Sensitivity = 5f;
        private float _currentRotationX;
        private float _currentRotationY;
        private float _mouseX;
        private float _mouseY;
        private Vector3 _offset;

        private void LateUpdate()
        {
            _mouseX = Input.GetAxis("Mouse X") * Sensitivity;
            _mouseY = Input.GetAxis("Mouse Y") * Sensitivity;
            _currentRotationX += _mouseX;
            _currentRotationY -= _mouseY;
            _currentRotationY = Mathf.Clamp(_currentRotationY, -90f, 90f);

            var rotation = Quaternion.Euler(0f, _currentRotationX, 0f);
            _offset = new Vector3(0f, Height, -Distance);
            _offset = rotation * _offset;

            var targetPosition = _target.position;
            transform.position = targetPosition + _offset;
            transform.LookAt(targetPosition);
            transform.rotation = Quaternion.Euler(_currentRotationY, _currentRotationX, 0f);
        }
    }
}