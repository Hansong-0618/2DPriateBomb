using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void EnterState(Enemy enemy);
    public abstract void OnState(Enemy enemy);
}
