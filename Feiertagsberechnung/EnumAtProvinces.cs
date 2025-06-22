using System.ComponentModel;

namespace HqNotenverwaltung.Feiertagsberechnung
{
    public enum EnumAtProvinces
    {
        [Description("Wien")]               Vienna          = 1,
        [Description("Niederösterreich")]   LowerAustria    = 2,
        [Description("Oberösterreich")]     UpperAustria    = 3,
        [Description("Salzburg")]           Salzburg        = 4,
        [Description("Tirol")]              Tyrol           = 5,
        [Description("Vorarlberg")]         Vorarlberg      = 6,
        [Description("Burgenland")]         Burgenland      = 7,
        [Description("Steiermark")]         Styria          = 8,
        [Description("Kärnten")]            Carinthia       = 9,
        [Description("Gesamtösterreich")]   All             = 10,

    }
}
