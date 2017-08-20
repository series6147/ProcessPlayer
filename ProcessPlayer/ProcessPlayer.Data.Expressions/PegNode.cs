namespace ProcessPlayer.Data.Expressions
{
    public class PegNode
    {
        #region fields

        public PegBegEnd match;
        public int id;
        public PegNode child, next, parent;

        #endregion Data Members

        #region constructors

        public PegNode(PegNode parent, int id, PegBegEnd match, PegNode child, PegNode next)
        {
            this.child = child;
            this.id = id;
            this.match = match;
            this.next = next;
            this.parent = parent;
        }
        public PegNode(PegNode parent, int id, PegBegEnd match, PegNode child)
            : this(parent, id, match, child, null)
        {
        }
        public PegNode(PegNode parent, int id, PegBegEnd match)
            : this(parent, id, match, null, null)
        {
        }
        public PegNode(PegNode parent, int id)
            : this(parent, id, new PegBegEnd(), null, null)
        {
        }

        #endregion

        #region public members

        public virtual PegNode Clone()
        {
            var clone = new PegNode(parent, id, match);

            CloneSubTrees(clone);

            return clone;
        }

        public virtual string GetAsString(string s)
        {
            return match.GetAsString(s);
        }

        public virtual string GetAsString(string s, char quote)
        {
            return match.GetAsString(s).Replace('"', quote);
        }

        public virtual PegNode GetLastChild()
        {
            PegNode child, next = this.child;

            do
            {
                child = next;
            }
            while ((next = child.next) != null);

            return child;
        }

        #endregion

        #region protected members

        protected void CloneSubTrees(PegNode clone)
        {
            PegNode child = null, next = null;

            if (this.child != null)
            {
                child = this.child.Clone();
                child.parent = clone;
            }

            if (this.next != null)
            {
                next = this.next.Clone();
                next.parent = clone;
            }

            clone.child = child;
            clone.next = next;
        }

        #endregion
    }
}
