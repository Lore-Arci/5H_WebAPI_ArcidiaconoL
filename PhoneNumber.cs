public class PhoneNumber
{
    public string CountryCode { get; set; }
    public string Number { get; set; }

    public PhoneNumber(string countryCode, string number)
    {
        CountryCode = countryCode;
        Number = number;
    }

    // During Serialization (PhoneNumber -> String)
    public override string ToString()
    {
        return $"+{CountryCode}-{Number}";
    }
    
    // Called for deserialization (String -> PhoneNumber)
    public static PhoneNumber Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;

        // Cause the number format is "CountryCode-Number"
        var parts = value.Split('-');
        if (parts.Length != 2)
        {
            throw new FormatException($"Invalid phone number format: {value}");
        }

        // CountryCode, Number
        return new PhoneNumber(parts[0], parts[1]);
    }
}