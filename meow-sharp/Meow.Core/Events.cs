using System.Runtime.InteropServices;

namespace Meow.Core
{

    public delegate void KeyEvent(int code, int character);
    public delegate void RefreshEvent(int timePassed);
    public delegate void MouseEvent(int x, int y);
    public delegate void JoyEvent(int id, int data);
    public delegate void JoyAxisEvent(int id, int data);
    public delegate void WindowSizeEvent(int width, int height);

    /// <summary>
    /// Eventing system implementation. A singleton.
    /// </summary>
    /// <remarks>
    /// This class is supposed to make a bridge between low-level C pointers and C# events
    /// This class has public events that are raised in certain conditions
    /// </remarks>
    public sealed class Events
    {
        private enum EventType
        {
            Draw = 0,
            Update = 1,
            KeyDown = 2,
            KeyUp = 3,
            LmbDown = 4,
            LmbUp = 5,
            RmbDown = 6,
            RmbUp = 7,
            MouseMove = 8
        }

        private Event[] cacheEvents;

        private delegate void Event(int fparam, int sparam);

        [DllImport("meow")]
        private static extern void events_subscribe(int type, Event evt);

        [DllImport("meow")]
        private static extern void events_unsubscribe(int type);

        /// <summary>
        /// Happens when renderer is ready to draw
        /// </summary>
        public event RefreshEvent OnDraw;

        /// <summary>
        /// Happens when update is needed
        /// </summary>
        public event RefreshEvent OnUpdate;

        /// <summary>
        /// Happens when a KeyDown event is raised
        /// </summary>
        public event KeyEvent OnKeyDown;

        /// <summary>
        /// Happens when KeyUp event is raised
        /// </summary>
        public event KeyEvent OnKeyUp;

        /// <summary>
        /// Happens when Left Mouse Button is pressed
        /// </summary>
        public event MouseEvent OnLMBDown;

        /// <summary>
        /// Happens when Light Mouse Button is released
        /// </summary>
        public event MouseEvent OnLMBUp;

        /// <summary>
        /// Happens when Right Mouse Button is pressed
        /// </summary>
        public event MouseEvent OnRMBDown;

        /// <summary>
        /// Happens when Right Mouse Button is released
        /// </summary>
        public event MouseEvent OnRMBUp;

        /// <summary>
        /// Happens when mouse cursor moves
        /// </summary>
        public event MouseEvent OnMouseMove;

        /// <summary>
        /// Happens when a joystick button is released
        /// </summary>
        public event JoyEvent OnJoyBtnUp;

        /// <summary>
        /// Happens when a joustick button is pressed
        /// </summary>
        public event JoyEvent OnJoyBtnDown;

        /// <summary>
        /// Happens when the main windows is resized
        /// </summary>
        public event WindowSizeEvent OnWindowSize;

        /// <summary>
        /// Happens when jostick axis is moved
        /// </summary>
        public event JoyAxisEvent OnJoyAxis;


        private static Events sharedInstance;

        /// <summary>
        /// Shared instance of events manager
        /// </summary>
        public static Events SharedInstance
        {
            get
            {
                if (sharedInstance == null)
                    sharedInstance = new Events();

                return sharedInstance;
            }
        }

        private void Draw(int timePassed, int nothing)
        {
            OnDraw?.Invoke(timePassed);
        }

        private void Update(int timePassed, int nothing)
        {
            OnUpdate?.Invoke(timePassed);
        }

        private void KeyDown(int code, int character)
        {
            OnKeyDown?.Invoke(code, character);
        }

        private void KeyUp(int code, int nothing)
        {
            OnKeyUp?.Invoke(code, nothing);
        }

        private void LMBDown(int x, int y)
        {
            OnLMBDown?.Invoke(x, y);
        }

        private void LMBUp(int x, int y)
        {
            OnLMBUp?.Invoke(x, y);
        }

        private void RMBDown(int x, int y)
        {
            OnRMBDown?.Invoke(x, y);
        }

        private void RMBUp(int x, int y)
        {
            OnRMBUp?.Invoke(x, y);
        }

        private void MouseMove(int x, int y)
        {
            OnMouseMove?.Invoke(x, y);
        }

        private void SubscribeEvents()
        {
            for (int i = 0; i < cacheEvents.Length; i++)
                events_subscribe(i, cacheEvents[i]);
        }

        private void JoyDown(int id, int btn)
        {
            OnJoyBtnDown?.Invoke(id, btn);
        }

        private void JoyUp(int id, int btn)
        {
            OnJoyBtnUp?.Invoke(id, btn);
        }

        private void WindowSize(int w, int h)
        {
            OnWindowSize?.Invoke(w, h);
        }

        private void JoyAxis(int id, int data)
        {
            OnJoyAxis?.Invoke(id, data);
        }

        private Events()
        {
            // All events go in defined order, listed in events.h of meow
            cacheEvents = new Event[13];
            cacheEvents[0] = Draw;
            cacheEvents[1] = Update;
            cacheEvents[2] = KeyDown;
            cacheEvents[3] = KeyUp;
            cacheEvents[4] = LMBDown;
            cacheEvents[5] = LMBUp;
            cacheEvents[6] = RMBDown;
            cacheEvents[7] = RMBUp;
            cacheEvents[8] = MouseMove;
            cacheEvents[9] = JoyUp;
            cacheEvents[10] = JoyDown;
            cacheEvents[11] = WindowSize;
            cacheEvents[12] = JoyAxis;
            SubscribeEvents();
        }
    }
}
