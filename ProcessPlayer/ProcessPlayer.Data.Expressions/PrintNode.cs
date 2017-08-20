namespace ProcessPlayer.Data.Expressions
{
    public abstract class PrintNode
    {
        #region public methods

        public abstract bool IsLeaf(PegNode node);

        public virtual bool IsSkip(PegNode node) { return false; }

        public abstract int LenDistNext(PegNode node, bool alignVertical, ref int offsetLineBeg, int level);

        public abstract int LenLeaf(PegNode node);

        public abstract int LenMaxLine();

        public abstract int LenNodeBeg(PegNode node);

        public abstract int LenNodeEnd(PegNode node);

        public abstract void PrintDistNext(PegNode node, bool alignVertical, ref int offsetLineBeg, int level);

        public abstract void PrintLeaf(PegNode node, ref int offsetLineBeg, bool alignVertical);

        public abstract void PrintNodeBeg(PegNode node, bool alignVertical, ref int offsetLineBeg, int level);

        public abstract void PrintNodeEnd(PegNode node, bool alignVertical, ref int offsetLineBeg, int level);

        #endregion
    }
}
