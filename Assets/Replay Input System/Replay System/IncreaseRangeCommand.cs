namespace Replay_Input_System.Replay_System
{
public class IncreaseRangeCommand : ICommand
{
   public CommandType CommandType => CommandType.IncreaseRange;
   public CommandParameters CommandParameters { get; }
   
   private readonly BallRefactored _ball;
   
   public IncreaseRangeCommand(BallRefactored ball) => _ball = ball;
   
   public void Execute() => _ball.Range++;
}
}