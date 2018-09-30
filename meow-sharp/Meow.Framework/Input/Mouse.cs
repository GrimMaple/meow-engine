using Meow.Core;
using Meow.Framework.Graphics;

namespace Meow.Framework.Input
{
    public delegate void MouseEvent(Mouse state);

    /// <summary>
    /// Current mouse state and info. Singleton.
    /// </summary>
    public sealed class Mouse
    {
        private static Mouse sharedInstance;
        private bool[] keys;

        private Point position;

        /// <summary>
        /// Occurs when mouse is moved
        /// </summary>
        public event MouseEvent OnMouseMove;

        /// <summary>
        /// Occurs when Left mouse button is pressed
        /// </summary>
        public event MouseEvent OnLMBDown;

        /// <summary>
        /// Occurs when Left mouse button is released
        /// </summary>
        public event MouseEvent OnLMBUp;

        /// <summary>
        /// Occurs when Right mouse button is pressed
        /// </summary>
        public event MouseEvent OnRMBDown;

        /// <summary>
        /// Occurs when Right mouse button is released
        /// </summary>
        public event MouseEvent OnRMBUp;

        /// <summary>
        /// Current mouse state
        /// </summary>
        public static Mouse CurrentState
        {
            get
            {
                if (sharedInstance == null)
                    sharedInstance = new Mouse();

                return sharedInstance;
            }
        }

        /// <summary>
        /// Mouse position within the game window
        /// </summary>
        public Point Position
        {
            get
            {
                return position;
            }
        }

        /// <summary>
        /// Dedicates if left mouse button is down
        /// </summary>
        public bool LButton
        {
            get
            {
                return keys[0];
            }
        }

        /// <summary>
        /// Dedicates if right mouse button is down
        /// </summary>
        public bool RButton
        {
            get
            {
                return keys[1];
            }
        }

        private Mouse()
        {
            keys = new bool[5];
            position = new Point(0, 0);

            Events.SharedInstance.OnLMBDown += LMBDown;
            Events.SharedInstance.OnLMBUp += LMBUp;
            Events.SharedInstance.OnMouseMove += MouseMove;
            Events.SharedInstance.OnRMBDown += RMBDown;
            Events.SharedInstance.OnRMBUp += RMBUp;
        }

        private void SetCoords(int x, int y)
        {
            position.X = x;
            position.Y = y;
        }
        private void LMBDown(int x, int y)
        {
            keys[0] = true;
            SetCoords(x, y);

            OnLMBDown?.Invoke(this);
        }

        private void LMBUp(int x, int y)
        {
            keys[0] = false;
            SetCoords(x, y);

            OnLMBUp?.Invoke(this);
        }

        private void MouseMove(int x, int y)
        {
            SetCoords(x, y);

            OnMouseMove?.Invoke(this);
        }

        private void RMBDown(int x, int y)
        {
            keys[1] = true;
            SetCoords(x, y);

            OnRMBDown?.Invoke(this);
        }

        private void RMBUp(int x, int y)
        {
            keys[1] = false;
            SetCoords(x, y);

            OnRMBUp?.Invoke(this);
        }
    }
}
