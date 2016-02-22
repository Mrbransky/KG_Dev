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
        string InputID = CreateInputRequestString(button, playerNum);
        return Input.GetButtonDown(InputID);
    }

    public static float GrabVal(XBOX360_AXES axis, int playerNum)
    {
        string InputID = CreateInputRequestString(axis, playerNum);
        return Input.GetAxis(InputID);
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
