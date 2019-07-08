using System.ComponentModel;
using System.Configuration;

public enum DDR
{
    [Description("Up")]
    Up,
    [Description("Down")]
    Down,
    [Description("Left")]
    Left,
    [Description("Right")]
    Right,
    [Description("Circle")]
    Circle,
    [Description("Cross")]
    Cross,
    [Description("Square")]
    Square,
    [Description("Triangle")]
    Triangle,
    [Description("Start")]
    Start,
    [Description("Select")]
    Select
}