using Mirror;
using UnityEngine;

namespace Movement
{
    /// <summary>
    /// Класс, отвечающий за перемещение игрока
    /// </summary>
    public class MovementMethod : NetworkBehaviour
    {
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
            transform.GetChild(0).rotation = Quaternion.LookRotation(movement);
            targetTransform.position += movement * (MoveSpeed * Time.deltaTime);
        }
    }
}