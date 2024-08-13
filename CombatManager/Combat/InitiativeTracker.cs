namespace CombatManager.Combat;

public class InitiativeTracker
{
    public int Current { get; private set; }
    public bool IsActive { get; private set; }

    public void Initialize()
    {
        Current = 0;
        IsActive = true;
    }

    public void Stop()
    {
        IsActive = false;
    }

    public int PeekNextValue(int maximum)
    {
        if (!IsActive || Current >= maximum - 1)
        {
            return 0;
        }

        return Current + 1;
    }

    public int PeekPrevValue(int maximum)
    {
        if (!IsActive)
        {
            return 0;
        }

        if (Current == 0)
        {
            return maximum - 1;
        }

        return Current - 1;
    }
    
    public void Increment(int maximum)
    {
        Current = PeekNextValue(maximum);
    }

    public void Decrement(int maximum)
    {
        Current = PeekPrevValue(maximum);
    }
}