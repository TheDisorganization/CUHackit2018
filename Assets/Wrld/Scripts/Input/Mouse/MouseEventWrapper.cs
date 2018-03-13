namespace Wrld.MapInput.Mouse
<<<<<<< HEAD
{
    public enum MouseInputAction
    {
        None,
        MousePrimaryDown,
        MousePrimaryUp,
        MouseSecondaryDown,
        MouseSecondaryUp,
        MouseMiddleDown,
        MouseMiddleUp,
        MouseMove,
        MouseWheel
    };

    public enum KeyboardModifiers
    {
        KeyboardModifierNone = 0,
        KeyboardModifierAlt = 1,
        KeyboardModifierControl = 2,
        KeyboardModifierShift = 4
    };

    public struct MouseInputEvent
    {
        public MouseInputAction Action;
        public KeyboardModifiers KeyboardModifiers;

        public float x;
        public float y;
        public float z;

        public MouseInputEvent(MouseInputAction action, float _x, float _y, float _z)
        {
            Action = action;
            KeyboardModifiers = KeyboardModifiers.KeyboardModifierNone;

            x = _x;
            y = _y;
            z = _z;
        }
    };

    public struct KeyboardInputEvent
    {
        public char KeyCode;
        public bool KeyDownEvent;
    };
=======
{
    public enum MouseInputAction
    {
        None,
        MousePrimaryDown,
        MousePrimaryUp,
        MouseSecondaryDown,
        MouseSecondaryUp,
        MouseMiddleDown,
        MouseMiddleUp,
        MouseMove,
        MouseWheel
    };

    public enum KeyboardModifiers
    {
        KeyboardModifierNone = 0,
        KeyboardModifierAlt = 1,
        KeyboardModifierControl = 2,
        KeyboardModifierShift = 4
    };

    public struct MouseInputEvent
    {
        public MouseInputAction Action;
        public KeyboardModifiers KeyboardModifiers;

        public float x;
        public float y;
        public float z;

        public MouseInputEvent(MouseInputAction action, float _x, float _y, float _z)
        {
            Action = action;
            KeyboardModifiers = KeyboardModifiers.KeyboardModifierNone;

            x = _x;
            y = _y;
            z = _z;
        }
    };

    public struct KeyboardInputEvent
    {
        public char KeyCode;
        public bool KeyDownEvent;
    };
>>>>>>> 93976baab53246a27158b03be0d07d7b8897ef5e
}
