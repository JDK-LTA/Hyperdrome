using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimBall : EnemyAnimBase
{
    private bool isExploding = false;

    public event EnemyAnimEvents Explode;
    public event EnemyAnimEvents StartRunningEvent;

    protected override void Start()
    {
        base.Start();
        transform.parent.GetComponent<EnemyBase>().ReadyToRun += TriggerRunning;
        transform.parent.GetComponent<EnemyBall>().ReadyToExplode += TriggerExploding;
    }

    private void TriggerExploding()
    {
        isExploding = true;
    }

    protected virtual void StartRunning()
    {
        StartRunningEvent?.Invoke();
    }
    protected void TriggerRunning()
    {
        _animator.SetTrigger("PrepareToRoll");
        isPreparingToRun = true;
    }
}
