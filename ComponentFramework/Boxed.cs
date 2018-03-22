namespace ComponentFramework
{
    public class Boxed<T>
    {
        #region Static Methods

        public static implicit operator T(Boxed<T> boxed)
        {
            return boxed.Value;
        }

        #endregion

        #region Constructors

        public Boxed(T value)
        {
            this.IsInUse = true;
            this.Value = value;
        }

        #endregion

        #region Fields

        public bool IsInUse;

        public T Value;

        #endregion

        #region Method Overrides

        public override string ToString()
        {
            if (this.Value != null)
            {
                return this.Value.ToString();
            }
            return "null";
        }

        #endregion
    }
}