using UnityEngine;

namespace Movement
{
    /// <summary>
    /// Класс, отвечающий за контроллер от третьего лица
    /// </summary>
    public class ThirdPersonController : MonoBehaviour
    {
        private const float Distance = 3f;
        private const float Height = 2f;
        private const float Sensitivity = 5f;
        private const float Smooth = 10f;
        private float _currentRotationX;
        private float _currentRotationY;
        private Vector3 _offset;

        public static ThirdPersonController Instance { get; private set; }
        public Transform Target { get; set; }

        private void Awake()
        {
            Instance = this;
            //Cursor.lockState = CursorLockMode.Confined;
        }

        private void LateUpdate()
        {
           // Debug.Log(Target.gameObject.name);
            if (Target is null) return;
            GetMousePosition();
            FollowTarget();
        }

        private void GetMousePosition()
        {
            _currentRotationX += Input.GetAxis("Mouse X") * Sensitivity;
            _currentRotationY -= Input.GetAxis("Mouse Y") * Sensitivity;
            _currentRotationY = Mathf.Clamp(_currentRotationY, -90f, 90f);
        }

        private void FollowTarget()
        {
            //Debug.Log("FollowTarget");
            var rotation = Quaternion.Euler(0f, _currentRotationX, 0f);
            _offset = new Vector3(0f, Height, -Distance);
            _offset = rotation * _offset;
            var targetPosition = Target.position;
            var playerTransform = transform;
            playerTransform.position = targetPosition + _offset;
            playerTransform.LookAt(targetPosition);
            playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation,
                Quaternion.Euler(_currentRotationY, _currentRotationX, 0f), Smooth * Time.fixedDeltaTime);
            Target.rotation = Quaternion.Lerp(Target.rotation, Quaternion.Euler(0f, _currentRotationX, 0f),
                Smooth * Time.fixedDeltaTime);
        }
    }
}