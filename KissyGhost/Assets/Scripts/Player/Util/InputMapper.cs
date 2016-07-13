using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public enum XBOX360_BUTTONS { A, B, X, Y };
public enum XBOX360_AXES
{
    LeftStick_Vert,
    LeftStick_Horiz,
    RightStick_Horiz,
    RightStick_Vert,
    Trig_Left,
    Trig_Right
};

public static class InputMapper 
{
    public static bool GrabVal(XBOX360_BUTTONS button, int playerNum)
    {
        //string InputID = CreateInputRequestString(button, playerNum);
        //return Input.GetButtonDown(InputID);

        GamePadState gamepadState = GamePad.GetState((PlayerIndex)(playerNum - 1));
        
        switch ((int)button)
        {
            case (int)XBOX360_BUTTONS.A:
                return (gamepadState.Buttons.A == ButtonState.Pressed);
            case (int)XBOX360_BUTTONS.B:
                return (gamepadState.Buttons.B == ButtonState.Pressed);
            case (int)XBOX360_BUTTONS.X:
                return (gamepadState.Buttons.X == ButtonState.Pressed);
            case (int)XBOX360_BUTTONS.Y:
                return (gamepadState.Buttons.Y == ButtonState.Pressed);
        }

        return false;
    }

    public static float GrabVal(XBOX360_AXES axis, int playerNum)
    {
        //string InputID = CreateInputRequestString(axis, playerNum);
        //return Input.GetAxis(InputID);
        
        GamePadState gamepadState = GamePad.GetState((PlayerIndex)(playerNum - 1));

        switch ((int)axis)
        {
            case (int)XBOX360_AXES.LeftStick_Horiz:
                return gamepadState.ThumbSticks.Left.X;
            case (int)XBOX360_AXES.LeftStick_Vert:
                return gamepadState.ThumbSticks.Left.Y;
            case (int)XBOX360_AXES.RightStick_Horiz:
                return gamepadState.ThumbSticks.Right.X;
            case (int)XBOX360_AXES.RightStick_Vert:
                return gamepadState.ThumbSticks.Right.Y;
        }

        return gamepadState.ThumbSticks.Left.X;        
    }

    private static string CreateInputRequestString(XBOX360_BUTTONS button, int playerNum)
    {
        string buttonString = "";

        switch (button)
        {
            case XBOX360_BUTTONS.A: buttonString = "pad_A";
                break;
            case XBOX360_BUTTONS.B: buttonString = "pad_B";
                break;
            default:
                break;
        }

        return "P" + playerNum + buttonString;
    }

    private static string CreateInputRequestString(XBOX360_AXES axis, int playerNum)
    {
        string axisString = "";

        switch (axis)
        {
            case XBOX360_AXES.LeftStick_Horiz: axisString = "pad_Horiz";
                break;
            case XBOX360_AXES.LeftStick_Vert: axisString = "pad_Vert";
                break;
            default:
                break;
        }

        return "P" + playerNum + axisString;
    }

    public static IEnumerator Vibration(int playerNum, float timeAmt, float leftMotor, float rightMotor)
    {
        GamePad.SetVibration((PlayerIndex)playerNum - 1, leftMotor, rightMotor);

        yield return new WaitForSeconds(timeAmt);

        GamePad.SetVibration((PlayerIndex)playerNum - 1, 0, 0);
    }
}
