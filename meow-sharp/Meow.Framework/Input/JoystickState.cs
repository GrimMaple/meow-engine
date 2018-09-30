namespace Meow.Framework.Input
{
    /// <summary>
    /// Represents current joystick state
    /// </summary>
    public class JoystickState
    {
        private bool[] buttons;
        private float[] axis;

        /// <summary>
        /// Joystick global id
        /// </summary>
        public int Id
        {
            get; private set;
        }

        internal JoystickState(int id)
        {
            Id = id;
            buttons = new bool[14];
            for (int i = 0; i < 14; i++)
                buttons[i] = false;
            axis = new float[6];
        }

        internal void SetDown(int i)
        {
            buttons[i] = true;
        }

        internal void SetUp(int i)
        {
            buttons[i] = false;
        }

        internal void SetAxis(JoystickAxis axis, float value)
        {
            int index = (int)((long)axis & 0xFF000000 >> 24);
            this.axis[index] = value;
        }
    }
}
