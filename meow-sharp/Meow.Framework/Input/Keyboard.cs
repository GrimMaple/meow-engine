using Meow.Core;

namespace Meow.Framework.Input
{


    public delegate void KeyEvent(Keys key, int character);

    /// <summary>
    /// Current keyboard state and info. Singleton.
    /// </summary>
    public sealed class Keyboard
    {
        private static Keyboard sharedInstance;
        private bool[] keys;

        public event KeyEvent OnKeyDown;
        public event KeyEvent OnKeyUp;

        /// <summary>
        /// Current keyboard state
        /// </summary>
        public static Keyboard CurrentState
        {
            get
            {
                if (sharedInstance == null)
                    sharedInstance = new Keyboard();

                return sharedInstance;
            }
        }

        /// <summary>
        /// Is key pressed
        /// </summary>
        /// <param name="key">Key value</param>
        /// <returns></returns>
        public bool this[char key]
        {
            get
            {
                return keys[(int)key];
            }
        }

        /// <summary>
        /// Is key pressed
        /// </summary>
        /// <param name="code">Key code</param>
        /// <returns></returns>
        public bool this[int code]
        {
            get
            {
                return keys[code];
            }
        }

        /// <summary>
        /// Is key pressed
        /// </summary>
        /// <param name="code">Key value</param>
        /// <returns></returns>
        public bool this[Keys code]
        {
            get
            {
                return keys[(int)code];
            }
        }

        private void KeyDown(int code, int character)
        {
            keys[code] = true;

            OnKeyDown?.Invoke((Keys)code, character);
        }

        private void KeyUp(int code, int character)
        {
            keys[code] = false;

            OnKeyUp?.Invoke((Keys)code, character);
        }

        private Keyboard()
        {
            keys = new bool[256];
            Events.SharedInstance.OnKeyDown += KeyDown;
            Events.SharedInstance.OnKeyUp += KeyUp;
        }
    }
}
