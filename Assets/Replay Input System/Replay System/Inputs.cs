using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Replay_Input_System.Replay_System
{
public class Inputs : MonoBehaviour
{
    private enum InputMode
    {
        Normal,
        Record,
        Replay
    }
    
    [SerializeField] private BallRefactored ball;
    [SerializeField] private InputMode inputMode;
    
    private Invoker _invoker;
    private GamePlayCommandsIO _gamePlayCommandsIO;
    private Camera _camera;
    private IncreaseRangeCommand _increaseRangeCommand;
    private DecreaseRangeCommand _decreaseRangeCommand;
    private ChangeColorCommand _changeColorCommand;
    private MoveCommand _moveCommand;

    private void Awake()
    {
        _camera = Camera.main;
        _gamePlayCommandsIO = new GamePlayCommandsIO("example");

        if(ball == null) 
            Debug.LogError("Ball is null!");

        _increaseRangeCommand = new IncreaseRangeCommand(ball);
        _decreaseRangeCommand = new DecreaseRangeCommand(ball);
        _changeColorCommand = new ChangeColorCommand(ball);
        _moveCommand = new MoveCommand(ball);

        _invoker = inputMode == InputMode.Record ? new Invoker(true) : new Invoker(false);
    }

    private void Start()
    {
        if (inputMode == InputMode.Replay)
            StartCoroutine(ReplayInput());
    }

    private void Update()
    {
        if(inputMode is InputMode.Normal or InputMode.Record)
            PlayerInput();
    }

    private void PlayerInput()
    {
        if (Mouse.current.scroll.ReadValue().y > 0f)
        {
            _invoker.Command = _increaseRangeCommand;
            _invoker.Execute();
        }

        if (Mouse.current.scroll.ReadValue().y < 0f)
        {
            _invoker.Command = _decreaseRangeCommand;
            _invoker.Execute();
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            _invoker.Command = _changeColorCommand;
            _invoker.Execute();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _moveCommand.Pos = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            _invoker.Command = _moveCommand;
            _invoker.Execute();
        }
    }

    private IEnumerator ReplayInput()
    {
        List<CommandLog> loadedCommands = _gamePlayCommandsIO.Load();
        int commandNumber = 0;

        while (commandNumber < loadedCommands.Count)
        {
            if (loadedCommands[commandNumber].FrameExecuted == Time.frameCount)
            {
                SetCommand(loadedCommands, commandNumber);

                _invoker.Execute();
                commandNumber++;
            }
            yield return null;
        }

        Debug.Log("Resuming normal input.");
        inputMode = InputMode.Normal;
    }

    private void SetCommand(List<CommandLog> loadedCommands, int commandNumber)
    {
        switch (loadedCommands[commandNumber].CommandType)
        {
            case CommandType.IncreaseRange:
                _invoker.Command = _increaseRangeCommand;
                break;
            case CommandType.DecreaseRange:
                _invoker.Command = _decreaseRangeCommand;
                break;
            case CommandType.ChangeColor:
                _invoker.Command = _changeColorCommand;
                break;
            case CommandType.Move:
                _moveCommand.Pos = loadedCommands[commandNumber].CommandParameters.Coordinates;
                _invoker.Command = _moveCommand;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnApplicationQuit()
    {
        if(inputMode == InputMode.Record)
            _gamePlayCommandsIO.Save(_invoker.CommandsLog);
    }
}
}
