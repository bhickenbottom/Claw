namespace ComponentFramework
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class Component : ComponentObject
    {
        #region Constructors

        public Component()
        {
            this.Connections = new Dictionary<string, ComponentConnection>();
            this.IsEnabled = true;
        }

        #endregion

        #region Fields

        internal Dictionary<string, ComponentConnection> Connections;

        internal TypeInfo TypeCache;

        #endregion

        #region Properties

        public bool IsEnabled { get; set; }

        public ComponentObject Parent { get; internal set; }

        #endregion

        #region Virtual Methods

        public virtual void DynamicInvoke(string methodName, Type[] types, string[] names, object[] values, bool handled)
        {
            // Empty
        }

        #endregion
    }
}