using System.Text.RegularExpressions;

namespace Duey.Extensions;

public static partial class RegexPatterns
{
    /// <summary>
    /// #p[npcid]# - Shows the name of the NPC.
    /// </summary>
    [GeneratedRegex("#p[0-9]+#")]
    public static partial Regex AnyNpcName();

    /// <summary>
    /// #m[mapid]# - Shows the name of the map.
    /// </summary>
    [GeneratedRegex("#m[0-9]+#")]
    public static partial Regex AnyMapName();

    /// <summary>
    /// #o[mobid]# - Shows the name of the mob.
    /// </summary>
    [GeneratedRegex("#o[0-9]+#")]
    public static partial Regex AnyMobName();

    /// <summary>
    /// #t[itemid]# - Shows the name of the item.
    /// </summary>
    [GeneratedRegex("#t[0-9]+#")]
    public static partial Regex AnyItemName();

    /// <summary>
    /// #z[itemid]# - Shows the name of the item.
    /// </summary>
    [GeneratedRegex("#z[0-9]+#")]
    public static partial Regex AnyItemName2();

    /// <summary>
    /// #i[itemid]# - Shows a picture of the item.
    /// </summary>
    [GeneratedRegex("#i[0-9]+#")]
    public static partial Regex AnyItemPicture();

    /// <summary>
    ///     #v[itemid]# - Shows a picture of the item.
    /// </summary>
    [GeneratedRegex("#v[0-9]+#")]
    public static partial Regex AnyItemPicture2();

    /// <summary>
    /// #s[skillid]# - Shows the image of the skill.
    /// </summary>
    [GeneratedRegex("#s[0-9]+#")]
    public static partial Regex AnySkillPicture();

    /// <summary>
    /// #q[skillid]# - Shows the name of the skill.
    /// </summary>
    [GeneratedRegex("#q[0-9]+#")]
    public static partial Regex AnySkillName();

    /// <summary>
    /// #c[itemid]# - Shows how many [itemid] the player has in their inventory.
    /// </summary>
    [GeneratedRegex("#c[0-9]+#")]
    public static partial Regex AnyItemCountInPlayersInv();

    /// <summary>
    /// #L[number]# - Selection list open.
    /// </summary>
    [GeneratedRegex("#L[0-9]+#")]
    public static partial Regex AnyListOpening();

    /// <summary>
    /// #l - Selection list close.
    /// </summary>
    [GeneratedRegex("#l")]
    public static partial Regex AnyListClosing();

    /// <summary>
    /// #h # - Shows the name of the player.
    /// </summary>
    [GeneratedRegex("#h #")]
    public static partial Regex AnyPlayerName();

    /// <summary>
    /// #B[%]# - Shows a 'progress' bar.
    /// </summary>
    [GeneratedRegex("#B[%]+#")]
    public static partial Regex AnyProgressBar();

    /// <summary>
    /// unknown
    /// </summary>
    [GeneratedRegex("#x")]
    public static partial Regex AnyUnknown();

    /// <summary>
    /// f[imagelocation]# - Shows an image inside the .wz files.
    /// </summary>
    [GeneratedRegex("#f[a-zA-Z]+#")]
    public static partial Regex ImageLocation();

    /// <summary>
    /// #F[imagelocation]# - Shows an image inside the .wz files.
    /// </summary>
    [GeneratedRegex("#F[a-zA-Z]+#")]
    public static partial Regex ImageLocation2();

    public static Match AnyPlayerNameFirstMatch(ReadOnlySpan<char> input)
    {
        return AnyPlayerName().Match(input.ToString());
    }

    public static Regex.ValueMatchEnumerator AnyPlayerNameAllMatches(ReadOnlySpan<char> input)
    {
        return AnyPlayerName().EnumerateMatches(input);
    }

    public static Match AnyProgressBarFirstMatch(ReadOnlySpan<char> input)
    {
        return AnyProgressBar().Match(input.ToString());
    }

    public static Regex.ValueMatchEnumerator AnyProgressBarAllMatches(ReadOnlySpan<char> input)
    {
        return AnyProgressBar().EnumerateMatches(input);
    }

    public static Match AnyUnknownFirstMatch(ReadOnlySpan<char> input)
    {
        return AnyUnknown().Match(input.ToString());
    }

    public static Regex.ValueMatchEnumerator AnyUnknownAllMatches(ReadOnlySpan<char> input)
    {
        return AnyUnknown().EnumerateMatches(input);
    }

    public static Match AnyImageLocationFirstMatch(ReadOnlySpan<char> input)
    {
        return ImageLocation().Match(input.ToString());
    }

    public static Regex.ValueMatchEnumerator AnyImageLocationAllMatches(ReadOnlySpan<char> input)
    {
        return ImageLocation().EnumerateMatches(input);
    }

    public static Match AnyImageLocation2FirstMatch(ReadOnlySpan<char> input)
    {
        return ImageLocation2().Match(input.ToString());
    }

    public static Regex.ValueMatchEnumerator AnyImageLocation2AllMatches(ReadOnlySpan<char> input)
    {
        return ImageLocation2().EnumerateMatches(input);
    }

    public static class TextFormatting
    {
        /// <summary>
        ///     #e = Bold text.
        /// </summary>
        public static readonly Regex AnyBold = new("#e");

