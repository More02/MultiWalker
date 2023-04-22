using Mirror;
using UnityEngine;

namespace Movement
{
    public class MovementMethod : NetworkBehaviour, IMovable
    {
        private const float MoveSpeed = 10f;
        private Transform _cameraTransform;
        private TypesOfMove _typesOfMove;
        private TypesOfMove _typeOfMove;
        private TypesOfMove _nowTypeOfMovement;

        public override void OnStartLocalPlayer()
        {
            _cameraTransform = Camera.main!.transform;
            ThirdPersonController.Instance.Target = transform;
            NowTypeOfMovement = TypesOfMove.Idle;
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

        public TypesOfMove NowTypeOfMovement { get; set; }
    }
}