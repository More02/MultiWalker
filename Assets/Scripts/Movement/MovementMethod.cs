using System;
using FishNet;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;

namespace Movement
{
    /// <summary>
    /// Класс, отвечающий за перемещение игрока
    /// </summary>
    public class MovementMethod : NetworkBehaviour
    {
        private const float MoveSpeed = 10f;

        private void OnEnable()
        {
           // InstanceFinder.ClientManager.OnClientConnectionState += SetTarget;
            SetTarget();
        }
        
        public void SetTarget()
        {
            ThirdPersonController.Instance.Target = transform;
            Debug.Log("SetTarget");
        }

        // public void SetTarget(ClientConnectionStateArgs сlientConnectionStateArgs)
        // {
        //     ThirdPersonController.Instance.Target = transform;
        //     Debug.Log("SetTarget");
        // }

        // public override void OnStartLocalPlayer()
        // {
        //     ThirdPersonController.Instance.Target = transform;
        // }

        private void FixedUpdate()
        {
            if (!IsOwner) return;
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