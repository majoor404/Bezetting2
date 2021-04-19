using System;

namespace Bezetting2.Data
{
    [Serializable]
    class VeranderingenVerhuis
    {
        public string Maand_ { get; set; }
        public string Jaar_ { get; set; }
        public string Personeel_nr { get; set; }
        public DateTime Datumafwijking_ { get; set; }
        public string Afwijking_ { get; set; }
        public DateTime Datuminvoer_ { get; set; }
        public string Invoerdoor_ { get; set; }
        public string Rede_ { get; set; }
        public string Kleur_ { get; set; }
    }
}
