namespace ProcessPlayer.Data.Expressions
{
    public class JScriptParser : PegCharParser
    {
        #region private variables

        private static readonly OptimizedLiterals _protectedProperties;
        private static readonly OptimizedLiterals _reservedWords;

        #endregion

        #region grammar rules

        public bool AdditiveExpression()
        {
            return TreeAST((int)EJScriptParser.additiveExpression, () =>
                OptBinary(MultiplicativeExpression, () => TreeChars(() => OneOf("+-"))));
        }

        public bool Array()
        {
            return TreeNT((int)EJScriptParser.array, () =>
                And(() => Space()
                    && Char('[')
                    && Space()
                    && Option(() => OptBinary(() => Expression() || Array() || Object(), () => Char(',')))
                    && Space()
                    && (Char(']') || SyntaxError("<<']'>> expected"))));
        }

        public bool AssignmentExpression()
        {
            return TreeAST((int)EJScriptParser.assignmentExpression, () =>
                And(() => Space()
                    && (PostfixExpression() || Identifier())
                    && Space()
                    && (And(() => TreeChars(() => Char('='))
                        && Space()
                        && (Function() || Expression() || Array() || Object()))
                        || And(() => TreeChars(() => Char("+=") || Char("-=") || Char("*=") || Char("/=") || Char("%="))
                            && Space()
                            && AdditiveExpression()))));
        }

        public bool AssignmentStatement()
        {
            return TreeNT((int)EJScriptParser.assignmentStatement, () =>
                And(() => AssignmentExpression()
                    && Space()
                    && Char(';')));
        }

        public bool B()
        {
            return Not(() => (In('A', 'Z', 'a', 'z', '0', '9') || OneOf("$_")));
        }

        public bool Block()
        {
            return TreeNT((int)EJScriptParser.block, () =>
                Space()
                && (And(() => Char('{')
                    && Space()
                    && OptRepeat(() => AssignmentStatement()
                        || Break()
                        || And(() => (Call() && Space() && (Char(';') || SyntaxError("<<';'>> expected"))))
                        || Comment()
                        || Continue()
                        || ForIn()
                        || If()
                        || And(() => (PostfixCall() && Space() && (Char(';') || SyntaxError("<<';'>> expected"))))
                        || Return()
                        || Switch()
                        || Variables()
                        || While())
                    && Space()
                    && (Char('}') || SyntaxError("<<'}'>> expected")))
                || And(() => AssignmentStatement()
                    || Break()
                    || And(() => (Call() && Space() && (Char(';') || SyntaxError("<<';'>> expected"))))
                    || Comment()
                    || Continue()
                    || ForIn()
                    || If()
                    || And(() => (PostfixCall() && Space() && (Char(';') || SyntaxError("<<';'>> expected"))))
                    || Return()
                    || Switch()
                    || Variables()
                    || While())));
        }

        public bool Break()
        {
            return TreeNT((int)EJScriptParser.Break, () =>
                And(() => Space()
                    && Char("break")
                    && Space()
                    && (Char(';') || SyntaxError("<<';'>> expected"))));
        }

        public bool Call()
        {
            return TreeNT((int)EJScriptParser.call, () =>
                And(() =>
                    Space()
                    && Identifier()
                    && Space()
                    && Char('(')
                    && Space()
                    && Option(() => OptBinary(() => Expression() || Array() || Object(), () => Char(',')))
                    && Space()
                    && (Char(')') || SyntaxError("<<')'>> expected"))));
        }

        public bool Case(ref int defaultCounter)
        {
            var res = TreeNT((int)EJScriptParser.Default, () =>
                And(() => Space()
                    && Char("default")
                    && Space()
                    && Char(':')
                    && Space()
                    && Option(Block)
                    && Space()
                    && Option(Break)));

            if (res)
                defaultCounter++;
            else
                res = TreeNT((int)EJScriptParser.Case, () =>
                    And(() => Space()
                        && Char("case")
                        && Space()
                        && (Number() || String())
                        && Space()
                        && Char(':')
                        && Space()
                        && Option(Block)
                        && Space()
                        && Option(Break)));

            return res;
        }

