namespace ComponentFramework
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class ComponentObject
    {
        #region Constructors

        public ComponentObject()
        {
            this.components = new List<Component>();
        }

        #endregion

        #region Properties

        private List<Component> components { get; set; }

        #endregion

        #region Methods

        public T AddComponent<T>() where T : Component
        {
            T component = Activator.CreateInstance<T>();
            this.AddComponent(component);
            return component;
        }

        public T AddComponent<T>(T component) where T : Component
        {
            if (component.Parent != null)
            {
                throw new InvalidOperationException("Components cannot be assigned to multiple component objects.");
            }
            component.Parent = this;
            this.components.Add(component);
            return component;
        }

        public List<T> GetComponents<T>() where T : Component
        {
            List<T> results = new List<T>();
            List<Component> components = this.components;
            int componentCount = components.Count;
            for (int i = 0; i < componentCount; i++)
            {
                Component component = components[i];
                if (component is T typedComponent)
                {
                    results.Add(typedComponent);
                }
            }
            return results;
        }

        public void InvokeComponents(object parameter1 = null, object parameter2 = null, object parameter3 = null, object parameter4 = null, object parameter5 = null, object parameter6 = null, object parameter7 = null, object parameter8 = null, [CallerMemberName] string methodName = "Default", int depth = 1, object source = null)
        {
            // Enumerator Components
            for (int i = 0; i < this.components.Count; i++)
            {
                // Component
                Component component = this.components[i];

                // Lock Component
                lock (component)
                {
                    // Is Enabled?
                    if (!component.IsEnabled)
                    {
                        continue;
                    }

                    // Populate Type Cache
                    if (component.TypeCache == null)
                    {
                        Type type = component.GetType();
                        component.TypeCache = type.GetTypeInfo();
                    }

                    // Populate Connection
                    if (!component.Connections.ContainsKey(methodName))
                    {
                        // Stack
                        StackTrace stackTrace = new StackTrace();
                        StackFrame parentStackFrame = stackTrace.GetFrame(depth);

                        // Method
                        MethodBase parentMethodBase = parentStackFrame.GetMethod();

                        // Parameters
                        ParameterInfo[] parentParameterInfos = parentMethodBase.GetParameters();
                        Type[] types = new Type[parentParameterInfos.Length + 1];
                        string[] names = new string[parentParameterInfos.Length + 1];
                        types[0] = this.GetType();
                        names[0] = "source";
                        for (int j = 0; j < parentParameterInfos.Length; j++)
                        {
                            ParameterInfo parentParameterInfo = parentParameterInfos[j];
                            Type parameterType = parentParameterInfo.ParameterType;
                            if (parameterType.IsValueType)
                            {
                                Type boxedType = typeof(Boxed<>);
                                parameterType = boxedType.MakeGenericType(parameterType);
                            }
                            types[j + 1] = parameterType;
                            names[j + 1] = parentParameterInfo.Name;
                        }

                        // Return Type
                        MethodInfo parentMethodInfo = parentMethodBase as MethodInfo;
                        if (parentMethodInfo != null && parentMethodInfo.ReturnType != typeof(void))
                        {
                            Type boxedType = typeof(Boxed<>);
                            Type returnType = boxedType.MakeGenericType(parentMethodInfo.ReturnType);
                            Type[] typesPlusReturn = new Type[types.Length + 1];
                            Array.Copy(types, typesPlusReturn, types.Length);
                            typesPlusReturn[types.Length] = returnType;
                            types = typesPlusReturn;
                        }

                        // Connection
                        ComponentConnection connection = new ComponentConnection();
                        connection.ParameterTypes = types;
                        connection.ParameterNames = names;
                        connection.ParameterBuffer = new object[types.Length];

                        // Component Method
                        MethodInfo methodInfo = component.TypeCache.GetMethod(methodName, types);
                        if (methodInfo != null)
                        {
                            Type actionType = typeof(Action<>);
                            if (types.Length == 1)
                            {
                                actionType = typeof(Action<>);
                            }
                            else if (types.Length == 2)
                            {
                                actionType = typeof(Action<,>);
                            }
                            else if (types.Length == 3)
                            {
                                actionType = typeof(Action<,,>);
                            }
                            else if (types.Length == 4)
                            {
                                actionType = typeof(Action<,,,>);
                            }
                            else if (types.Length == 5)
                            {
                                actionType = typeof(Action<,,,,>);
                            }
                            else if (types.Length == 6)
                            {
                                actionType = typeof(Action<,,,,,>);
                            }
                            else if (types.Length == 7)
                            {
                                actionType = typeof(Action<,,,,,,>);
                            }
                            else if (types.Length == 8)
                            {
                                actionType = typeof(Action<,,,,,,,>);
                            }
                            else if (types.Length == 8)
                            {
                                actionType = typeof(Action<,,,,,,,,>);
                            }
                            Type specificActionType = actionType.MakeGenericType(types);
                            connection.MethodDelegate = methodInfo.CreateDelegate(specificActionType, component);
                        }

                        // Add
                        component.Connections.Add(methodName, connection);
                    }

                    // Get Connection
                    ComponentConnection connectionToExecute = component.Connections[methodName];

                    // Parameter Buffer
                    connectionToExecute.ParameterBuffer[0] = source ?? this;
                    if (connectionToExecute.ParameterBuffer.Length > 1)
                    {
                        connectionToExecute.ParameterBuffer[1] = parameter1;
                    }
                    if (connectionToExecute.ParameterBuffer.Length > 2)
                    {
                        connectionToExecute.ParameterBuffer[2] = parameter2;
                    }
                    if (connectionToExecute.ParameterBuffer.Length > 3)
                    {
                        connectionToExecute.ParameterBuffer[3] = parameter3;
                    }
                    if (connectionToExecute.ParameterBuffer.Length > 4)
                    {
                        connectionToExecute.ParameterBuffer[4] = parameter4;
                    }
                    if (connectionToExecute.ParameterBuffer.Length > 5)
                    {
                        connectionToExecute.ParameterBuffer[5] = parameter5;
                    }
                    if (connectionToExecute.ParameterBuffer.Length > 6)
                    {
                        connectionToExecute.ParameterBuffer[6] = parameter6;
                    }
                    if (connectionToExecute.ParameterBuffer.Length > 7)
                    {
                        connectionToExecute.ParameterBuffer[7] = parameter7;
                    }
                    if (connectionToExecute.ParameterBuffer.Length > 8)
                    {
                        connectionToExecute.ParameterBuffer[8] = parameter8;
                    }
                    if (connectionToExecute.ParameterBuffer.Length > 8)
                    {
                        connectionToExecute.ParameterBuffer[9] = parameter8;
                    }

                    // Execute
                    if (connectionToExecute.MethodDelegate != null)
                    {
                        connectionToExecute.MethodDelegate.DynamicInvoke(connectionToExecute.ParameterBuffer);
                        component.DynamicInvoke(methodName, connectionToExecute.ParameterTypes, connectionToExecute.ParameterNames, connectionToExecute.ParameterBuffer, true);
                        component.InvokeComponents(parameter1, parameter2, parameter3, parameter4, parameter5, parameter6, parameter7, parameter8, methodName, depth + 1, this);
                    }
                    else
                    {
                        component.DynamicInvoke(methodName, connectionToExecute.ParameterTypes, connectionToExecute.ParameterNames, connectionToExecute.ParameterBuffer, false);
                    }
                }
            }

            // Reset Boxer
            Boxer.Reset();
        }

        #endregion
    }
}