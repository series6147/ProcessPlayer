using System;

namespace ProcessPlayer.Data.Expressions
{
    public class ConditionalParser : PegCharParser
    {
        #region grammar rules

        public bool AdditiveExpression()
        {
            return TreeAST((int)EConditionalParser.additiveExpression, () =>
                Binary(MultiplicativeExpression, () =>
                    TreeChars(() => !Char('-', '>') && (Char('+') || Char('-')))));
        }

        public bool Between()
        {
            return TreeAST((int)EConditionalParser.between, () =>
                In()
                || And(() =>
                    Space()
                    && (Constant() || Identifier())
                    && Space()
                    && IChar("BETWEEN")
                    && Space()
                    && (Constant() || Identifier())
                    && Space()
                    && IChar("AND")
                    && Space()
                    && (Constant() || Identifier())));
        }

        public bool Binary(delMatcher operand, delMatcher Operator)
        {
            return And(() =>
                operand()
                && Space()
                && OptRepeat(() =>
                    And(() =>
                        Operator()
                        && Space()
                        && operand()
                        && Space())));
        }

        public bool Char()
        {
            return TreeAST((int)EConditionalParser.Char, () =>
                In('\u0020', '\u0026', '\u0028', '\u007f'));
        }

        public bool CharacterConstant()
        {
            return TreeAST((int)EConditionalParser.characterConstant, () =>
                And(() =>
                    OneOf("'")
                    && (EscapeSequence() || Char())
                    && OneOf("'")));
        }

        public bool ConditionalExpression()
        {
            return TreeAST((int)EConditionalParser.conditionalExpression, () =>
                And(LogicalExpression));
        }

        public bool Constant()
        {
            return Space()
                && (String() || CharacterConstant() || FloatingConstant() || IntegerConstant());
        }

        public bool EscapeSequence()
        {
            return TreeAST((int)EConditionalParser.escapeSequence, () =>
                And(() =>
                    Char('\\')
                    && (ForRepeat(1, 3, () =>
                        In('0', '7'))
                        || And(() =>
                            Char('x')
                            && PlusRepeat(() => In('0', '9', 'a', 'f', 'A', 'F'))))));
        }

        public bool EqualityExpression()
        {
            return TreeAST((int)EConditionalParser.equalityExpression, () =>
                Between()
                || Binary(RelationalExpression, () =>
                        Space()
                        && TreeChars(() =>
                            Char('!', '<')
                            || Char('!', '=')
                            || Char('!', '>')
                            || Char('<')
                            || Char('<', '=')
                            || Char('=')
                            || Char('>')
                            || Char('>', '='))));
        }

        public bool Exponent()
        {
            return TreeAST((int)EConditionalParser.exponent, () =>
                And(() =>
                    OneOf("eE")
                    && Option(() => OneOf("+-"))
                    && PlusRepeat(() => In('0', '9'))));
        }

        public bool Expression()
        {
            return TreeAST((int)EConditionalParser.expression, () =>
                And(() =>
                    Space()
                    && (LogicalExpression() || Constant() || Identifier())));
        }

        public bool FloatingConstant()
        {
            return TreeNT((int)EConditionalParser.floatingConstant, () =>
                And(() => IntegerConstant() && Exponent())
                || And(() =>
                    IntegerConstant()
                    && Char('.')
                    && Option(Fraction))
                    || And(() =>
                        Option(IntegerConstant)
                        && Char('.')
                        && Fraction()));
        }

        public bool Fraction()
        {
            return TreeAST((int)EConditionalParser.fraction, () =>
                And(() => IntegerConstant() && Option(Exponent)) || Exponent());
        }

        public bool Identifier()
        {
            return Space()
                && And(() =>
                    Char('\"')
                    && TreeNT((int)EConditionalParser.identifier, () =>
                        (In('\u0041', '\u005A', '\u0061', '\u007A') || IsLetter() || OneOf('#', '@', '_'))
                        && OptRepeat(() => In('\u0020', '\u0021') || In('\u0023', '\u003C', '\u003E', '\u005A', '\u005C', '\u005C', '\u005E', '\u007E') || IsLetter()))
                        && (Char('\"') || SyntaxError("<<'\"'>> expected")))
                        || TreeNT((int)EConditionalParser.identifier, () => And(() =>
                            (In('\u0041', '\u005A', '\u0061', '\u007A') || IsLetter() || OneOf('#', '@', '_'))
                            && OptRepeat(() => In('\u0030', '\u0039', '\u0041', '\u005A', '\u0061', '\u007A') || IsLetter() || OneOf('#', '$', '@', '_'))));
        }

        public bool In()
        {
            return TreeAST((int)EConditionalParser.In, () =>
                Like()
                || And(() =>
                    Space()
                    && (Constant() || Identifier())
                    && Space()
                    && ((IChar("NOT") && Space() && IChar("IN")) || IChar("IN"))
                    && Space()
                    && Char('(')
                    && Space()
                    && Binary(() => (Constant() || Identifier()), () => Char(','))
                    && Space()
                    && Char(')')));
        }

