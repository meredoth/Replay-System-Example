using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Replay_Input_System.Replay_System
{

[Serializable]
public enum CommandType
{
   IncreaseRange,
   DecreaseRange,
   ChangeColor,
   Move
}

[Serializable]
public struct CommandParameters
{
   public Vector2 Coordinates;
}

[Serializable]
public struct CommandLog
{
   public int FrameExecuted;
   public CommandType CommandType;
   public CommandParameters CommandParameters;
}

[Serializable]
public class GameInputData
{
   public List<CommandLog> CommandLog;
}

public class GamePlayCommandsIO
{
   private readonly string _jsonPath;
   
   public GamePlayCommandsIO(string fileName)
   {
      if (fileName.Length == 0)
         throw new InvalidDataException("Not valid filename.");
      
      _jsonPath = "Assets/Replay Input System/Replay System/" + fileName + ".json";
   }
   
   public void Save(List<CommandLog> commandsLog)
   {
      var gameInputData = new GameInputData
      {
         CommandLog = commandsLog
      };

      var actionInfo = JsonUtility.ToJson(gameInputData); 
      File.WriteAllText(_jsonPath,actionInfo);
   }

   public List<CommandLog> Load()
   {
      var file = File.ReadAllText(_jsonPath);
      return JsonUtility.FromJson<GameInputData>(file).CommandLog;
      
   }
}
}