        public bool Comment()
        {
            return TreeAST((int)EJScriptParser.comment, () =>
                And(() => Char("/*")
                    && OptRepeat(() => Not(() => Char("*/")) && Any())
                    && (Char("*/") || SyntaxError("<<'*/'>> expected")))
                || And(() => Char("//")
                    && OptRepeat(() => NotOneOf("\r\n"))
                    && (OneOf("\r\n") || !Peek(Any))));
        }

        public bool ComparisonExpression()
        {
            return TreeAST((int)EJScriptParser.comparisonExpression, () =>
                OptBinary(AdditiveExpression, () => TreeChars(() => Char("===")
                    || Char("==")
                    || Char("!==")
                    || Char("!=")
                    || Char(">=")
                    || Char(">")
                    || Char("<=")
                    || Char("<"))));
        }

        public bool Continue()
        {
            return TreeNT((int)EJScriptParser.Continue, () =>
                And(() => Space()
                    && Char("continue")
                    && Space()
                    && (Char(';') || SyntaxError("<<';'>> expected"))));
        }

        public bool Else()
        {
            return TreeNT((int)EJScriptParser.Else, () =>
                And(() => Space()
                    && Char("else")
                    && Space()
                    && Block()));
        }

        public bool ElseIf()
        {
            return TreeNT((int)EJScriptParser.elseif, () =>
                And(() => Space()
                    && Char("else if")
                    && Space()
                    && Char('(')
                    && Space()
                    && Expression()
                    && Space()
                    && (Char(')') || SyntaxError("<<')'>> expected"))
                    && Space()
                    && Block()));
        }

        public bool Exponent()
        {
            return TreeAST((int)EJScriptParser.exponent, () =>
                OneOf("eE") && Option(() => OneOf("+-")) && PlusRepeat(() => In('0', '9')));
        }

        public bool Expression()
        {
            return TreeAST((int)EJScriptParser.expression, () =>
                And(() => TernaryOperator() || Not() || LogicalExpression()));
        }

        public bool ForIn()
        {
            return TreeNT((int)EJScriptParser.forIn, () =>
                And(() => Space()
                    && Char("for")
                    && Space()
                    && Char('(')
                    && Space()
                    && Option(() => Char("var"))
                    && Space()
                    && Identifier()
                    && Space()
                    && Char("in")
                    && Space()
                    && (Object() || PostfixExpression())
                    && Space()
                    && (Char(')') || SyntaxError("<<')'>> expected"))
                    && Space()
                    && Block()));
        }

        public bool ForStatement1()
        {
            return TreeNT((int)EJScriptParser.forStatement1, () =>
                And(() => Space()
                    && Option(() => Option(() => Char("var"))
                        && Space()
                        && OptBinary(AssignmentExpression, () => Char(',')))
                    && Space()
                    && (Char(';') || SyntaxError("<<';'>> expected"))));
        }

        public bool ForStatement2()
        {
            return TreeNT((int)EJScriptParser.forStatement2, () =>
                And(() => Space()
                    && Option(Expression)
                    && Space()
                    && (Char(';') || SyntaxError("<<';'>> expected"))));
        }

        public bool ForStatement3()
        {
            return TreeNT((int)EJScriptParser.forStatement3, () =>
                And(() => Space()
                    && OptBinary(AssignmentExpression, () => Char(','))));
        }

        public bool Function()
        {
            return TreeAST((int)EJScriptParser.function, () =>
                And(() =>
                    Space()
                    && Char("function")
                    && Space()
                    && Char('(')
                    && Space()
                    && Option(() => OptBinary(() => Identifier(), () => Char(',')))
                    && Space()
                    && (Char(')') || SyntaxError("<<')'>> expected"))
                    && Space()
                    && Char('{')
                    && Space()
                    && Option(Block)
                    && Space()
                    && (Char('}') || SyntaxError("<<'}'>> expected"))));
        }

        public bool Hexadecimal()
        {
            return TreeAST((int)EJScriptParser.hexadecimal, () =>
                IChar("0X") && PlusRepeat(() => In('0', '9', 'A', 'F', 'a', 'f')));
        }

