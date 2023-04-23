using UnityEngine;

namespace Movement
{
    public class ThirdPersonController : MonoBehaviour
    {
        private const float Distance = 3f;
        private const float Height = 2f;
        private const float Sensitivity = 5f;
        private const float Smooth = 10f;
        private float _currentRotationX;
        private float _currentRotationY;
        private float _mouseX;
        private float _mouseY;
        private Vector3 _offset;

        public static ThirdPersonController Instance { get; private set; }
        public Transform Target { get; set; }

        private void Awake()
        {
            Instance = this;
            Cursor.lockState = CursorLockMode.Confined; 
        }

        private void LateUpdate()
        {
            if (Target is null) return;
            GetMousePosition();
            FollowTarget();
        }

        private void GetMousePosition()
        {
            _mouseX = Input.GetAxis("Mouse X") * Sensitivity;
            _mouseY = Input.GetAxis("Mouse Y") * Sensitivity;
            _currentRotationX += _mouseX;
            _currentRotationY -= _mouseY;
            _currentRotationY = Mathf.Clamp(_currentRotationY, -90f, 90f);
        }

        private void FollowTarget()
        {
            var rotation = Quaternion.Euler(0f, _currentRotationX, 0f);
            _offset = new Vector3(0f, Height, -Distance);
            _offset = rotation * _offset;
            var targetPosition = Target.position;
            transform.position = targetPosition + _offset;
            transform.LookAt(targetPosition);
            //transform.rotation = Quaternion.Euler(_currentRotationY, _currentRotationX, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(_currentRotationY, _currentRotationX, 0f), Smooth*Time.fixedDeltaTime);
            // Target.rotation = Quaternion.Euler(0f, _currentRotationX, 0f);
            Target.rotation = Quaternion.Lerp(Target.rotation, Quaternion.Euler(0f, _currentRotationX, 0f), Smooth*Time.fixedDeltaTime);
        }
    }
}