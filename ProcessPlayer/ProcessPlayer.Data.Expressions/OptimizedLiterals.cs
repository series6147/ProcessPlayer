namespace ProcessPlayer.Data.Expressions
{
    public sealed class OptimizedLiterals
    {
        #region fields

        internal Trie literalsRoot;

        #endregion

        #region constructors

        public OptimizedLiterals(string[] literalsAlternatives)
        {
            literalsRoot = new Trie('\u0000', 0, literalsAlternatives);
        }

        #endregion
    }
}