        public bool Identifier()
        {
            return TreeAST((int)EJScriptParser.identifier, () =>
                And(() => Not(() => Keyword() && B()) && (IsLetter() || OneOf("$_")) && OptRepeat(() => IsLetter() || In('0', '9') || OneOf("$_"))));
        }

        public bool If()
        {
            return TreeNT((int)EJScriptParser.If, () =>
                And(() => Space()
                    && Char("if")
                    && Space()
                    && Char('(')
                    && Space()
                    && Expression()
                    && Space()
                    && (Char(')') || SyntaxError("<<')'>> expected"))
                    && Space()
                    && Block()
                    && OptRepeat(ElseIf)
                    && Option(Else)));
        }

        public bool Integer()
        {
            return TreeAST((int)EJScriptParser.integer, () =>
                PlusRepeat(() => In('0', '9')));
        }

        public bool Keyword()
        {
            return OneOfLiterals(_reservedWords);
        }

        public bool LogicalExpression()
        {
            return TreeAST((int)EJScriptParser.logicalExpression, () =>
                    OptBinary(() => Not() || ComparisonExpression(), () => TreeChars(() => Char("&&") || Char("||"))));
        }

        public bool Main()
        {
            return TreeNT((int)EJScriptParser.main, () =>
                And(() => PlusRepeat(() => AssignmentStatement()
                    || Break()
                    || And(() => (Call() && Space() && (Char(';') || SyntaxError("<<';'>> expected"))))
                    || Comment()
                    || Continue()
                    || ForIn()
                    || If()
                    || And(() => (PostfixCall() && Space() && (Char(';') || SyntaxError("<<';'>> expected"))))
                    || Return()
                    || Switch()
                    || Variables()
                    || While())));
        }

        public bool MultiplicativeExpression()
        {
            return TreeAST((int)EJScriptParser.multiplicativeExpression, () =>
                OptBinary(PrimaryExpression, () => TreeChars(() => OneOf("*/%"))));
        }

        public bool Not()
        {
            return TreeNT((int)EJScriptParser.not, () =>
                And(() => Space() && Char('!') && Space() && OptBinary(ComparisonExpression, () => TreeChars(() => Char("&&") || Char("||")))));
        }

        public bool Null()
        {
            return TreeAST((int)EJScriptParser.Null, () =>
                And(() => Char("null")));
        }

        public bool Number()
        {
            return TreeAST((int)EJScriptParser.number, () =>
                And(() => Option(() => OneOf("+-")) && (And(() => Integer() && Char('.') && Integer()) || And(Hexadecimal) || And(() => Integer() && Option(Exponent)))));
        }

        public bool Object()
        {
            return TreeNT((int)EJScriptParser.Object, () =>
                And(() => Space()
                    && Char('{')
                    && Space()
                    && OptBinary(ObjectPropertyDeclaration, () => Char(','))
                    && Space()
                    && (Char('}') || SyntaxError("<<'}'>> expected"))));
        }

        public bool ObjectPropertyDeclaration()
        {
            return TreeNT((int)EJScriptParser.objectPropertyDeclaration, () =>
                And(() => Space()
                    && String()
                    && Space()
                    && Char(':')
                    && Space()
                    && (Expression() || Array() || Object())));
        }

        public bool OptBinary(delMatcher operand, delMatcher Operator)
        {
            return And(() =>
                operand()
                && OptRepeat(() =>
                    Space()
                    && Operator()
                    && Space()
                    && operand()));
        }

        public bool PostfixCall()
        {
            return TreeAST((int)EJScriptParser.postfixCall, () =>
                And(() => Space()
                    && (Call() || Identifier() || (This() || TreeChars(() => Char('.')) || ThisProperty()))
                    && PlusRepeat(() => Space()
                        && ((TreeChars(() => Char('.')) && PostfixCall())
                        || Call()))));
        }

