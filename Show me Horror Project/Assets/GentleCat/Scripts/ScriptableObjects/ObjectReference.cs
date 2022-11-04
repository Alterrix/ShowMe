namespace GentleCat.ScriptableObjects
{
    public abstract class ObjectReference<T, J> where J : ObjectVariable<T>
    {
        public T constantValue;
        public bool useConstant = true;
        public J variable;

        protected ObjectReference()
        {
        }

        public ObjectReference(T value)
        {
            useConstant = true;
            constantValue = value;
        }

        public T Value
        {
            get => useConstant ? constantValue : variable.CurrentValue;
            set
            {
                if (useConstant) constantValue = value;
                else variable.CurrentValue = value;
            }
        }


        public static implicit operator T(ObjectReference<T, J> reference)
        {
            return reference.Value;
        }
    }
}