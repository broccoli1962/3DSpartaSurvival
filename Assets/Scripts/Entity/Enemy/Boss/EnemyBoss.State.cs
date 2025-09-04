using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyBoss : MonoBehaviour
{
    #region 이동상태
    public class MoveState : BaseState<EnemyBoss>
    {
        public MoveState(EnemyBoss component) : base(component) { }

        public override void End()
        {
            StopAnimation(AnimParam.Move);
            Debug.Log("움직임 상태 끝");
        }

        public override void HandleUpdate()
        {
            EnemyMovement();
        }

        public override void PhysicsUpdate()
        {
        }

        public override void Start()
        {
            if (Component.player == null)
                Component._fsm.ChangeTo(EState.Wait);

            StartAnimation(AnimParam.Attack);
        }

        public override void Update()
        {
            if (Component.player == null) return;


            if (GetDistance() <= Component.monsterSO.attackRange) //사정거리 내에 진입.
            {
                Component._fsm.ChangeTo(EState.Attack);
            }
        }

        public float GetDistance()
        {
            if (Component.player == null) return 0;
            return Vector3.Distance(Component.player.transform.position, Component.transform.position);
        }

        public void EnemyMovement()
        {
            //방향
            Vector3 targetPos = Component.player.transform.position;
            Vector3 dir = (targetPos - Component.transform.position).normalized;
            Component._rigid.MovePosition(Component.transform.position + dir * Component.monsterSO.moveSpeed * Time.deltaTime);

            //회전
            Quaternion targetRot = Quaternion.LookRotation(dir);
            Component._rigid.MoveRotation(Quaternion.Slerp(Component.transform.rotation, targetRot, Component.monsterSO.moveSpeed * Time.deltaTime));
        }

        public override void StartAnimation(int animationHash)
        {
            base.StartAnimation(animationHash);
        }

        public override void StopAnimation(int animationHash)
        {
            base.StopAnimation(animationHash);
        }
    }
    #endregion

    #region 대기상태
    public class WaitState : BaseState<EnemyBoss>
    {
        public WaitState(EnemyBoss component) : base(component) { }

        public override void End()
        {
        }

        public override void HandleUpdate()
        {
        }

        public override void PhysicsUpdate()
        {
        }

        public override void Start()
        {
        }

        public override void Update()
        {
        }
    }
    #endregion

    #region 공격상태
    public class AttackState : BaseState<EnemyBoss>
    {
        public AttackState(EnemyBoss component) : base(component) { }

        public override void End()
        {
        }

        public override void HandleUpdate()
        {
        }

        public override void PhysicsUpdate()
        {
        }

        public override void Start()
        {
            Debug.Log("공격 상태 진입함");
        }

        public override void Update()
        {
        }
    }
    #endregion

    #region 스킬1
    public class SkillState : BaseState<EnemyBoss>
    {
        public SkillState(EnemyBoss component) : base(component) { }

        public override void End()
        {
        }

        public override void HandleUpdate()
        {
        }

        public override void PhysicsUpdate()
        {
        }

        public override void Start()
        {
        }

        public override void Update()
        {
        }
    }
    #endregion
}
