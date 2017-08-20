using ProcessPlayer.Data.Expressions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProcessPlayer.Data.CodeGen.Generators
{
    public class JScriptGen : CodeGen
    {
        #region private variables

        private int _fIndex;
        private readonly StringBuilder _sbFun = new StringBuilder();
        private string _functionDeclaration;

        #endregion

        #region protected methods

        protected virtual void AdditiveExpression(PegNode node, StringBuilder sb, int spaceCount)
        {
            DefaultNodeGen(node, sb, 0, false);
        }

        protected virtual void Array(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append('[')
                .Append(string.Join(",", GetNodeChildren(node).Select(n => n.GetAsString(_expression))))
                .Append(']');
        }

        protected virtual void AssignmentExpression(PegNode node, StringBuilder sb, int spaceCount)
        {
            DefaultNodeGen(node, sb, 0, false);
        }

        protected virtual void AssignmentStatement(PegNode node, StringBuilder sb, int spaceCount)
        {
            TruncateOrTerminateLine(sb);

            sb.Append(string.Empty.PadRight(IndentSize * spaceCount, IndentChar));

            DefaultNodeGen(node.child, sb, 0, false);
        }

        protected virtual void Block(PegNode node, StringBuilder sb, int spaceCount)
        {
            DefaultNodeGen(node, sb, spaceCount, false);
        }

        protected virtual void Break(PegNode node, StringBuilder sb, int spaceCount)
        {
            TruncateOrTerminateLine(sb);

            sb.Append(string.Empty.PadRight(IndentSize * spaceCount, IndentChar))
                .AppendLine(node.parent.id == (int)EJScriptParser.Case ? "return" : "break");
        }

        protected virtual void Call(PegNode node, StringBuilder sb, int spaceCount)
        {
            var name = node.child.GetAsString(_expression);

            if (spaceCount > 0)
            {
                TruncateOrTerminateLine(sb);

                sb.Append(string.Empty.PadRight(IndentSize * spaceCount, IndentChar));
            }

            sb.Append(name)
                .Append('(');

            foreach (var child in GetNodeChildren(node).Skip(1))
                sb.Append(GetNodeString(child))
                    .Append(',');

            while (sb.Length > 0 && sb[sb.Length - 1] == ',')
                sb.Length--;

            sb.Append(')');
        }

        protected virtual void Comment(PegNode node, StringBuilder sb, int spaceCount)
        {
        }

        protected virtual void ComparisonExpression(PegNode node, StringBuilder sb, int spaceCount)
        {
            DefaultNodeGen(node, sb, 0, false);
        }

        protected virtual void Continue(PegNode node, StringBuilder sb, int spaceCount)
        {
            TruncateOrTerminateLine(sb);

            sb.Append(string.Empty.PadRight(IndentSize * spaceCount, IndentChar))
                .AppendLine("continue");
        }

        protected virtual void Else(PegNode node, StringBuilder sb, int spaceCount)
        {
            TruncateOrTerminateLine(sb);

            sb.Append(string.Empty.PadRight(IndentSize * spaceCount, IndentChar))
                .AppendLine("else:");

            var block = GetNodeChildren(node).FirstOrDefault(n => n.id == (int)EJScriptParser.block);

            if (block == null)
                sb.Append(string.Empty.PadRight(IndentSize * (spaceCount + 1), IndentChar))
                    .AppendLine("pass");
            else
            {
                TruncateOrTerminateLine(sb);
                DefaultNodeGen(block, sb, spaceCount + 1, false);
            }
        }

        protected virtual void ElseIf(PegNode node, StringBuilder sb, int spaceCount)
        {
            TruncateOrTerminateLine(sb);

            sb.Append(string.Empty.PadRight(IndentSize * spaceCount, IndentChar))
                .Append("elif (")
                .Append(GetNodeString(node.child))
                .AppendLine("):");

            var block = GetNodeChildren(node).FirstOrDefault(n => n.id == (int)EJScriptParser.block);

            if (block == null)
                sb.Append(string.Empty.PadRight(IndentSize * (spaceCount + 1), IndentChar))
                    .AppendLine("pass");
            else
            {
                TruncateOrTerminateLine(sb);
                DefaultNodeGen(block, sb, spaceCount + 1, false);
            }
        }

        protected virtual void Exponent(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append(node.GetAsString(_expression));
        }

        protected virtual void Expression(PegNode node, StringBuilder sb, int spaceCount)
        {
            DefaultNodeGen(node, sb, 0, false);
        }

        protected virtual void ForIn(PegNode node, StringBuilder sb, int spaceCount)
        {
            TruncateOrTerminateLine(sb);

            sb.Append(string.Empty.PadRight(IndentSize * spaceCount, IndentChar))
                .Append("for (")
                .Append(GetNodeString(node.child))
                .Append(" in ")
                .Append(GetNodeString(node.child.next))
                .AppendLine("):");

            var block = GetNodeChildren(node).FirstOrDefault(n => n.id == (int)EJScriptParser.block);

            if (block == null)
                sb.Append(string.Empty.PadRight(IndentSize * (spaceCount + 1), IndentChar))
                    .AppendLine("pass");
            else
            {
                TruncateOrTerminateLine(sb);
                DefaultNodeGen(block, sb, spaceCount + 1, false);
            }
        }

        protected virtual void Function(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.AppendFormat("f{0}", ++_fIndex);

            _sbFun.AppendFormat("def f{0}({1}):", _fIndex, string.Join(",", GetNodeChildren(node).Where(n => n.id == (int)EJScriptParser.identifier).Select(n => GetNodeString(n))))
                .AppendLine();

            var block = GetNodeChildren(node).FirstOrDefault(n => n.id == (int)EJScriptParser.block);

            if (block == null)
                _sbFun.Append(string.Empty.PadRight(IndentSize * 1, IndentChar))
                    .AppendLine("pass");
            else
            {
                TruncateOrTerminateLine(_sbFun);
                DefaultNodeGen(block, _sbFun, 1, false);
            }
        }

        protected virtual void Hexadecimal(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append(node.GetAsString(_expression));
        }

        protected virtual void Identifier(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append(node.GetAsString(_expression));
        }

        protected virtual void If(PegNode node, StringBuilder sb, int spaceCount)
        {
            TruncateOrTerminateLine(sb);

            sb.Append(string.Empty.PadRight(IndentSize * spaceCount, IndentChar))
                .Append("if (")
                .Append(GetNodeString(node.child))
                .AppendLine("):");

            var block = GetNodeChildren(node).FirstOrDefault(n => n.id == (int)EJScriptParser.block);

            if (block == null)
                sb.Append(string.Empty.PadRight(IndentSize * (spaceCount + 1), IndentChar))
                    .AppendLine("pass");
            else
            {
                TruncateOrTerminateLine(sb);
                DefaultNodeGen(block, sb, spaceCount + 1, false);
            }

            foreach (var e in GetNodeChildren(node).Where(n => n.id == (int)EJScriptParser.Else || n.id == (int)EJScriptParser.elseif))
                if (e.id == (int)EJScriptParser.Else)
                    Else(e, sb, spaceCount);
                else
                    ElseIf(e, sb, spaceCount);
        }

        protected virtual void Integer(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append(node.GetAsString(_expression));
        }

        protected virtual void LogicalExpression(PegNode node, StringBuilder sb, int spaceCount)
        {
            foreach (var n in GetNodeChildren(node))
            {
                if (Enum.IsDefined(typeof(EJScriptParser), n.id))
                    _actions[n.id](n, sb, spaceCount);
                else if (string.Equals(n.GetAsString(_expression), "&&"))
                    sb.Append(" and ");
                else if (string.Equals(n.GetAsString(_expression), "||"))
                    sb.Append(" or ");
                else
                    sb.Append(n.GetAsString(_expression));
            }
        }

        protected virtual void Main(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append(string.Format(@"#!/usr/bin/python
#coding: utf-8

import clr

from System import DBNull
from ProcessPlayer.Data.Functions.DateTimeExtensions import *
from ProcessPlayer.Data.Functions.LogExtensions import *
from ProcessPlayer.Data.Functions.MathExtensions import *
from ProcessPlayer.Data.Functions.StringExtensions import *

{0}
def {1}:", _sbFun.ToString(), string.IsNullOrEmpty(_functionDeclaration) ? "Perform(arg1, *vartuple)" : _functionDeclaration))
         .AppendLine();

            foreach (var n in GetNodeChildren(node))
            {
                if (Enum.IsDefined(typeof(EJScriptParser), n.id))
                    _actions[n.id](n, sb, 1);
                else
                    sb.Append(string.Empty.PadRight(1, IndentChar))
                        .Append(n.GetAsString(_expression));
            }
        }

        protected virtual void MultiplicativeExpression(PegNode node, StringBuilder sb, int spaceCount)
        {
            DefaultNodeGen(node, sb, 0, false);
        }

        protected virtual void Not(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append("not (")
                .Append(GetNodeString(node.child))
                .Append(")");
        }

        protected virtual void Null(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append("None");
        }

        protected virtual void Number(PegNode node, StringBuilder sb, int spaceCount)
        {
            DefaultNodeGen(node, sb, 0, false);
        }

        protected virtual void Object(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append('{');

            foreach (var child in GetNodeChildren(node))
                sb.Append(GetNodeString(child))
                    .Append(',');

            while (sb.Length > 0 && sb[sb.Length - 1] == ',')
                sb.Length--;

            sb.Append('}');
        }

        protected virtual void ObjectProperty(PegNode node, StringBuilder sb, int spaceCount)
        {
            DefaultNodeGen(node, sb, 0, false);
        }

        protected virtual void ObjectPropertyDeclaration(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append(node.child.GetAsString(_expression))
                .Append(':')
                .Append(GetNodeString(node.child.next));
        }

        protected virtual void PostfixCall(PegNode node, StringBuilder sb, int spaceCount)
        {
            if (spaceCount > 0)
            {
                TruncateOrTerminateLine(sb);

                sb.Append(string.Empty.PadRight(IndentSize * spaceCount, IndentChar));
            }

            DefaultNodeGen(node, sb, 0, false);
        }

        protected virtual void PostfixExpression(PegNode node, StringBuilder sb, int spaceCount)
        {
            DefaultNodeGen(node, sb, 0, false);
        }

        protected virtual void PrimaryExpression(PegNode node, StringBuilder sb, int spaceCount)
        {
            DefaultNodeGen(node, sb, 0, false);
        }

        protected virtual void PropertyByIndex(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append(node.GetAsString(_expression));
        }

        protected virtual void PropertyByKey(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append(node.GetAsString(_expression));
        }

        protected virtual void PropertyByName(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append(node.GetAsString(_expression));
        }

        protected virtual void Return(PegNode node, StringBuilder sb, int spaceCount)
        {
            TruncateOrTerminateLine(sb);

            sb.Append(string.Empty.PadRight(IndentSize * spaceCount, IndentChar))
                .Append("return")
                .AppendLine(node.child == null ? null : string.Concat(" ", GetNodeString(node.child)));
        }

        protected virtual void String(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append(node.GetAsString(_expression));
        }

        protected virtual void Switch(PegNode node, StringBuilder sb, int spaceCount)
        {
            TruncateOrTerminateLine(sb);

            sb.Append(string.Empty.PadRight(IndentSize * spaceCount, IndentChar))
                .AppendFormat("f{0}(", _fIndex + 1)
                .Append(GetNodeString(node.child))
                .AppendLine(")");

            foreach (var c in GetNodeChildren(node).Where(n => n.id == (int)EJScriptParser.Case || n.id == (int)EJScriptParser.Default).OrderBy(n => n.id))
            {
                switch (c.id)
                {
                    case (int)EJScriptParser.Case:
                        _sbFun.AppendFormat("def f{0}(arg):", ++_fIndex)
                            .AppendLine()
                            .Append(" if (arg is None || (")
                            .Append(GetNodeString(c.child))
                            .AppendLine(")):");
                        break;
                    case (int)EJScriptParser.Default:
                        _sbFun.AppendFormat("def f{0}(arg):", ++_fIndex)
                            .AppendLine();
                        break;
                }

                if (GetNodeChildren(c).Skip(1).Any())
                    foreach (var n in GetNodeChildren(c).Skip(1))
                    {
                        TruncateOrTerminateLine(_sbFun);
                        DefaultNodeGen(n, _sbFun, 2, false);
                    }
                else
                    _sbFun.AppendLine("  pass");

                _sbFun.AppendFormat(" f{0}(None)", _fIndex + 1)
                    .AppendLine();
            }

            _sbFun.AppendFormat("def f{0}(arg):", ++_fIndex)
                .AppendLine()
                .AppendLine(" return");
        }

        protected virtual void TernaryOperator(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append("((")
                .Append(GetNodeString(node.child.next))
                .Append(") if (")
                .Append(GetNodeString(node.child))
                .Append(") else (")
                .Append(GetNodeString(node.child.next.next))
                .Append("))");
        }

        protected virtual void This(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append("this");
        }

        protected virtual void ThisProperty(PegNode node, StringBuilder sb, int spaceCount)
        {
            DefaultNodeGen(node, sb, 0, false);
        }

        protected virtual void Undefined(PegNode node, StringBuilder sb, int spaceCount)
        {
            sb.Append("None");
        }

        protected virtual void Variables(PegNode node, StringBuilder sb, int spaceCount)
        {
            TruncateOrTerminateLine(sb);

            foreach (var v in GetNodeChildren(node))
                DefaultNodeGen(v, sb, spaceCount, false);
        }

        protected virtual void While(PegNode node, StringBuilder sb, int spaceCount)
        {
            TruncateOrTerminateLine(sb);

            sb.Append(string.Empty.PadRight(IndentSize * spaceCount, IndentChar))
                .Append("while (")
                .Append(GetNodeString(node.child))
                .AppendLine("):");

            var block = GetNodeChildren(node).FirstOrDefault(n => n.id == (int)EJScriptParser.block);

            if (block == null)
                sb.Append(string.Empty.PadRight(IndentSize * (spaceCount + 1), IndentChar))
                    .AppendLine("pass");
            else
            {
                TruncateOrTerminateLine(sb);
                DefaultNodeGen(block, sb, spaceCount + 1, false);
            }
        }

        #endregion

        #region public methods

        public void Generate(string src, StringBuilder sbOut, IDictionary<string, int> properties, char indentChar, int indentSize, string functionDeclaration)
        {
            _functionDeclaration = functionDeclaration;

            Generate(src, sbOut, properties, indentChar, indentSize);
        }

        #endregion

        #region CodeGen Members

        public override void Generate(string src, StringBuilder sbOut, IDictionary<string, int> properties, char indentChar, int indentSize)
        {
            var parser = new JScriptParser();

            _expression = src;
            _parser = parser;
            _properties = properties;

            IndentChar = indentChar;
            IndentSize = indentSize > 0 ? indentSize : 1;

            using (var errOut = new StringWriter(sbOut))
            {
                parser.Construct(src, errOut);
                parser.Main();

                var root = parser.GetRoot();

                if (root == null)
                    throw new Exception("expression is not valid.");

                Main(root, sbOut, 0);
            }
        }

        protected override Type EnumType { get { return typeof(EJScriptParser); } }

        #endregion

        #region constructors

        public JScriptGen()
        {
            _actions = new Dictionary<int, Action<PegNode, StringBuilder, int>>
            {
            { (int)EJScriptParser.additiveExpression, AdditiveExpression },
            { (int)EJScriptParser.array, Array },
            { (int)EJScriptParser.assignmentExpression, AssignmentExpression },
            { (int)EJScriptParser.assignmentStatement, AssignmentStatement },
            { (int)EJScriptParser.block, Block },
            { (int)EJScriptParser.Break, Break },
            { (int)EJScriptParser.call, Call },
            { (int)EJScriptParser.comment, Comment },
            { (int)EJScriptParser.comparisonExpression, ComparisonExpression },
            { (int)EJScriptParser.Continue, Continue },
            { (int)EJScriptParser.Else, Else },
            { (int)EJScriptParser.elseif, ElseIf },
            { (int)EJScriptParser.exponent, Exponent },
            { (int)EJScriptParser.expression, Expression },
            { (int)EJScriptParser.forIn, ForIn },
            { (int)EJScriptParser.function, Function },
            { (int)EJScriptParser.hexadecimal, Hexadecimal },
            { (int)EJScriptParser.identifier, Identifier },
            { (int)EJScriptParser.If, If },
            { (int)EJScriptParser.integer, Integer },
            { (int)EJScriptParser.logicalExpression, LogicalExpression },
            { (int)EJScriptParser.main, Block },
            { (int)EJScriptParser.multiplicativeExpression, MultiplicativeExpression },
            { (int)EJScriptParser.not, Not },
            { (int)EJScriptParser.Null, Null },
            { (int)EJScriptParser.number, Number },
            { (int)EJScriptParser.Object, Object },
            { (int)EJScriptParser.postfixCall, PostfixCall },
            { (int)EJScriptParser.postfixExpression, PostfixExpression },
            { (int)EJScriptParser.propertyByIndex, PropertyByIndex },
            { (int)EJScriptParser.propertyByKey, PropertyByKey },
            { (int)EJScriptParser.propertyByName, PropertyByName },
            { (int)EJScriptParser.objectPropertyDeclaration, ObjectPropertyDeclaration },
            { (int)EJScriptParser.primaryExpression, PrimaryExpression },
            { (int)EJScriptParser.Return, Return },
            { (int)EJScriptParser.String, String },
            { (int)EJScriptParser.Switch, Switch },
            { (int)EJScriptParser.ternaryOperator, TernaryOperator },
            { (int)EJScriptParser.This, This },
            { (int)EJScriptParser.thisProperty, ThisProperty },
            { (int)EJScriptParser.undefined, Undefined },
            { (int)EJScriptParser.variables, Variables },
            { (int)EJScriptParser.While, While } };
        }

        #endregion
    }
}
