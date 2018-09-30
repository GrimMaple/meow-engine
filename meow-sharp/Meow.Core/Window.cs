using System;
using System.Runtime.InteropServices;

namespace Meow.Core
{
    /// <summary>
    /// Contains abstractions between low-end C API and C# Framework
    /// </summary>
    internal class NamespaceDoc
    {

    }

    /// <summary>
    /// Abstraction between low-level window and high-level MeowGame
    /// </summary>
    public abstract class Window : IDisposable
    {
		[DllImport("meow")]
        private static extern uint window_create();

		[DllImport("meow")]
        private static extern uint window_cleanup();

		[DllImport("meow")]
        private static extern void window_resize(uint width, uint height);

		[DllImport("meow")]
        private static extern void window_run();

		[DllImport("meow")]
        private static extern uint window_fps();

		[DllImport("meow")]
        private static extern void window_set_text([MarshalAs(UnmanagedType.LPStr)] string text);

		[DllImport("meow")]
        private static extern void window_get_size(ref uint w, ref uint h);

		[DllImport("meow")]
        private static extern void window_break_cycle();

		[DllImport("meow")]
        private static extern uint window_is_fullscreen();

		[DllImport("meow")]
        private static extern void window_set_fullscreen(int fs);

		[DllImport("meow")]
        private static extern uint window_get_refresh_interval();

		[DllImport("meow")]
        private static extern uint window_get_redraw_interval();

		[DllImport("meow")]
        private static extern void window_set_refresh_interval(uint interval);

		[DllImport("meow")]
        private static extern void window_set_redraw_interval(uint interval);

		[DllImport("meow")]
        private static extern void vsync_set(int vsync);

		[DllImport("meow")]
        private static extern int vsync_get();

		[DllImport("meow")]
        private static extern void renderer_set_clear_color(float r, float g, float b, float a);

		[DllImport("meow")]
        private static extern void renderer_set_should_clear(int i);

		[DllImport("meow")]
        private static extern int renderer_get_should_clear();

        string title = "";

        int width, height;

        /// <summary>
        /// Indicates if colorbuffer should be cleared before each frame
        /// </summary>
        public bool ShouldClear
        {
            get
            {
                return renderer_get_should_clear() != 0;
            }
            set
            {
                renderer_set_should_clear(value ? 1 : 0);
            }
        }

        /// <summary>
        /// Get or set window's title
        /// </summary>
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                window_set_text(title);
            }
        }

        /// <summary>
        /// Get or set global update rate [in ms]. Will determines frequency of Update() calls
        /// </summary>
        public uint RefreshInterval
        {
            get
            {
                return window_get_refresh_interval();
            }
            set
            {
                window_set_refresh_interval(value);
            }
        }

        /// <summary>
        /// Get or set global redraw rate [in ms]. Basically FPS lock
        /// </summary>
        public uint RedrawInterval
        {
            get
            {
                return window_get_redraw_interval();
            }
            set
            {
                window_set_redraw_interval(value);
            }
        }

        /// <summary>
        /// Get or set current window's width
        /// </summary>
        public uint Width
        {
            get
            {
                return (uint)width;
            }
            set
            {
                window_resize(value, Height);
            }
        }

        /// <summary>
        /// Get or set current window's height
        /// </summary>
        public uint Height
        {
            get
            {
                return (uint)height;
            }
            set
            {
                window_resize(Width, value);
            }
        }

        /// <summary>
        /// Get window FPS
        /// </summary>
        public uint FPS
        {
            get
            {
                return window_fps();
            }
        }

        /// <summary>
        /// Check if window is in fullscreen mode
        /// </summary>
        public bool Fullscreen
        {
            get
            {
                return window_is_fullscreen() != 0;
            }
            set
            {
                window_set_fullscreen(value ? 1 : 0);
            }
        }

        /// <summary>
        /// Is vsync enabled
        /// </summary>
        public bool VSyncEnabled
        {
            get
            {
                return vsync_get() == 1;
            }
            set
            {
                vsync_set(value ? 1 : 0);
            }
        }

        private void OnResize(int width, int height)
        {
            this.width = width;
            this.height = height;
            OnWindowResize(width, height);
        }

        protected virtual void OnWindowResize(int width, int height)
        {
            return;
        }

        /// <summary>
        /// Set background clear color
        /// </summary>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        /// <param name="a">Alpha</param>
        protected void SetClearColor(float r, float g, float b, float a)
        {
            renderer_set_clear_color(r, g, b, a);
        }

        /// <summary>
        /// Creates a new window with given width and height
        /// </summary>
        /// <param name="w">Window width</param>
        /// <param name="h">Window height</param>
        /// <exception cref="System.Exception">
        /// Thrown if window didn't do a good job creating itself
        /// </exception>
        public Window(uint w, uint h)
        {
            Events.SharedInstance.OnWindowSize += OnResize;
            Resize(w, h);
            uint result = window_create();
            if (result != 0)
                throw new Exception("FOOLSCREEN");
            window_set_text("");
            Events.SharedInstance.OnDraw += (int passed) =>
            {
                Draw(passed);
                Present();
            };
            Events.SharedInstance.OnUpdate += Update;
        }

        /// <summary>
        /// Called when window is ready to be drawn
        /// </summary>
        /// <param name="timePassed">Amount of time (in ms) that has passed since last Draw() call</param>
        protected abstract void Draw(int timePassed);

        /// <summary>
        /// Called when window is ready to be updated
        /// </summary>
        /// <param name="timePassed">Amount of time (in ms) that has passed since last <c>Update()</c> call</param>
        protected abstract void Update(int timePassed);

        /// <summary>
        /// Called when drawing is done and scene has to be presented
        /// </summary>
        protected abstract void Present();

        /// <summary>
        /// Starts window rendering and updating cycle
        /// </summary>
        public void Run()
        {
            window_run();
        }

        /// <summary>
        /// Implementation of IDisposable
        /// </summary>
        public virtual void Dispose()
        {
            window_cleanup();
        }

        /// <summary>
        /// Resize current window
        /// </summary>
        /// <param name="width">New window width</param>
        /// <param name="height">New window height</param>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when trying to resize window while it is in fullscrren
        /// </exception>
        public void Resize(uint width, uint height)
        {
            if (Fullscreen)
                throw new System.InvalidOperationException("Can't resize window in fullscreen");
            window_resize(width, height);
        }

        /// <summary>
        /// Switch to fullscreen mode or turn it off
        /// </summary>
        public void ToggleFullscreen()
        {
            if (Fullscreen)
                window_set_fullscreen(0);
            else
                window_set_fullscreen(1);
        }

        /// <summary>
        /// Break the drawcycle, thus forcing the window to exit
        /// </summary>
        public void Exit()
        {
            window_break_cycle();
        }
    }
}
