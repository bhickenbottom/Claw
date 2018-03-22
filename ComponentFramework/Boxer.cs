namespace ComponentFramework
{
    using System;
    using System.Collections.Generic;

    public static class Boxer
    {
        #region Static Constructors

        static Boxer()
        {
            Boxer.Resetters = new List<Action>();
        }

        #endregion

        #region Static Fields

        internal static List<Action> Resetters;

        #endregion

        #region Static Methods

        public static Boxed<T> Box<T>(T value)
        {
            return Boxer<T>.Box(value);
        }

        public static Boxed<T> Box<T>()
        {
            return Boxer<T>.Box(default(T));
        }

        public static void Reset()
        {
            int resetterCount = Boxer.Resetters.Count;
            for (int i = 0; i < resetterCount; i++)
            {
                Boxer.Resetters[i]();
            }
        }

        #endregion
    }

    internal static class Boxer<T>
    {
        #region Static Constructors

        static Boxer()
        {
            Boxer<T>.boxes = new List<Boxed<T>>();
            Boxer.Resetters.Add(
                () =>
                {
                    int boxCount = Boxer<T>.boxes.Count;
                    for (int i = 0; i < boxCount; i++)
                    {
                        Boxed<T> boxed = Boxer<T>.boxes[i];
                        boxed.IsInUse = false;
                    }
                });
        }

        #endregion

        #region Static Fields

        private static List<Boxed<T>> boxes;

        #endregion

        #region Static Methods

        public static Boxed<T> Box(T value)
        {
            int boxCount = Boxer<T>.boxes.Count;
            for (int i = 0; i < boxCount; i++)
            {
                Boxed<T> boxed = Boxer<T>.boxes[i];
                if (!boxed.IsInUse)
                {
                    boxed.Value = value;
                    return boxed;
                }
            }
            Boxed<T> newBoxed = new Boxed<T>(value);
            Boxer<T>.boxes.Add(newBoxed);
            return newBoxed;
        }

        #endregion
    }
}