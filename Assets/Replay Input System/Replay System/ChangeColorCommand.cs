namespace Replay_Input_System.Replay_System
{
public class ChangeColorCommand : ICommand
{
   public CommandType CommandType => CommandType.ChangeColor;
   public CommandParameters CommandParameters { get; }
   
   private readonly BallRefactored _ball;

   public ChangeColorCommand(BallRefactored ball) => _ball = ball;
   
   public void Execute() => _ball.ChangeColor();
}
}