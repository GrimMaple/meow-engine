using Meow.Core;
using System;

namespace Meow.Framework.Input
{
    public delegate void JoystickButtonDown(object sender, JoystickState state, JoystickButtons button);
    public delegate void JoystickAxisMove(object sender, JoystickState state, JoystickAxis axis, float value);

    /// <summary>
    /// Provides joystick support
    /// </summary>
    public class Joystick
    {
        private static Joystick shared;
        private JoystickState[] joysticks;

        /// <summary>
        /// Happens whenever a joystick button is pressed
        /// </summary>
        public event JoystickButtonDown OnJoystickDown;

        /// <summary>
        /// Happens whenever a joystick button is released
        /// </summary>
        public event JoystickButtonDown OnJoystickUp;

        /// <summary>
        /// Happens whenever joystick axle is moved
        /// </summary>
        public event JoystickAxisMove OnAxisMoved;

        /// <summary>
        /// Shared controller instance
        /// </summary>
        public static Joystick SharedInstance
        {
            get
            {
                if (shared == null)
                    shared = new Joystick();

                return shared;
            }
        }

        private Joystick()
        {
            joysticks = new JoystickState[4];
            for (int i = 0; i < 4; i++)
                joysticks[i] = new JoystickState(i);

            Events.SharedInstance.OnJoyBtnDown += ButtonDown;
            Events.SharedInstance.OnJoyBtnUp += ButtonUp;
            Events.SharedInstance.OnJoyAxis += AxisMoved;
        }

        private void AxisMoved(int id, int data)
        {
            float value;
            JoystickAxis ax = (JoystickAxis)(data & 0xFF000000);
            data = data & 0x0000FFFF;
            if (ax == JoystickAxis.LeftTrigger || ax == JoystickAxis.RightTrigger)
                value = (float)data / 255;
            else
                value = Math.Max(-1, (float)BitConverter.ToInt16(BitConverter.GetBytes(data), 0) / (short.MaxValue));

            joysticks[id].SetAxis(ax, value);
            OnAxisMoved?.Invoke(this, joysticks[id], ax, value);
        }

        private void ButtonDown(int id, int button)
        {
            joysticks[id].SetDown(button);
            OnJoystickDown?.Invoke(this, joysticks[id], (JoystickButtons)button);
        }

        private void ButtonUp(int id, int button)
        {
            joysticks[id].SetUp(button);
            OnJoystickUp?.Invoke(this, joysticks[id], (JoystickButtons)button);
        }
    }
}
