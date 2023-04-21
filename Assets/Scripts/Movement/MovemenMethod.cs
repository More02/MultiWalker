using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Movement
{
    public class MovemenMethod : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        [FormerlySerializedAs("camera")] public Transform cameraTransform;
        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var direction = new Vector3(horizontal, 0f, vertical) * (_moveSpeed * Time.deltaTime);
            //movement = movement.x * camera.forward;
            var movement3 = cameraTransform.right * direction.x +  cameraTransform.forward*direction.z;
            //movement3.y = 0f;
            var mov = cameraTransform.right * horizontal + cameraTransform.forward * vertical;
            mov.y = 0f;
            transform.position += mov* (_moveSpeed * Time.deltaTime);
        }

        public void SetMoveSpeed(float moveSpeed)
        {
            this._moveSpeed = moveSpeed;
        }
    }
}