        /// <summary>
        ///     #n = Normal text (removes bold).
        /// </summary>
        public static readonly Regex AnyNotBold = new("#n");

        /// <summary>
        ///     #b = Blue text.
        /// </summary>
        public static readonly Regex AnyBlueColor = new("#b");

        /// <summary>
        ///     #d = Purple text.
        /// </summary>
        public static readonly Regex AnyPurpleColor = new("#d");

        /// <summary>
        ///     #g = Green text.
        /// </summary>
        public static readonly Regex AnyGreenColor = new("#g");

        /// <summary>
        ///     #k = Black text.
        /// </summary>
        public static readonly Regex AnyBlackColor = new("#k");

        /// <summary>
        ///     #r = Red text.
        /// </summary>
        public static readonly Regex AnyRedColor = new("#r");
    }

    public static class Npc
    {
        public static Match AnyNpcNameFirstMatch(ReadOnlySpan<char> input)
        {
            return AnyNpcName().Match(input.ToString());
        }

        public static Regex.ValueMatchEnumerator AnyNpcNameAllMatches(ReadOnlySpan<char> input)
        {
            return AnyNpcName().EnumerateMatches(input);
        }
    }

    public static class Map
    {
        public static Match AnyMapNameFirstMatch(ReadOnlySpan<char> input)
        {
            return AnyMapName().Match(input.ToString());
        }

        public static Regex.ValueMatchEnumerator AnyMapNameAllMatches(ReadOnlySpan<char> input)
        {
            return AnyMapName().EnumerateMatches(input);
        }
    }

    public static class Mob
    {
        public static Match AnyMobNameFirstMatch(ReadOnlySpan<char> input)
        {
            return AnyMobName().Match(input.ToString());
        }

        public static Regex.ValueMatchEnumerator AnyMobNameAllMatches(ReadOnlySpan<char> input)
        {
            return AnyMobName().EnumerateMatches(input);
        }
    }

    public static class Item
    {
        public static Match AnyItemNameFirstMatch(ReadOnlySpan<char> input)
        {
            return AnyItemName().Match(input.ToString());
        }

        public static Regex.ValueMatchEnumerator AnyItemNameAllMatches(ReadOnlySpan<char> input)
        {
            return AnyItemName().EnumerateMatches(input);
        }

        public static Match AnyItemName2FirstMatch(ReadOnlySpan<char> input)
        {
            return AnyItemName2().Match(input.ToString());
        }

        public static Regex.ValueMatchEnumerator AnyItemName2AllMatches(ReadOnlySpan<char> input)
        {
            return AnyItemName2().EnumerateMatches(input);
        }

        public static Match AnyItemPictureFirstMatch(ReadOnlySpan<char> input)
        {
            return AnyItemPicture().Match(input.ToString());
        }

        public static Regex.ValueMatchEnumerator AnyItemPictureAllMatches(ReadOnlySpan<char> input)
        {
            return AnyItemPicture().EnumerateMatches(input);
        }

        public static Match AnyItemPicture2FirstMatch(ReadOnlySpan<char> input)
        {
            return AnyItemPicture2().Match(input.ToString());
        }

        public static Regex.ValueMatchEnumerator AnyItemPicture2AllMatches(ReadOnlySpan<char> input)
        {
            return AnyItemPicture2().EnumerateMatches(input);
        }

        public static Match AnyItemCountInPlayersInvFirstMatch(ReadOnlySpan<char> input)
        {
            return AnyItemCountInPlayersInv().Match(input.ToString());
        }

        public static Regex.ValueMatchEnumerator AnyItemCountInPlayersInvAllMatches(ReadOnlySpan<char> input)
        {
            return AnyItemCountInPlayersInv().EnumerateMatches(input);
        }
    }

    public static class Skill
    {
        public static Match AnySkillNameFirstMatch(ReadOnlySpan<char> input)
        {
            return AnySkillName().Match(input.ToString());
        }

        public static Regex.ValueMatchEnumerator AnySkillNameAllMatches(ReadOnlySpan<char> input)
        {
            return AnySkillName().EnumerateMatches(input);
        }

        public static Match AnySkillPictureFirstMatch(ReadOnlySpan<char> input)
        {
            return AnySkillPicture().Match(input.ToString());
        }

        public static Regex.ValueMatchEnumerator AnySkillPictureAllMatches(ReadOnlySpan<char> input)
        {
            return AnySkillPicture().EnumerateMatches(input);
        }
    }

    public static class List
    {
        public static Match AnyListOpeningFirstMatch(ReadOnlySpan<char> input)
        {
            return AnyListOpening().Match(input.ToString());
        }

        public static Regex.ValueMatchEnumerator AnyListOpeningAllMatches(ReadOnlySpan<char> input)
        {
            return AnyListOpening().EnumerateMatches(input);
        }

        public static Match AnyListClosingFirstMatch(ReadOnlySpan<char> input)
        {
            return AnyListClosing().Match(input.ToString());
        }

        public static Regex.ValueMatchEnumerator AnyListClosingAllMatches(ReadOnlySpan<char> input)
        {
            return AnyListClosing().EnumerateMatches(input);
        }
    }
}