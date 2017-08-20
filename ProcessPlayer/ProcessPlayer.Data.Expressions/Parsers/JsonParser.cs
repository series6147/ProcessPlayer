using System;
using System.IO;

namespace ProcessPlayer.Data.Expressions
{
    public class JsonParser : PegCharParser
    {
        #region Input Properties

        public static EncodingClass encodingClass = EncodingClass.unicode;
        public static UnicodeDetection unicodeDetection = UnicodeDetection.FirstCharIsAscii;

        #endregion Input Properties

        #region Constructors

        public JsonParser()
            : base()
        {
        }
        public JsonParser(string src, TextWriter FerrOut)
            : base(src, FerrOut)
        {
        }

        #endregion Constructors

        #region Overrides

        public override void GetProperties(out EncodingClass encoding, out UnicodeDetection detection)
        {
            encoding = encodingClass;
            detection = unicodeDetection;
        }

        public override string GetRuleNameFromId(int id)
        {
            try
            {
                int val;
                var ruleEnum = (EJsonParser)id;
                var s = ruleEnum.ToString();

                return int.TryParse(s, out val) ? base.GetRuleNameFromId(id) : s;
            }
            catch (Exception)
            {
                return base.GetRuleNameFromId(id);
            }
        }

        #endregion Overrides

        #region Grammar Rules

        public bool Array()
        {
            return TreeNT((int)EJsonParser.array, () =>
                And(() => Space()
                    && Char('[')
                    && Space()
                    && (Peek(() => Char(']')) || Elements())
                    && Space()
                    && (Char(']') || SyntaxError("<<']'>> expected"))));
        }

        public bool Char()
        {
            return Escape() || And(() => Not(() => OneOf("\"\\") || ControlChars()) && UnicodeChar());
        }

        public bool ControlChars()
        {
            return In('\u0000', '\u001f');
        }

        public bool Elements()
        {
            return And(() => Value()
                && OptRepeat(() => And(() => Space() && Char(',') && Space() && Value())));
        }

        public bool Escape()
        {
            return TreeAST((int)EJsonParser.escape, () =>
                And(() => Char('\\')
                    && (OneOf(optimizedCharset0)
                    || And(() => Char('u') && (ForRepeat(4, 4, () => In('0', '9', 'A', 'F', 'a', 'f'))
                        || SyntaxError("4 hex digits expected")))
                    || SyntaxError("illegal escape"))));
        }

        public bool Exp()
        {
            return TreeAST((int)EJsonParser.exp, () =>
                And(() => OneOf("eE")
                    && OneOf("-+")
                    && PlusRepeat(() => In('0', '9'))));
        }

        public bool ExpectFileEnd()
        {
            return TreeAST((int)EJsonParser.expectFileEnd, () =>
                Not(() => Any()) || Warning("non-json stuff before end of file"));
        }

        public bool False()
        {
            return TreeNT((int)EJsonParser.False, () =>
                Char('f', 'a', 'l', 's', 'e'));
        }

        public bool Frac()
        {
            return TreeAST((int)EJsonParser.frac, () =>
                And(() => Char('.') && PlusRepeat(() => In('0', '9'))));
        }

        public bool JsonText()
        {
            return TreeNT((int)EJsonParser.jsonText, () =>
                And(() => Space() && TopElement() && ExpectFileEnd()));
        }

        public bool Identifier()
        {
            return TreeAST((int)EJsonParser.identifier, () =>
                And(() => (IsLetter() || OneOf("_")) && OptRepeat(() => IsLetter() || In('0', '9') || OneOf("_"))));
        }

        public bool Include()
        {
            return TreeNT((int)EJsonParser.include, () =>
                And(() => IChar("#INCLUDE")
                    && Space()
                    && String()));
        }

        public bool IncludeRelative()
        {
            return TreeNT((int)EJsonParser.includeRelative, () =>
                And(() => IChar("#INCLUDE")
                    && Space()
                    && String()
                    && Space()
                    && Char('$')
                    && String()));
        }

        public bool Integer()
        {
            return TreeAST((int)EJsonParser.integer, () =>
                Char('0') || And(() => In('1', '9') && OptRepeat(() => In('0', '9'))));
        }

        public bool Members()
        {
            return TreeAST((int)EJsonParser.members, () =>
                And(() => Space()
                    && Pair()
                    && Space()
                    && OptRepeat(() => And(() => Char(',') && Space() && Pair() && Space()))));
        }

        public bool Null()
        {
            return TreeNT((int)EJsonParser.Null, () =>
                Char('n', 'u', 'l', 'l'));
        }

        public bool Number()
        {
            return TreeNT((int)EJsonParser.number, () =>
                And(() => Option(() => OneOf("-+"))
                    && Integer()
                    && Option(() => Frac())
                    && Option(() => Exp())));
        }

        public bool Object()
        {
            return TreeNT((int)EJsonParser.Object, () =>
                And(() => Space()
                    && Char('{')
                    && Space()
                    && (Peek(() => Char('}')) || Members())
                    && Space()
                    && (Char('}') || SyntaxError("<<'}'>> expected"))));
        }

        public bool Pair()
        {
            return TreeNT((int)EJsonParser.pair, () =>
                And(() => Space()
                    && (Identifier() || String() || SyntaxError("<<identifier or string>> expected"))
                    && Space()
                    && (Char(':') || SyntaxError("<<':'>> expected"))
                    && Space()
                    && Value()));
        }

        public bool Preprocess()
        {
            return TreeNT((int)EJsonParser.preprocess, () =>
                OptRepeat(() => IncludeRelative() || Include() || Any()));
        }

        public bool Space()
        {
            return OptRepeat(() => OneOf(" \t\r\n"));
        }

        public bool String()
        {
            return TreeAST((int)EJsonParser.String, () =>
                And(() => Char('"')
                    && OptRepeat(() => Char() || OneOf("\t\r\n"))
                    && (Char('"') || SyntaxError("<<'\"'>> expected"))));
        }

        public bool TopElement()
        {
            return TreeAST((int)EJsonParser.topElement, () =>
                Object()
                || Array()
                || SyntaxError("json file must start with '{' or '['"));
        }

        public bool True()
        {
            return TreeNT((int)EJsonParser.True, () =>
                 Char('t', 'r', 'u', 'e'));
        }

        public bool UnicodeChar()
        {
            return In('\u0000', '\uffff');
        }

        public bool Value()
        {
            return TreeAST((int)EJsonParser.value, () =>
                And(() => Space()
                    && (Array()
                    || False()
                    || Null()
                    || Number()
                    || Object()
                    || String()
                    || True())
                    || SyntaxError("<<(string or number or object or array or 'true' or 'false' or 'null')>> expected")));
        }

        #endregion Grammar Rules

        #region Optimization Data

        internal static OptimizedCharset optimizedCharset0;

        static JsonParser()
        {
            char[] oneOfChars = new char[] { '"', '\\', '/', 'b', 'f', 'n', 'r', 't' };

            optimizedCharset0 = new OptimizedCharset(null, oneOfChars);
        }

        #endregion Optimization Data
    }

    public enum EJsonParser
    {
        array,
        elements,
        escape,
        exp,
        expectFileEnd,
        False,
        frac,
        identifier,
        include,
        includeRelative,
        integer,
        jsonText,
        members,
        Null,
        number,
        Object,
        pair,
        preprocess,
        space,
        String,
        topElement,
        True,
        value,
    };
}