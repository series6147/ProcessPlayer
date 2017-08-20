namespace ProcessPlayer.Data.Expressions
{
    public enum ECreatorPhase
    {
        eCreate,
        eCreateAndComplete,
        eCreationComplete
    }

    public enum EncodingClass
    {
        ascii,
        binary,
        unicode,
        utf8
    };

    public enum ESpecialNodes
    {
        eAnonymASTNode = -1001,
        eAnonymNTNode = -1000,
        eAnonymousNode = -100,
        eFatal = -10001
    }

    public enum FileEncoding
    {
        ascii,
        binary,
        none,
        unicode,
        uniCodeBOM,
        utf16be,
        utf16le,
        utf32be,
        utf32le,
        utf8
    };

    public enum UnicodeDetection
    {
        BOM,
        FirstCharIsAscii,
        notApplicable
    };
}
