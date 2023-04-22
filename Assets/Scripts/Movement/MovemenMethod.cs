using System;
using Mirror;
using UnityEngine;

namespace Movement
{
    public class MovemenMethod : NetworkBehaviour
    {
        private const float MoveSpeed = 10f;
        private Transform _cameraTransform;

        public override void OnStartLocalPlayer()
        {
            _cameraTransform = Camera.main!.transform;
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
            var movement = _cameraTransform.right * horizontal + _cameraTransform.forward * vertical;
            movement.y = 0f;
            transform.position += movement * (MoveSpeed * Time.deltaTime);
        }
    }
}