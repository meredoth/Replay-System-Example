using System.Collections.Generic;
using UnityEngine;

namespace Replay_Input_System.Replay_System
{
public class Invoker
{
   public ICommand Command { get; set; }
   public List<CommandLog> CommandsLog { get; }

   private readonly bool _isRecordingCommands;

   public Invoker(bool isRecordingCommands)
   {
      _isRecordingCommands = isRecordingCommands;
      CommandsLog = new List<CommandLog>();
   }

   public void Execute()
   {
      if (_isRecordingCommands)
      {
         CommandLog commandLog = new()
         {
            CommandType = Command.CommandType,
            CommandParameters = Command.CommandParameters,
            FrameExecuted = Time.frameCount
         };
         
         CommandsLog.Add(commandLog);
      }
      
      Command.Execute();
   }
}
}