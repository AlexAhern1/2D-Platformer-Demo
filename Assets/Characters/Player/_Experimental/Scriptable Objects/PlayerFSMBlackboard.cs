using System;
using UnityEngine;

namespace Game.Player
{
    [CreateAssetMenu(menuName = "SO/Managers/Player Blackboard")]
    public class PlayerFSMBlackboard : ScriptableObject
    {
        [Header("Configuration Data")]
        [Separator(3)]
        [Header("Movement")]
        [SerializeField] private float _walkingSpeed;
        [SerializeField] private float _runningSpeed;
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _maxFallSpeed;
        [SerializeField] private float _quickFallGravityScale;
        [SerializeField] private float _coyoteTimeInterval;
        [SerializeField] private float _bufferTimeDuration;

        [Header("Static")]
        [SerializeField] private float _maxStatic;
        [SerializeField] private float _startingStatic;
        
        [Header("Air Jumping")]
        [SerializeField] private int _airJumps;

        [Header("Dodging")]
        [SerializeField] private float _dodgeSpeed;
        [SerializeField] private float _dodgeDuration;

        [Header("Wall Jumping")]
        [SerializeField] private float _wallJumpHeight;
        [SerializeField] private float _wallJumpHorizontalDistance;
        [SerializeField] private float _wallJumpDuration;

        [Header("Wall Sliding")]
        [SerializeField][Range(0, 1)] private float _wallSlideSpeedRatio;

        [Header("Attacking")]
        [SerializeField] private float _attackDuration;
        [SerializeField] private float _attackCooldown;
    }
}