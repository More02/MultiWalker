using Mirror;
using Movement;
using UnityEngine;

public class PlayerAnimation : NetworkBehaviour
{
    [SerializeField] private Animator _animator;

    private static readonly int _toDash = Animator.StringToHash("ToDash");
    private static readonly int _toWalk = Animator.StringToHash("ToWalk");
    private static readonly int _toIdle = Animator.StringToHash("ToIdle");

    private void Update()
    {
        if (!isLocalPlayer) return;
        if (_animator is null) return;
        if ((Input.GetAxis("Horizontal") != 0) || (Input.GetAxis("Vertical") != 0))
        {
            _animator.SetBool(_toWalk, true);
            _animator.SetBool(_toDash, false);
            _animator.SetBool(_toIdle, false);
            if (Abilities.Instance.IsDashing)
            {
                _animator.SetBool(_toDash, true);
                _animator.SetBool(_toWalk, false);
            }
        }
        else if (Abilities.Instance.IsDashing)
        {
            _animator.SetBool(_toDash, true);
            _animator.SetBool(_toWalk, false);
            _animator.SetBool(_toIdle, false);
        }
        else
        {
            _animator.SetBool(_toIdle, true);
            _animator.SetBool(_toWalk, false);
            _animator.SetBool(_toDash, false);
        }
    }
}