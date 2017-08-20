using ProcessPlayer.Content.Models;
using ProcessPlayer.Data.Expressions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProcessPlayer.Content.Utils
{
    public class FilterBulder
    {
        #region public methods

        public static string BuildFilter(string filterExpression, IEnumerable<FilterMember> filterMembers)
        {
            var res = filterExpression;

            if (!string.IsNullOrEmpty(filterExpression) && filterMembers != null)
            {
                var sbErr = new StringBuilder();

                using (var errOut = new StringWriter(sbErr))
                {
                    PegNode root;
                    var parser = new ConditionalParser();

                    parser.Construct(filterExpression, errOut);
                    parser.LogicalExpression();

                    if (sbErr.Length == 0 && (root = parser.GetRoot()) != null)
                        res = string.Join("", Composition.SplitAndTranslate(filterExpression
                            , filterMembers.Where(m => !string.IsNullOrEmpty(m.ID)).ToDictionary(m => m.ID, m => m)
                            , PegCharParser.GetDescendants(root).Where(n => n.id == (int)EConditionalParser.identifier)).Reverse().Select(c => c.String));
                    else
                        throw new Exception(sbErr.ToString());
                }
            }

            return res;
        }

        #endregion
    }
}
