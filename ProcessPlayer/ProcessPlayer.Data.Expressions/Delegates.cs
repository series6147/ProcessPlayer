using ProcessPlayer.Data.Expressions;

public delegate PegNode delCreator(ECreatorPhase ePhase, PegNode parentOrCreated, int id);
public delegate string delGetNodeName(PegNode node);
public delegate bool delMatcher();
public delegate bool delSpaceMatcher(ref int spaceCount, int minSpaceCount);
