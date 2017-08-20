using ProcessPlayer.Content.Models;
using ProcessPlayer.Data.Expressions;
using System.Collections.Generic;
using System.Linq;

namespace ProcessPlayer.Content.Utils
{
    public partial class Composition
    {
        #region public methods

        public void Translate(IDictionary<string, FilterMember> filterMembers)
        {
            foreach (var m in GetType().GetMethods().Where(m => !string.Equals(m.Name, "Translate") && m.Name.StartsWith("Translate")))
                m.Invoke(this, new object[] { filterMembers });
        }

        #endregion

        #region public static methods

        public static IEnumerable<Composition> Split(string src, IEnumerable<PegNode> nodes)
        {
            var posEnd = src.Length - 1;

            foreach (var n in nodes.OrderByDescending(n => n.match.posEnd))
            {
                if (posEnd > n.match.posEnd)
                    yield return new Composition() { String = src.Substring(n.match.posEnd, posEnd - n.match.posEnd) };

                yield return new Composition() { ID = n.id, String = src.Substring(n.match.posBeg, n.match.posEnd - n.match.posBeg) };

                posEnd = n.match.posBeg;
            }

            if (posEnd > 0)
                yield return new Composition() { String = src.Substring(0, posEnd) };
        }

        public static IEnumerable<Composition> SplitAndTranslate(string src, IDictionary<string, FilterMember> filterMembers, IEnumerable<PegNode> nodes)
        {
            Composition cps;
            var posEnd = src.Length;

            foreach (var n in nodes.OrderByDescending(n => n.match.posEnd))
            {
                if (posEnd > n.match.posEnd)
                {
                    cps = new Composition() { String = src.Substring(n.match.posEnd, posEnd - n.match.posEnd) };
                    cps.Translate(filterMembers);

                    yield return cps;
                }

                cps = new Composition() { ID = n.id, String = src.Substring(n.match.posBeg, n.match.posEnd - n.match.posBeg) };
                cps.Translate(filterMembers);

                yield return cps;

                posEnd = n.match.posBeg;
            }

            if (posEnd > 0)
            {
                cps = new Composition() { String = src.Substring(0, posEnd) };
                cps.Translate(filterMembers);

                yield return cps;
            }
        }

        #endregion

        #region properties

        public virtual object ID { get; set; }

        public virtual string String { get; set; }

        #endregion
    }
}