        public bool PostfixExpression()
        {
            return TreeAST((int)EJScriptParser.postfixExpression, () =>
                And(() => Space()
                    && Option(() => TreeChars(() => OneOf("+-")))
                    && (Call() || Identifier() || (This() || TreeChars(() => Char('.')) || ThisProperty()))
                    && OptRepeat(() => Space()
                        && ((TreeChars(() => Char('.')) && PostfixExpression())
                        || PropertyByIndex()
                        || PropertyByKey()
                        || PropertyByName()
                        || And(() =>
                            Char('(')
                            && Space()
                            && Expression()
                            && Space()
                            && (Char(')') || SyntaxError("<<')'>> expected")))))));
        }

        public bool PrimaryExpression()
        {
            return TreeAST((int)EJScriptParser.primaryExpression, () =>
                PostfixExpression()
                || And(() => Space()
                    && (Null()
                    || Number()
                    || String()
                    || This()
                    || Undefined()
                    || And(() =>
                        Char('(')
                        && Space()
                        && Expression()
                        && Space()
                        && (Char(')') || SyntaxError("<<')'>> expected"))))));
        }

        public bool PropertyByIndex()
        {
            return TreeNT((int)EJScriptParser.propertyByIndex, () =>
                And(() => Char('[')
                    && Space()
                    && (Number() || Identifier())
                    && Space()
                    && (Char(']') || SyntaxError("<<']'>> expected"))));
        }

        public bool PropertyByKey()
        {
            return TreeNT((int)EJScriptParser.propertyByKey, () =>
                And(() => Char('[')
                    && Space()
                    && String()
                    && Space()
                    && (Char(']') || SyntaxError("<<']'>> expected"))));
        }

        public bool PropertyByName()
        {
            return TreeNT((int)EJScriptParser.propertyByName, () =>
                And(() => Identifier()));
        }

        public bool ProtectedProperty()
        {
            return OneOfLiterals(_protectedProperties);
        }

        public bool Return()
        {
            return TreeNT((int)EJScriptParser.Return, () =>
                And(() => Space()
                    && Char("return")
                    && Space()
                    && (Array() || Expression() || Object())
                    && Space()
                    && (Char(';') || SyntaxError("<<';'>> expected"))));
        }

        public bool Space()
        {
            return OptRepeat(() => Comment() || OneOf(" \t\r\n"));
        }

        public bool String()
        {
            return TreeAST((int)EJScriptParser.String, () =>
                And(() => Char('\'')
                    && OptRepeat(() => Char("\\")
                        || Char("\\'")
                        || Char("\\\"")
                        || Char("\\n")
                        || Char("\\r")
                        || Char("\\t")
                        || Char("\\b")
                        || Char("\\f")
                        || NotOneOf("'"))
                    && (Char('\'') || SyntaxError("<<'''>> expected")))
                || And(() => Char('"')
                    && OptRepeat(() => Char("\\\\")
                        || Char("\\'")
                        || Char("\\\"")
                        || Char("\\n")
                        || Char("\\r")
                        || Char("\\t")
                        || Char("\\b")
                        || Char("\\f")
                        || NotOneOf("\""))
                    && (Char('"') || SyntaxError("<<'\"'>> expected"))));
        }

        public bool Switch()
        {
            var defaultCounter = 0;

            return TreeNT((int)EJScriptParser.Switch, () =>
                And(() => Space()
                    && Char("switch")
                    && Space()
                    && Char('(')
                    && Space()
                    && Expression()
                    && Space()
                    && (Char(')') || SyntaxError("<<')'>> expected"))
                    && Space()
                    && Char('{')
                    && Space()
                    && (PlusRepeat(() => Case(ref defaultCounter)) || (defaultCounter > 1 && SyntaxError("<<only one default is allowed>>")))
                    && Space()
                    && (Char('}') || SyntaxError("<<'}'>> expected"))));
        }

        public bool TernaryOperator()
        {
            return TreeAST((int)EJScriptParser.ternaryOperator, () =>
                And(() => Space()
                    && TernaryOperatorTest()
                    && Space()
                    && Char('?')
                    && Space()
                    && TernaryOperatorExpression1()
                    && Space()
                    && Char(':')
                    && Space()
                    && TernaryOperatorExpression2()));
        }

