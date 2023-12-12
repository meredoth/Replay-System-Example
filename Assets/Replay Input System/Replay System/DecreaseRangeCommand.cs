namespace Replay_Input_System.Replay_System
{
public class DecreaseRangeCommand : ICommand
{
   public CommandType CommandType => CommandType.DecreaseRange;
   public CommandParameters CommandParameters { get; }
   
   private readonly BallRefactored _ball;

   public DecreaseRangeCommand(BallRefactored ball) => _ball = ball;
   
   public void Execute() => _ball.Range--;
}
}