public class ModSInt : ModSeidToken
{
    public int Value { get; set; } = 0;

    public ModSInt()
    {
        PropertyType = ModSeidPropertyType.Int;
    }

    public ModSInt(int value) : this()
    {
        Value = value;
    }
}