        public bool TernaryOperatorExpression1()
        {
            return TreeNT((int)EJScriptParser.ternaryOperatorExpression1, () =>
                And(() => Space()
                    && Expression()));
        }

        public bool TernaryOperatorExpression2()
        {
            return TreeNT((int)EJScriptParser.ternaryOperatorExpression2, () =>
                And(() => Space()
                    && Expression()));
        }

        public bool TernaryOperatorTest()
        {
            return TreeNT((int)EJScriptParser.ternaryOperatorTest, () =>
                And(() => Space()
                    && (Not() || LogicalExpression())));
        }

        public bool This()
        {
            return TreeNT((int)EJScriptParser.This, () =>
                And(() => Space()
                    && Char("this")));
        }

        public bool ThisProperty()
        {
            return TreeNT((int)EJScriptParser.thisProperty, () =>
                And(() => Not(() => ProtectedProperty() && B()) && Identifier()));
        }

        public bool Undefined()
        {
            return TreeAST((int)EJScriptParser.undefined, () =>
                And(() => Char("undefined")));
        }

        public bool Variables()
        {
            return TreeNT((int)EJScriptParser.variables, () =>
                And(() => Space()
                    && Char("var")
                    && Space()
                    && (OptBinary(() => AssignmentExpression() || Identifier(), () => Char(',')) || SyntaxError("<<declaration of variables>> expected"))
                    && Space()
                    && (Char(';') || SyntaxError("<<';'>> expected"))));
        }

        public bool While()
        {
            return TreeNT((int)EJScriptParser.While, () =>
                And(() => Space()
                    && Char("while")
                    && Space()
                    && Char('(')
                    && Space()
                    && Expression()
                    && Space()
                    && (Char(')') || SyntaxError("<<')'>> expected"))
                    && Space()
                    && Block()));
        }

        #endregion

        #region constructors

        static JScriptParser()
        {
            _protectedProperties = new OptimizedLiterals(new string[] { "CancellationToken", "Children", "ConditionDlg",
                "ExecuteAsync", "IncomingDataBuffer", "IncomingLinks", "Initialize", "OnCancelledDlg",
                "OnDataCommingDlg", "OnExecuteFinishedDlg", "OnExecuteStartedDlg", "OnLoadDlg", "OnVariableChangedDlg",
                "OutgoingLinks", "PerformDlg", "RaiseDataComming", "RaiseExecuteFinished", "RaiseExecuteStarted" });
            _reservedWords = new OptimizedLiterals(new string[] { "abstract", "arguments", "boolean", "break", "byte",
                "case", "catch", "char", "class", "const",
                "continue", "debugger", "default", "delete", "do",
                "double", "else", "enum", "eval", "export",
                "extends", "false", "final", "finally", "float",
                "for", "function", "goto", "if", "implements",
                "import", "in", "instanceof", "int", "interface",
                "let", "long", "native", "new", "null",
                "package", "private", "protected", "public", "return",
                "short", "static", "super", "switch", "synchronized",
                "this", "throw", "throws", "transient", "true",
                "try", "typeof", "var", "void", "volatile",
                "while", "with", "yield" });
        }

        #endregion
    }

    public enum EJScriptParser
    {
        additiveExpression,
        array,
        assignmentExpression,
        assignmentStatement,
        block,
        Break,
        call,
        Case,
        comment,
        comparisonExpression,
        Continue,
        Default,
        Else,
        elseif,
        exponent,
        expression,
        forIn,
        forStatement1,
        forStatement2,
        forStatement3,
        function,
        hexadecimal,
        identifier,
        If,
        integer,
        logicalExpression,
        main,
        multiplicativeExpression,
        not,
        Null,
        number,
        Object,
        objectPropertyDeclaration,
        postfixCall,
        postfixExpression,
        primaryExpression,
        propertyByIndex,
        propertyByKey,
        propertyByName,
        Return,
        String,
        Switch,
        ternaryOperator,
        ternaryOperatorExpression1,
        ternaryOperatorExpression2,
        ternaryOperatorTest,
        This,
        thisProperty,
        undefined,
        variables,
        While,
    }
}