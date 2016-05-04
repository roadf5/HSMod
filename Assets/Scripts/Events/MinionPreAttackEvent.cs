﻿public class MinionPreAttackEvent
{
    public MinionCard Minion;
    public Character Target;
    
    public PreStatus Status = PreStatus.Normal;

    public void SwitchTargetTo(Character other)
    {
        if (Status != PreStatus.Cancelled)
        {
            Target = other;
            Status = PreStatus.TargetSwitched;
        }
    }

    public void Cancel()
    {
        Status = PreStatus.Cancelled;
    }
}