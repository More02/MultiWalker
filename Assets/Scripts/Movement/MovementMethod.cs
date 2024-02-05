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

        private void Start()
        {
           // InstanceFinder.ClientManager.OnClientConnectionState += SetTarget;
            if (Owner.IsLocalClient) SetTarget();
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
            if (!Owner.IsLocalClient) return;
           // Debug.Log("Owner.IsLocalClient from MovementMethod = "+Owner.IsLocalClient + " game object name = " + gameObject.name);
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

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(200, 10, Screen.width, Screen.height));
            GUILayout.Label($"Target = {ThirdPersonController.Instance.Target}");
            GUILayout.Label($"GameObject Name = {gameObject.name}");
            GUILayout.EndArea();
        }
    }
}