using System.Reflection;

namespace SharpLab;

public static class CoupleService
{
    private static readonly Random Rng = new();

    public static bool RandomLike(double probability) => Rng.NextDouble() < probability;
    
    public static IHasName? Couple(Human first, Human second,
        out bool firstLikes, out bool secondLikes)
    {
        if (first.Gender == second.Gender)
            throw new SameGenderException(first, second);

        CoupleAttribute? firstAttr = FindAttr(first.GetType(), second.GetType().Name);
        firstLikes = firstAttr != null && RandomLike(firstAttr.Probability);

        CoupleAttribute? secondAttr = FindAttr(second.GetType(), first.GetType().Name);
        secondLikes = secondAttr != null && RandomLike(secondAttr.Probability);

        if (!firstLikes || !secondLikes)
            return null;

        Human male = first.Gender == Gender.Male ? first : second;
        CoupleAttribute? maleAttr = male == first ? firstAttr : secondAttr;
        if (maleAttr == null || string.IsNullOrEmpty(maleAttr.ChildType))
            return null;
        
        MethodInfo? nameMethod = second.GetType()
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => !m.IsSpecialName && m.DeclaringType != typeof(object))
            .FirstOrDefault(m => m.ReturnType == typeof(string) && m.GetParameters().Length == 0);

        string childName = "Безіменний";
        if (nameMethod != null)
        {
            try   { childName = (string)nameMethod.Invoke(second, null)!; }
            catch { childName = "Безіменний"; }
        }

        Type? childType = Assembly.GetExecutingAssembly()
            .GetTypes()
            .FirstOrDefault(t => t.Name == maleAttr.ChildType);
        if (childType == null) return null;

        var child = (IHasName)Activator.CreateInstance(childType)!;
        childType.GetProperty("Name")?.SetValue(child, childName);

        PropertyInfo? patronymicProp = childType.GetProperty("Patronymic");
        if (patronymicProp != null && child is Human childHuman)
        {
            string suffix = childHuman.Gender == Gender.Female ? "івна" : "ович";
            patronymicProp.SetValue(child, male.Name + suffix);
        }

        return child;
    }

    private static CoupleAttribute? FindAttr(Type type, string pairName)
    {
        var enumerator = new CoupleAttributeEnumerator(type);
        while (enumerator.MoveNext())
            if (enumerator.Current.Pair == pairName)
                return enumerator.Current;
        return null;
    }
}
