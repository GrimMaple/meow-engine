using System;

namespace Meow.Core.Util
{
    /// <summary>
    /// Contains utility for Meow Core
    /// </summary>
    internal class NamespaceDoc
    {

    }

    /// <summary>
    /// Abstract layer between unmanaged resources and managed loaders
    /// </summary>
    public abstract class UnmanagedResource : IDisposable
    {
        private IntPtr resource;

        protected bool Disposed
        {
            get; set;
        }

        internal IntPtr Resource
        {
            get
            {
                return resource;
            }

            set
            {
                if (resource != IntPtr.Zero)
                    throw new InvalidOperationException("Tried to rewrite resource handle");
                resource = value;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected UnmanagedResource()
        {
            resource = IntPtr.Zero;
            Disposed = false;
        }

        protected virtual void Dispose(bool disposing)
        {
            resource = IntPtr.Zero;
            Disposed = true;
        }

        internal UnmanagedResource(IntPtr handle) : this()
        {
            resource = handle;
        } 

        ~UnmanagedResource()
        {
            Dispose(false);
        }
    }
}
