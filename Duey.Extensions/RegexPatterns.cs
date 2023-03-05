using System.Text.RegularExpressions;

namespace Duey.Extensions;

public static class RegexPatterns
{
    /// <summary>
    /// #e = Bold text.
    /// </summary>
    public static readonly Regex AnyBold = new("#e");

    /// <summary>
    /// #n = Normal text (removes bold).
    /// </summary>
    public static readonly Regex AnyNotBold = new("#n");

    /// <summary>
    /// #b = Blue text.
    /// </summary>
    public static readonly Regex AnyBlueColor = new("#b");

    /// <summary>
    /// #d = Purple text.
    /// </summary>
    public static readonly Regex AnyPurpleColor = new("#d");

    /// <summary>
    /// #g = Green text.
    /// </summary>
    public static readonly Regex AnyGreenColor = new("#g");

    /// <summary>
    /// #k = Black text.
    /// </summary>
    public static readonly Regex AnyBlackColor = new("#k");

    /// <summary>
    /// #r = Red text.
    /// </summary>
    public static readonly Regex AnyRedColor = new("#r");

    /// <summary>
    /// #p[npcid]# - Shows the name of the NPC.
    /// </summary>
    public static readonly Regex AnyNpcName = new("#p[0-9]+#");

    /// <summary>
    /// #m[mapid]# - Shows the name of the map.
    /// </summary>
    public static readonly Regex AnyMapName = new("#m[0-9]+#");

    /// <summary>
    /// #o[mobid]# - Shows the name of the mob.
    /// </summary>
    public static readonly Regex AnyMobName = new("#o[0-9]+#");

    /// <summary>
    /// #t[itemid]# - Shows the name of the item.
    /// </summary>
    public static readonly Regex AnyItemName = new("#t[0-9]+#");

    /// <summary>
    /// #z[itemid]# - Shows the name of the item.
    /// </summary>
    public static readonly Regex AnyItemName2 = new("#z[0-9]+#");

    /// <summary>
    /// #i[itemid]# - Shows a picture of the item.
    /// </summary>
    public static readonly Regex AnyItemPicture = new("#i[0-9]+#");

    /// <summary>
    /// #v[itemid]# - Shows a picture of the item.
    /// </summary>
    public static readonly Regex AnyItemPicture2 = new("#v[0-9]+#");

    /// <summary>
    /// #s[skillid]# - Shows the image of the skill.
    /// </summary>
    public static readonly Regex AnySkillPicture = new("#q[0-9]+#");

    /// <summary>
    /// #q[skillid]# - Shows the name of the skill.
    /// </summary>
    public static readonly Regex AnySkillName = new("#s[0-9]+#");

    /// <summary>
    /// #c[itemid]# - Shows how many [itemid] the player has in their inventory.
    /// </summary>
    public static readonly Regex AnyItemCountInPlayersInv= new("#c[0-9]+#");

    /// <summary>
    /// #L[number]# - Selection list open.
    /// </summary>
    public static readonly Regex AnyListOpening= new("#L[0-9]+#");

    /// <summary>
    /// #l - Selection list close.
    /// </summary>
    public static readonly Regex AnyListEnding = new("#l");

    /// <summary>
    /// #h # - Shows the name of the player.
    /// </summary>
    public static readonly Regex AnyPlayerName = new("#h #");

    /// <summary>
    /// #B[%]# - Shows a 'progress' bar.
    /// </summary>
    public static readonly Regex AnyProgressBar = new("#B[%]#");

    /// <summary>
    /// unknown
    /// </summary>
    public static readonly Regex AnyUnknown = new("#x");

    /// <summary>
    /// #f[imagelocation]# - Shows an image inside the .wz files.
    /// </summary>
    public static readonly Regex ImageLocation = new("#f[a-zA-Z]+#");

    /// <summary>
    /// #F[imagelocation]# - Shows an image inside the .wz files.
    /// </summary>
    public static readonly Regex ImageLocation2 = new("#F[a-zA-Z]+#");
}