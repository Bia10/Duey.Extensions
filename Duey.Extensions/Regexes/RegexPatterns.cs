using System.Text.RegularExpressions;

namespace Duey.Extensions.Regexes;

public static partial class RegexPatterns
{
    public static readonly IReadOnlyList<Regex> AllRegexes = new List<Regex>
    {
        AnyHyperlinkPrefix()
        //AnyMobName(),
        //AnyNpcName(),
        //AnyMapName(),
        //AnyItemName(),
        //AnyItemName2(),
        //AnyItemPicture(),
        //AnyItemPicture2(),
        //AnySkillPicture(),
        //AnySkillName(),
        //AnyItemCountInPlayersInv(),
        //AnyListOpening(),
        //AnyListClosing(),
        //AnyPlayerName(),
        //AnyProgressBar(),
        //AnyUnknown(),
        //AnyImageLocation(),
        //AnyImageLocation2(),
        //AnyBoldText(),
        //AnyNotBoldText(),
        //AnyBlueColor(),
        //AnyPurpleColor(),
        //AnyGreenColor(),
        //AnyBlackColor(),
        //AnyRedColor()
    };

    public static readonly IReadOnlyList<Regex> AllFormattingRegexes = new List<Regex>
    {
        AnyListOpening(),
        AnyListClosing(),
        AnyProgressBar(),
        AnyBoldText(),
        AnyNotBoldText(),
        AnyBlueColor(),
        AnyPurpleColor(),
        AnyGreenColor(),
        AnyBlackColor(),
        AnyRedColor()
    };

    /// <summary>
    ///     #p[npcid]# - Shows the name of the NPC.
    /// </summary>
    [GeneratedRegex("#.[0-9]+", RegexOptions.IgnoreCase, 1000)]
    public static partial Regex AnyHyperlinkPrefix();

    /// <summary>
    ///     #p[npcid]# - Shows the name of the NPC.
    /// </summary>
    [GeneratedRegex("#p[0-9]+#", RegexOptions.IgnoreCase, 1000)]
    public static partial Regex AnyNpcName();

    /// <summary>
    ///     #m[mapid]# - Shows the name of the map.
    /// </summary>
    [GeneratedRegex("#m[0-9]+#", RegexOptions.IgnoreCase, 1000)]
    public static partial Regex AnyMapName();

    /// <summary>
    ///     #o[mobid]# - Shows the name of the mob.
    /// </summary>
    [GeneratedRegex("#o[0-9]+#", RegexOptions.IgnoreCase, 1000)]
    public static partial Regex AnyMobName();

    /// <summary>
    ///     #t[itemid]# - Shows the name of the item.
    /// </summary>
    [GeneratedRegex("#t[0-9]+#", RegexOptions.IgnoreCase, 1000)]
    public static partial Regex AnyItemName();

    /// <summary>
    ///     #z[itemid]# - Shows the name of the item.
    /// </summary>
    [GeneratedRegex("#z[0-9]+#", RegexOptions.IgnoreCase, 1000)]
    public static partial Regex AnyItemName2();

    /// <summary>
    ///     #i[itemid]# - Shows a picture of the item.
    /// </summary>
    [GeneratedRegex("#i[0-9]+#", RegexOptions.IgnoreCase, 1000)]
    public static partial Regex AnyItemPicture();

    /// <summary>
    ///     #v[itemid]# - Shows a picture of the item.
    /// </summary>
    [GeneratedRegex("#v[0-9]+#", RegexOptions.IgnoreCase, 1000)]
    public static partial Regex AnyItemPicture2();

    /// <summary>
    ///     #s[skillid]# - Shows the image of the skill.
    /// </summary>
    [GeneratedRegex("#s[0-9]+#", RegexOptions.IgnoreCase, 1000)]
    public static partial Regex AnySkillPicture();

    /// <summary>
    ///     #q[skillid]# - Shows the name of the skill.
    /// </summary>
    [GeneratedRegex("#q[0-9]+#", RegexOptions.IgnoreCase, 1000)]
    public static partial Regex AnySkillName();

    /// <summary>
    ///     #c[itemid]# - Shows how many [itemid] the player has in their inventory.
    /// </summary>
    [GeneratedRegex("#c[0-9]+#", RegexOptions.IgnoreCase, 1000)]
    public static partial Regex AnyItemCountInPlayersInv();

    /// <summary>
    ///     #L[number]# - Selection list open.
    /// </summary>
    [GeneratedRegex("#L[0-9]+#", RegexOptions.None, 1000)]
    public static partial Regex AnyListOpening();

    /// <summary>
    ///     #l - Selection list close.
    /// </summary>
    [GeneratedRegex("#l", RegexOptions.None, 1000)]
    public static partial Regex AnyListClosing();

    /// <summary>
    ///     #h # - Shows the name of the player.
    /// </summary>
    [GeneratedRegex("#h #", RegexOptions.None, 1000)]
    public static partial Regex AnyPlayerName();

    /// <summary>
    ///     #B[%]# - Shows a 'progress' bar.
    /// </summary>
    [GeneratedRegex("#B[%]+#", RegexOptions.None, 1000)]
    public static partial Regex AnyProgressBar();

    /// <summary>
    ///     unknown
    /// </summary>
    [GeneratedRegex("#x", RegexOptions.None, 1000)]
    public static partial Regex AnyUnknown();

    /// <summary>
    ///     f[imagelocation]# - Shows an image inside the .wz files.
    /// </summary>
    [GeneratedRegex("#f[a-zA-Z]+#", RegexOptions.None, 1000)]
    public static partial Regex AnyImageLocation();

    /// <summary>
    ///     #F[imagelocation]# - Shows an image inside the .wz files.
    /// </summary>
    [GeneratedRegex("#F[a-zA-Z]+#", RegexOptions.None, 1000)]
    public static partial Regex AnyImageLocation2();

    /// <summary>
    ///     #e = Bold text.
    /// </summary>
    [GeneratedRegex("#e", RegexOptions.IgnoreCase, 1000)]
    public static partial Regex AnyBoldText();

    /// <summary>
    ///     #n = Normal text (removes bold).
    /// </summary>
    [GeneratedRegex("#n", RegexOptions.IgnoreCase, 1000)]
    public static partial Regex AnyNotBoldText();

    /// <summary>
    ///     #b = Blue text.
    /// </summary>
    [GeneratedRegex("#b", RegexOptions.IgnoreCase, 1000)]
    public static partial Regex AnyBlueColor();

    /// <summary>
    ///     #d = Purple text.
    /// </summary>
    [GeneratedRegex("#d", RegexOptions.IgnoreCase, 1000)]
    public static partial Regex AnyPurpleColor();

    /// <summary>
    ///     #g = Green text.
    /// </summary>
    [GeneratedRegex("#g", RegexOptions.IgnoreCase, 1000)]
    public static partial Regex AnyGreenColor();

    /// <summary>
    ///     #k = Black text.
    /// </summary>
    [GeneratedRegex("#k", RegexOptions.IgnoreCase, 1000)]
    public static partial Regex AnyBlackColor();

    /// <summary>
    ///     #r = Red text.
    /// </summary>
    [GeneratedRegex("#r", RegexOptions.IgnoreCase, 1000)]
    public static partial Regex AnyRedColor();
}