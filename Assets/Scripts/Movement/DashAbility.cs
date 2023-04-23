using System;
using System.Collections;
using Mirror;
using UnityEngine;

namespace Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class DashAbility : NetworkBehaviour
    {
        private Rigidbody _rigidbody;
        private readonly Color _dashedColor = new Color((float)0.6226415, (float)0.2196867, (float)0.2196867);
        private Color _baseColor = Color.white;
        [SerializeField] private float _distance = 5f;
        //[SerializeField] private Transform _mesh;

        private const float Force = 3;
        private Vector3 _startPosition;

        private static readonly int _red = Shader.PropertyToID("Red");

        private static readonly int _color = Shader.PropertyToID("_Color");
        //private Transform _cameraTransform;

        public bool IsDashing { get; private set; }

        public static DashAbility Instance { get; private set; }

        private void Start()
        {
            Instance = this;
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        }

        public override void OnStartLocalPlayer()
        {
            //_rigidbody = gameObject.GetComponent<Rigidbody>();
            //_cameraTransform = Camera.main!.transform;
        }

        private void Update()
        {
            if (!isLocalPlayer) return;
            if (!Input.GetMouseButtonDown(0)) return;
            if (IsDashing) return;
            IsDashing = true;
            _startPosition = transform.position;
            StartCoroutine(Dash());
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!isLocalPlayer) return;
            if (!collision.gameObject.CompareTag("Player")) return;
            var skinnedMeshRenderers = collision.transform.root.GetComponentsInChildren(typeof(SkinnedMeshRenderer), true);

            foreach (var component in skinnedMeshRenderers)
            {
                var skinnedMeshRenderer = (SkinnedMeshRenderer)component;
                skinnedMeshRenderer.materials[0].SetColor(_color, _dashedColor);
            }
        }

        private IEnumerator Dash()
        {
            if (!IsDashing) yield break;
            while (IsDashing)
            {
                _rigidbody.AddForce(transform.GetChild(0).forward * Force, ForceMode.Impulse);
                //_rigidbody.AddForce(_cameraTransform.forward * Force, ForceMode.Impulse);
                var currentDistance = transform.position - _startPosition;
                if (currentDistance.magnitude >= _distance)
                {
                    IsDashing = false;
                    _rigidbody.velocity = Vector3.zero;
                    yield break;
                }

                yield return null;
            }
        }
    }
}