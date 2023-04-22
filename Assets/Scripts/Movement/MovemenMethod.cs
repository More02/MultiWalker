using System;
using Mirror;
using UnityEngine;

namespace Movement
{
    public class MovemenMethod : NetworkBehaviour
    {
        private const float MoveSpeed = 10f;
        private Transform _cameraTransform;

        [Obsolete("Obsolete")]
        private void FixedUpdate()
        {
            if (hasAuthority)
            {
                Move();
                _cameraTransform = Camera.main!.transform;
            }
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