        public bool IntegerConstant()
        {
            return TreeAST((int)EConditionalParser.integerConstant, () =>
                PlusRepeat(() => In('0', '9')));
        }

        public bool Like()
        {
            return TreeAST((int)EConditionalParser.like, () =>
                And(() =>
                    Space()
                    && (Constant() || Identifier())
                    && Space()
                    && IChar("LIKE")
                    && Space()
                    && String()));
        }

        public bool LogicalClause()
        {
            return TreeNT((int)EConditionalParser.logicalClause, () =>
                And(() =>
                    Space()
                    && ((IChar("IS") && Space() && (IChar("NULL") || (IChar("NOT") && Space() && IChar("NULL"))))
                    || (IChar("NOT") && Space() && IChar("IS") && Space() && IChar("NULL")))));
        }

        public bool LogicalComparision()
        {
            return TreeAST((int)EConditionalParser.logicalComparision, () =>
                And(() =>
                    EqualityExpression()
                    && Space()
                    && TreeChars(() => IChar("IN")
                        || (IChar("BEGINSWITH"))
                        || (IChar("CONTAINS"))
                        || (IChar("ENDSWITH"))
                        || (IChar("NOTBEGINSWITH"))
                        || (IChar("NOTCONTAINS"))
                        || (IChar("NOTENDSWITH"))
                        || (IChar("NOT") && Space() && IChar("IN"))
                        || (IChar("NOT") && Space() && IChar("LIKE"))
                        || (IChar("LIKE"))
                        || (IChar("NOT")))
                    && Space()
                    && EqualityExpression()));
        }

        public bool LogicalExpression()
        {
            return TreeAST((int)EConditionalParser.logicalExpression, () =>
                Binary(EqualityExpression
                    , () => TreeChars(() => IChar("AND") || IChar("OR"))));
        }

        public bool MultiplicativeExpression()
        {
            return TreeAST((int)EConditionalParser.multiplicativeExpression, () =>
                Binary(UnaryExpression, () =>
                    TreeChars(() => !Char('/', '/') && (Char('*') || Char('/') || Char('%')))));
        }

        public bool PostfixExpression()
        {
            return TreeAST((int)EConditionalParser.postfixExpression, () =>
                And(() =>
                    Space()
                    && PrimaryExpression()
                    && OptRepeat(() =>
                        And(() =>
                            TreeChars(() => Char('.'))
                            && Space()
                            && (PrimaryExpression() || SyntaxError("<<identifier>> expected"))))));
        }

        public bool PrimaryExpression()
        {
            return TreeAST((int)EConditionalParser.primaryExpression, () =>
                And(() =>
                    Space()
                    && (Constant() || Identifier()
                    || And(() =>
                        Char('(')
                        && Space()
                        && LogicalExpression()
                        && Space()
                        && (Char(')'))))));
        }

        public bool RelationalExpression()
        {
            return TreeAST((int)EConditionalParser.relationalExpression, () =>
                Binary(AdditiveExpression, () =>
                        Space()
                        && TreeChars(() => !Char('<', '-') && (Char('>', '=') || Char('>') || Char('<', '=') || Char('<')))));
        }

        public bool Space()
        {
            return OptRepeat(() => OneOf(" \t\r\n"));
        }

        public bool String()
        {
            return TreeAST((int)EConditionalParser.String, () =>
                And(() =>
                    Char("'")
                    && OptRepeat(() => PlusRepeat(() => (In('\u0020', '\u0026', '\u0028', '\u007f') || OneOf(Environment.NewLine))) || EscapeSequence())
                    && Char("'")));
        }

        public bool UnaryExpression()
        {
            return TreeAST((int)EConditionalParser.unaryExpression, () =>
                (PostfixExpression() && Option(() => LogicalClause()))
                || And(() =>
                    Space()
                    && UnaryOperator()
                    && Space()
                    && (UnaryExpression() || SyntaxError("<<cast_expression>> expected"))));
        }

        public bool UnaryOperator()
        {
            return TreeAST((int)EConditionalParser.unaryOperator, () =>
                And(() =>
                    Char('+')
                    || (!Char('-', '>') && !Char('<', '-') && Char('-'))
                    || Char('~')));
        }

        #endregion
    }

    public enum EConditionalParser
    {
        additiveExpression,
        between,
        Char,
        characterConstant,
        chars,
        conditionalExpression,
        equalityExpression,
        escapeSequence,
        exponent,
        expression,
        floatingConstant,
        fraction,
        identifier,
        In,
        integerConstant,
        like,
        logicalClause,
        logicalComparision,
        logicalExpression,
        multiplicativeExpression,
        postfixExpression,
        primaryExpression,
        relationalExpression,
        String,
        unaryExpression,
        unaryOperator,
    }
}
