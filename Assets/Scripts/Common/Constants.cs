using UnityEngine;

public enum EWeaponType
{
    None,
    Melee,
    Bow,
    Staff,
    Wand
}

public enum EAttackShape
{
    None,
    Circle,
    Square,
    Seta
}

public enum EState
{
    None,
    Attack,
    Wait,
    Move,
    Skill
}

public static class AnimParam
{
    public static readonly int Idle = Animator.StringToHash("Idle");
    public static readonly int Move = Animator.StringToHash("Move");
    public static readonly int Attack = Animator.StringToHash("Attack");
    public static readonly int AttackSpeedMul = Animator.StringToHash("AttackSpeedMul");
}