using Mirror;
using UnityEngine;

namespace Movement
{
    public class MovementMethod : NetworkBehaviour
    {
        [SerializeField] private Transform _mesh;
        private const float MoveSpeed = 10f;

        public override void OnStartLocalPlayer()
        {
            ThirdPersonController.Instance.Target = transform;
        }

        private void FixedUpdate()
        {
            if (!isLocalPlayer) return;
            Move();
        }

        private void Move()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var targetTransform = transform;
            var movement = targetTransform.right * horizontal + targetTransform.forward * vertical;
            movement.y = 0f;
            if (movement.magnitude == 0) return;
            _mesh.rotation = Quaternion.LookRotation(movement);
            targetTransform.position += movement * (MoveSpeed * Time.deltaTime);
            //_rigidbody.MovePosition(targetTransform.position + movement * (MoveSpeed * Time.fixedDeltaTime));
        }
    }
}