namespace ComponentFramework
{
    using System;

    public struct ComponentConnection
    {
        #region Fields

        public Delegate MethodDelegate;

        public object[] ParameterBuffer;

        public string[] ParameterNames;

        public Type[] ParameterTypes;

        #endregion
    }
}