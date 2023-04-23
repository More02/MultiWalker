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
            ResetToFalseAllBools();
            _animator.SetBool(_toWalk, true);
            if (!DashAbility.Instance.IsDashing) return;
            ResetToFalseAllBools();
            _animator.SetBool(_toDash, true);
        }
        else if (DashAbility.Instance.IsDashing)
        {
            ResetToFalseAllBools();
            _animator.SetBool(_toDash, true);
        }
        else
        {
            ResetToFalseAllBools();
            _animator.SetBool(_toIdle, true);
        }
    }

    private void ResetToFalseAllBools()
    {
        _animator.SetBool(_toIdle, false);
        _animator.SetBool(_toWalk, false);
        _animator.SetBool(_toDash, false);
    }
}