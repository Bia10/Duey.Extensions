using System.Text.RegularExpressions;

namespace Duey.Extensions;

public static class RegexPatterns
{
    public static readonly Regex AnyNpc = new("#p[0-9]+#");
    public static readonly Regex AnyMap = new("#m[0-9]+#");
    public static readonly Regex AnyMob = new("#o[0-9]+#");
    public static readonly Regex AnyItem = new("#t[0-9]+#");
}