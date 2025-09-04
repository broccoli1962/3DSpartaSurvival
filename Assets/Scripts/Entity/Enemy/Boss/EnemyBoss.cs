using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyBoss : MonoBehaviour
{
    public Transform player;

    public MonsterData monsterSO;

    private Rigidbody _rigid;
    private Animator _anim;

    protected FiniteStateMachine _fsm;
    protected Dictionary<EState, IState> _states;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();

        _states = new Dictionary<EState, IState>()
        {
            { EState.Move, new MoveState(this) },
            { EState.Wait, new MoveState(this) },
            { EState.Attack, new AttackState(this) },
            { EState.Skill, new SkillState(this) }
        };

        _fsm = new FiniteStateMachine(_states);
    }

    private void Start()
    {
        _fsm.ChangeTo(EState.Move);
    }

    private void Update()
    {
        _fsm.HandleUpdate();
        _fsm.Update();
    }

    private void FixedUpdate()
    {
        _fsm.PhysicsUpdate();
    }
}