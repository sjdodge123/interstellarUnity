using System;

public abstract class GameState
{
    public GameState()
    {

    }

    public abstract GameState Activate();
    public abstract void Deactivate();

}
