using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyBaseState
{
    public override void EnterState(Enemy enemy)
    {
        enemy.animState = 2;
        enemy.targetPoint = enemy.attackObj[0];
    }

    public override void OnState(Enemy enemy)
    {
        if (enemy.hasBomb)
            return;
        //怪物的巡逻范围内无敌人时，自动切换回巡逻状态
        if (enemy.attackObj.Count == 0)
        {
            enemy.TransitionToState(enemy.patrolState);
        }
        //怪物的巡逻范围内有多个怪物时，追踪离得最近的那个
        if (enemy.attackObj.Count > 1)
        {
            for (int i = 0; i < enemy.attackObj.Count; i++)
            {
                if(Mathf.Abs(enemy.transform.position.x-enemy.attackObj[i].position.x)<
                    Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x))
                {
                    enemy.targetPoint = enemy.attackObj[i];
                }
            }
        }
        //
        if(enemy.attackObj.Count == 1)
        {
            enemy.targetPoint = enemy.attackObj[0];
        }

        //面对目标的不同选择不同的攻击方式
        if (enemy.targetPoint.CompareTag("Player"))
        {
            enemy.AttackAction();
        }
        if (enemy.targetPoint.CompareTag("Bomb"))
        {
            enemy.SkillAction();
        }

        enemy.MoveToTarget();
    }
}
