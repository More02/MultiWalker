using UnityEngine;

namespace Movement
{
    public class MovemenMethod : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 10f;
        [SerializeField] private Transform _cameraTransform;

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var movement = _cameraTransform.right * horizontal + _cameraTransform.forward * vertical;
            movement.y = 0f;
            transform.position += movement * (_moveSpeed * Time.deltaTime);
        }
    }
}