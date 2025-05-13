namespace OperationResult
{
    /// <summary>
    /// Represents a successful result with no return value.
    /// Used as a placeholder in Result&lt;None&gt;.
    /// </summary>
    public sealed class None
    {
        private None()
        {
        }

        public static readonly None Value = new();
    }
}