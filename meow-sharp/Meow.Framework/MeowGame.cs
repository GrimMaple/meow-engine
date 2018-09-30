using Meow.Core;
using Meow.Framework.Graphics;

namespace Meow.Framework
{
    /// <summary>
    /// Contains basic utility for rendering
    /// </summary>
    internal class NamespaceDoc
    {

    }

    /// <summary>
    /// Window resize event delegate
    /// </summary>
    /// <param name="width">New window width</param>
    /// <param name="height">New window height</param>
    public delegate void WindowResized(int width, int height);

    /// <summary>
    /// Base Meow Window class
    /// </summary>
    public abstract class MeowGame : Window
    {
		private RenderTarget renderTarget;
        private Color clearColor;

        /// <summary>
        /// Primitives assembly factory
        /// </summary>
        protected PrimitiveBatch primitiveBatch;

        /// <summary>
        /// Sprites assembly factory
        /// </summary>
        protected SpriteBatch spriteBatch;

        /// <summary>
        /// Back buffer rendering target
        /// </summary>
        protected Graphics.Renderer renderer;

        /// <summary>
        /// Happens when the window is being resized
        /// </summary>
        public event WindowResized OnWindowSize;

        /// <summary>
        /// Back buffer rendering target
        /// </summary>
		protected RenderTarget RenderTarget
		{
			get
			{
				return renderTarget;
			}
		}

        /// <summary>
        /// Window's clear color
        /// </summary>
        protected Color ClearColor
        {
            get
            {
                return clearColor;
            }
            set
            {
                clearColor = value;
                SetClearColor(clearColor.R, clearColor.G, clearColor.B, clearColor.A);
            }
        }

        /// <summary>
        /// Will be called whenever the window is resized
        /// </summary>
        /// <param name="width">New window width</param>
        /// <param name="height">New window height</param>
        protected override void OnWindowResize(int width, int height)
        {
            OnWindowSize?.Invoke(width, height);
        }

        /// <summary>
        /// Makes sure to render all the remaining triangle is the factory
        /// </summary>
        protected sealed override void Present()
        {
			renderTarget.Flush();
        }

        /// <summary>
        /// Create a new window
        /// </summary>
        /// <param name="w">Window width</param>
        /// <param name="h">Window height</param>
        public MeowGame(uint w, uint h) : base(w, h)
        {
            ClearColor = new Graphics.Color(0, 0, 0);
            renderer = new Graphics.Renderer();
			renderTarget = new RenderTarget(renderer);
            primitiveBatch = new PrimitiveBatch(renderTarget);
            spriteBatch = new SpriteBatch(renderTarget);
        }

        /// <summary>
        /// Releases all unmanaged resources connected to this window
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
