using ProcessPlayer.Content.Models;
using ProcessPlayer.Data.Common;
using ProcessPlayer.Data.Expressions;
using System;
using System.Collections.Generic;

namespace ProcessPlayer.Content.Utils
{
    public partial class Composition
    {
        #region private methods

        private static string convertCondition(string condition, FilterMember member)
        {
            switch (condition)
            {
                case "contains":
                    member.Value = string.Concat("'%", member.Value.Trim('%'), "%'");

                    return "LIKE";
                case "endswith":
                    member.Value = string.Concat("'%", member.Value.Trim('%'), "'");

                    return "LIKE";
                case "in":
                    member.Value = string.Concat("(", member.Value.TrimStart('(').TrimEnd(')'), ")");

                    return "IN";
                case "isnotnull":
                    member.Value = null;

                    return "IS NOT NULL";
                case "isnull":
                    member.Value = null;

                    return "IS NULL";
                case "startswith":
                    member.Value = string.Concat("'", member.Value.Trim('%'), "%'");

                    return "LIKE";
                default:
                    return condition;
            }
        }

        private static string convertValue(string value, FilterMember member)
        {
            switch (member.Type)
            {
                case "Date":
                    return value.ToValue<DateTime>().ToString("yyyyMMdd");
                case "Number":
                    return value;
                default:
                    return string.Concat("'", value.Trim('\''), "'");
            }
        }

        #endregion

        #region public methods

        public void TranslateIdentifier(IDictionary<string, FilterMember> filterMembers)
        {
            FilterMember member;

            if (Equals(ID, (int)EConditionalParser.identifier)
                && filterMembers != null
                && filterMembers.TryGetValue(String.ToValue<string>(), out member))
                String = string.IsNullOrEmpty(member.Table)
                    ? string.Concat("\""
                        , member.Field
                        , "\" "
                        , convertCondition(member.Condition, member)
                        , " "
                        , convertValue(member.Value, member))
                    : string.Concat("\""
                        , member.Table
                        , "\".\""
                        , member.Field
                        , "\" "
                        , convertCondition(member.Condition, member)
                        , " "
                        , convertValue(member.Value, member));
        }

        #endregion
    }
}
