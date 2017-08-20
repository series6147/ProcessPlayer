using ProcessPlayer.Data.Expressions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProcessPlayer.Data.CodeGen.Generators
{
    public class ConditionGen : CodeGen
    {
        #region protected methods

        protected virtual void AdditiveExpression(PegNode node, StringBuilder sb, int spaceCount)
        {
            DefaultNodeGen(node, sb, 0, true);
        }

        protected virtual void Between(PegNode node, StringBuilder sb, int spaceCount)
        {
            var children = GetNodeChildren(node).ToArray();

            sb.AppendFormat("{0}>={1} and {0}<={2}"
                , GetNodeString(children[0])
                , GetNodeString(children[1])
                , GetNodeString(children[2]));
        }

        protected virtual void Char(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.AppendFormat("'{0}'", node.GetAsString(_expression));
        }

        protected virtual void CharacterConstant(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.AppendFormat("'{0}'", node.GetAsString(_expression));
        }

        protected virtual void Constant(PegNode node, StringBuilder sb, int spaceCount)
        {
            DefaultNodeGen(node, sb, 0, false);
        }

        protected virtual void EqualityExpression(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append('(');

            foreach (var n in GetNodeChildren(node))
            {
                if (Enum.IsDefined(typeof(EConditionalParser), n.id))
                    _actions[n.id](n, sb, spaceCount);
                else if (string.Equals(n.GetAsString(_expression), "!<"))
                    sb.Append(">=");
                else if (string.Equals(n.GetAsString(_expression), "!>"))
                    sb.Append("<=");
                else if (string.Equals(n.GetAsString(_expression), "="))
                    sb.Append("==");
                else
                    sb.Append(n.GetAsString(_expression));
            }

            sb.Append(')');
        }

        protected virtual void EscapeSequence(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append(node.GetAsString(_expression));
        }

        protected virtual void Exponent(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append(node.GetAsString(_expression));
        }

        protected virtual void Expression(PegNode node, StringBuilder sb, int spaceCount)
        {
            if (spaceCount > 0)
                sb.AppendLine();

            DefaultNodeGen(node, sb, spaceCount, false);
        }

        protected virtual void FloatingConstant(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append(node.GetAsString(_expression));
        }

        protected virtual void Fraction(PegNode node, StringBuilder sb, int spaceCount)
        {
            DefaultNodeGen(node, sb, 0, false);
        }

        protected virtual void Identifier(PegNode node, StringBuilder sb, int spaceCount)
        {
            var identifier = node.GetAsString(_expression).Trim('\"');

            if (_properties.ContainsKey(identifier))
                sb.AppendFormat("values[{0}]", _properties[identifier]);
            else
                sb.Append(string.Equals(identifier, "NULL", StringComparison.InvariantCultureIgnoreCase) ? "None" : node.GetAsString(_expression));
        }

        protected virtual void IntegerConstant(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append(node.GetAsString(_expression));
        }

        protected virtual void LogicalClause(PegNode node, StringBuilder sb, int spaceCount)
        {
            switch (node.GetAsString(_expression).Trim().ToUpper())
            {
                case "IS NOT NULL":
                    sb.Append("!=None");
                    break;
                case "IS NULL":
                    sb.Append("==None");
                    break;
            }
        }

        protected virtual void LogicalComparision(PegNode node, StringBuilder sb, int spaceCount)
        {
            var children = GetNodeChildren(node).ToArray();

            switch (children[1].GetAsString(_expression).Trim().ToUpper())
            {
                case "BEGINSWITH":
                    sb.AppendFormat("{1}.find(u'{0}'.encode('utf-8'))==0"
                        , GetNodeString(children[2])
                        , GetNodeString(children[0]));
                    break;
                case "CONTAINS":
                    sb.AppendFormat("{1}.find(u'{0}'.encode('utf-8'))>=0"
                        , GetNodeString(children[2])
                        , GetNodeString(children[0]));
                    break;
                case "ENDSWITH":
                    sb.AppendFormat("{1}.find(u'{0}'.encode('utf-8'))==len({0})-len({1})"
                        , GetNodeString(children[2])
                        , GetNodeString(children[0]));
                    break;
                case "IN":
                    sb.Append(GetNodeString(children[0]))
                        .Append(" in [")
                        .Append(children[2].GetAsString(_expression))
                        .Append(']');
                    break;
                case "IS NOT NULL":
                    sb.AppendFormat("{0}!=None", GetNodeString(children[0]));
                    break;
                case "IS NULL":
                    sb.AppendFormat("{0}==None", GetNodeString(children[0]));
                    break;
                case "NOTBEGINSWITH":
                    sb.AppendFormat("{1}.find(u'{0}'.encode('utf-8'))!=0"
                        , GetNodeString(children[2])
                        , GetNodeString(children[0]));
                    break;
                case "NOTCONTAINS":
                    sb.AppendFormat("{1}.find(u'{0}'.encode('utf-8'))<0"
                        , GetNodeString(children[2])
                        , GetNodeString(children[0]));
                    break;
                case "NOTENDSWITH":
                    sb.AppendFormat("{1}.find(u'{0}'.encode('utf-8'))!=len({0})-len({1})"
                        , GetNodeString(children[2])
                        , GetNodeString(children[0]));
                    break;
                case "NOT IN":
                    sb.Append(GetNodeString(children[0]))
                        .Append(" not in [")
                        .Append(children[2].GetAsString(_expression))
                        .Append(']');
                    break;
                case "NOT LIKE":
                    if (children[2].GetAsString(_expression).StartsWith("%"))
                    {
                        sb.AppendFormat(
                            children[2].GetAsString(_expression).EndsWith("%")
                                ? "{1}.find(u'{0}'.encode('utf-8'))<0"
                                : "{1}.find(u'{0}'.encode('utf-8'))!=0"
                            , GetNodeString(children[2]).Trim('%', '"')
                            , GetNodeString(children[0]));
                    }
                    else if (children[2].GetAsString(_expression).EndsWith("%"))
                        sb.AppendFormat("{1}.find(u'{0}'.encode('utf-8'))!=len({0})-len({1})"
                            , GetNodeString(children[2]).Trim('%', '"')
                            , GetNodeString(children[0]));
                    break;
                case "LIKE":
                    if (children[2].GetAsString(_expression).StartsWith("%"))
                    {
                        sb.AppendFormat(
                            children[2].GetAsString(_expression).EndsWith("%")
                                ? "{1}.find(u'{0}'.encode('utf-8'))>=0"
                                : "{1}.find(u'{0}'.encode('utf-8'))==0"
                            , GetNodeString(children[2]).Trim('%', '"')
                            , GetNodeString(children[0]));
                    }
                    else if (children[2].GetAsString(_expression).EndsWith("%"))
                        sb.AppendFormat("{1}.find(u'{0}'.encode('utf-8'))==len({0})-len({1})"
                            , GetNodeString(children[2]).Trim('%', '"')
                            , GetNodeString(children[0]));
                    break;
            }
        }

        protected virtual void LogicalExpression(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append('(');

            foreach (var n in GetNodeChildren(node))
            {
                if (Enum.IsDefined(typeof(EConditionalParser), n.id))
                    _actions[n.id](n, sb, spaceCount);
                else if (string.Equals(n.GetAsString(_expression), "AND", StringComparison.InvariantCultureIgnoreCase))
                    sb.Append(" and ");
                else if (string.Equals(n.GetAsString(_expression), "OR", StringComparison.InvariantCultureIgnoreCase))
                    sb.Append(" or ");
                else
                    sb.Append(n.GetAsString(_expression));
            }

            sb.Append(')');
        }

        protected virtual void Main(PegNode node, StringBuilder sb, int spaceCount)
        {
            foreach (var n in GetNodeChildren(node))
            {
                if (Enum.IsDefined(typeof(EConditionalParser), n.id))
                    _actions[n.id](n, sb, 0);
                else
                    sb.Append(string.Empty.PadRight(0, IndentChar))
                        .Append(n.GetAsString(_expression));
            }

            sb.Insert(0, @"#!/usr/bin/python
#coding: utf-8

import clr

from System import DBNull
from ProcessPlayer.Data.Functions.DateTimeExtensions import *
from ProcessPlayer.Data.Functions.LogExtensions import *
from ProcessPlayer.Data.Functions.MathExtensions import *
from ProcessPlayer.Data.Functions.StringExtensions import *

def Filter(values):
 return ");
        }

        protected virtual void MultiplicativeExpression(PegNode node, StringBuilder sb, int spaceCount)
        {
            DefaultNodeGen(node, sb, 0, true);
        }

        protected virtual void PostfixExpression(PegNode node, StringBuilder sb, int spaceCount)
        {
            throw new NotImplementedException();
        }

        protected virtual void PrimaryExpression(PegNode node, StringBuilder sb, int spaceCount)
        {
            DefaultNodeGen(node, sb, 0, false);
        }

        protected virtual void RelationalExpression(PegNode node, StringBuilder sb, int spaceCount)
        {
            DefaultNodeGen(node, sb, 0, true);
        }

        protected virtual void String(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.AppendFormat("{0}", node.GetAsString(_expression));
        }

        protected virtual void UnaryExpression(PegNode node, StringBuilder sb, int spaceCount)
        {
            DefaultNodeGen(node, sb, 0, false);
        }

        protected virtual void UnaryOperator(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append(node.GetAsString(_expression));
        }

        protected virtual void Variable(PegNode node, StringBuilder sb, int spaceCount)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region properties

        protected override Type EnumType
        {
            get { return typeof(EConditionalParser); }
        }

        #endregion

        #region constructors

        public ConditionGen()
        {
            _actions = new Dictionary<int, Action<PegNode, StringBuilder, int>>
            {
            { (int)EConditionalParser.additiveExpression, AdditiveExpression },
            { (int)EConditionalParser.between, Between },
            { (int)EConditionalParser.Char, Char },
            { (int)EConditionalParser.characterConstant, CharacterConstant },
            { (int)EConditionalParser.equalityExpression, EqualityExpression },
            { (int)EConditionalParser.escapeSequence, EscapeSequence },
            { (int)EConditionalParser.exponent, Exponent },
            { (int)EConditionalParser.expression, Expression },
            { (int)EConditionalParser.floatingConstant, FloatingConstant },
            { (int)EConditionalParser.fraction, Fraction },
            { (int)EConditionalParser.identifier, Identifier },
            { (int)EConditionalParser.integerConstant, IntegerConstant },
            { (int)EConditionalParser.logicalClause, LogicalClause},
            { (int)EConditionalParser.logicalComparision, LogicalComparision },
            { (int)EConditionalParser.logicalExpression, LogicalExpression },
            { (int)EConditionalParser.multiplicativeExpression, MultiplicativeExpression },
            { (int)EConditionalParser.postfixExpression, PostfixExpression },
            { (int)EConditionalParser.primaryExpression, PrimaryExpression },
            { (int)EConditionalParser.relationalExpression, RelationalExpression },
            { (int)EConditionalParser.String, String },
            { (int)EConditionalParser.unaryExpression, UnaryExpression },
            { (int)EConditionalParser.unaryOperator, UnaryOperator } };
        }

        #endregion

        #region CodeGen Members

        public override void Generate(string src, StringBuilder sbOut, IDictionary<string, int> properties, char indentChar, int indentSize)
        {
            var parser = new ConditionalParser();

            _expression = src;
            _parser = parser;
            _properties = properties;

            IndentChar = indentChar;
            IndentSize = indentSize > 0 ? indentSize : 1;

            using (var errOut = new StringWriter(sbOut))
            {
                parser.Construct(src, errOut);
                parser.ConditionalExpression();

                var root = parser.GetRoot();

                if (root == null)
                    throw new Exception("expression is not valid.");

                Main(root, sbOut, 0);
            }
        }

        #endregion
    }
}
