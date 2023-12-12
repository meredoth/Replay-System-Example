namespace Replay_Input_System.Replay_System
{
public interface ICommand
{
   CommandType CommandType { get; }
   CommandParameters CommandParameters { get; }
   public void Execute();
}
}