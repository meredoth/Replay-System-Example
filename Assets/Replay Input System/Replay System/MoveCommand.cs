using UnityEngine;

namespace Replay_Input_System.Replay_System
{
public class MoveCommand : ICommand
{
   public CommandType CommandType => CommandType.Move;
   public CommandParameters CommandParameters => _commandParameters;

   public Vector2 Pos
   {
      get => _pos;
      set
      {
         _pos = value;
         _commandParameters.Coordinates = Pos;
      }
   }

   private readonly BallRefactored _ball;
   private CommandParameters _commandParameters;
   private Vector2 _pos;

   public MoveCommand(BallRefactored ball) => _ball = ball;

   public void Execute() => _ball.Move(_commandParameters.Coordinates);
}
}