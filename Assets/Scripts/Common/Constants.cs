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
    Skill1,
    Death
}

public enum EStatType
{
    Health,
    MoveSpeed,
    Power,
    AttackRange,
    CoolTime,
    AttackSpeed,
    AttackCount,
    ProjectileSpeed,
    ProjectileCount,
    ProjectileAngle
}

public enum GameState {
    InitialWait,
    WaveInProgress,
    WaveComplete,
    BossFight,
    GameWon 
}

public enum ESceneType
{
    Menu,
    Battle
}

public static class AnimParam
{
    public static readonly int Idle = Animator.StringToHash("Idle");
    public static readonly int Move = Animator.StringToHash("Move");
    public static readonly int Attack = Animator.StringToHash("Attack");
    public static readonly int AttackSpeedMul = Animator.StringToHash("AttackSpeedMul");
    public static readonly int Skill1 = Animator.StringToHash("Skill1");
    public static readonly int Death = Animator.StringToHash("Death");
}

public static class Path
{
    public const string Prefab = "Prefab/";
    public const string UI = Prefab + "UI/";

    public const string Character = Prefab + "Character/Player/";
    public const string Enemy = Prefab + "Character/Enemy/";

    public const string Map = Prefab + "Map/";
    public const string MapElement = Map + "Environment/";
}

public static class Prefab
{
    //UI
    public const string Canvas = "Canvas";
    public const string EventSystem = "EventSystem";

    //Character
    public const string Character = "Character";
    public const string Player = "Player";
    public const string EnemyBoss = "EnemyBoss";

    //Map
    public const string Magma = "Magma";
    public const string Map = "Map";
}