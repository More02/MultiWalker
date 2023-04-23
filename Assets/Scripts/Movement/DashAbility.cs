using System.Collections;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;

namespace Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class DashAbility : NetworkBehaviour
    {
        private Rigidbody _rigidbody;
        private readonly Color _dashedColor = new Color((float)0.6226415, (float)0.2196867, (float)0.2196867);
        private readonly Color _baseColor = Color.white;
        [SerializeField] private float _distance = 5f;
        [SerializeField] private float _disabledTime = 3f;
        private Component[] _skinnedMeshRenderers;
        private int _countOfSuccessDash;
        private bool _isAvailableForDash = true;

        private const float Force = 3f;
        private Vector3 _startPosition;

        private static readonly int _color = Shader.PropertyToID("_Color");
        private Transform _cameraTransform;
        private Transform _collisionRoot;

        private IEnumerator _dashCoroutine;

        public bool IsDashing { get; private set; }

        public static DashAbility Instance { get; private set; }

        private void Start()
        {
            Instance = this;
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        public override void OnStartLocalPlayer()
        {
            //_rigidbody = gameObject.GetComponent<Rigidbody>();
            _cameraTransform = Camera.main!.transform;
        }

        private void Update()
        {
            if (!isLocalPlayer) return;
            if (!Input.GetMouseButtonDown(0)) return;
            if (IsDashing) return;
            IsDashing = true;
            _startPosition = transform.position;
            _dashCoroutine = Dash();
            StartCoroutine(_dashCoroutine);
        }

        private async void OnCollisionEnter(Collision collision)
        {
            if (!isLocalPlayer) return;
            _rigidbody.velocity = Vector3.zero;
            if (!collision.gameObject.CompareTag("Player")) return;
            _collisionRoot = collision.transform.root;
            _collisionRoot.GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (!IsDashing) return;
            var collisionDashAbility = _collisionRoot.GetComponent<DashAbility>();
            if (!collisionDashAbility._isAvailableForDash) return;
            collisionDashAbility._isAvailableForDash = false;
            _countOfSuccessDash++;
            Debug.Log(_countOfSuccessDash);
            StopCoroutine(_dashCoroutine);
            IsDashing = false;
            // _rigidbody.velocity = Vector3.zero;
            // _collisionRoot.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _skinnedMeshRenderers = _collisionRoot.GetComponentsInChildren(typeof(SkinnedMeshRenderer), true);
            _collisionRoot.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(ChangeColor());
            // CmdChangeColor(_dashedColor);
            // await DelayTask();
            // CmdChangeColor(_baseColor);
            // collisionDashAbility._isAvailableForDash = true;
        }

        private IEnumerator Dash()
        {
            if (!IsDashing) yield break;
            while (IsDashing)
            {
                _rigidbody.AddForce(transform.GetChild(0).forward * Force, ForceMode.Impulse);
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

        // [ClientRpc]
        // private void ShowWin()
        // {
        //     if (_countOfSuccessDash == 3)
        //     {
        //         Debug.Log("Win");
        //     }
        // }


        private IEnumerator ChangeColor()
        {
            foreach (var component in _skinnedMeshRenderers)
            {
                var skinnedMeshRenderer = (SkinnedMeshRenderer)component;
                skinnedMeshRenderer.materials[0].SetColor(_color, _dashedColor);
            }

            yield return new WaitForSeconds(_disabledTime);
            if (_countOfSuccessDash == 3)
            {
                Debug.Log("Win");
            }

            foreach (var component in _skinnedMeshRenderers)
            {
                var skinnedMeshRenderer = (SkinnedMeshRenderer)component;
                skinnedMeshRenderer.materials[0].SetColor(_color, _baseColor);
            }

            _collisionRoot.GetComponent<DashAbility>()._isAvailableForDash = true;
        }


        [Command(requiresAuthority = false)]
        private void CmdChangeColor(Color color)
        {
            RpcChangeColor(color);
            ChangeColor(color);
        }

        [ClientRpc]
        private void RpcChangeColor(Color color)
        {
            ChangeColor(color);
        }

        private void ChangeColor(Color color)
        {
            foreach (var component in _skinnedMeshRenderers)
            {
                var skinnedMeshRenderer = (SkinnedMeshRenderer)component;
                skinnedMeshRenderer.materials[0].SetColor(_color, color);
            }
        }

        private async Task DelayTask()
        {
            await Task.Delay((int)_disabledTime);
        }
    }
}