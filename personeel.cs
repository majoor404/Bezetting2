﻿using System;

namespace Bezetting2
{
    [Serializable]
    public class personeel
    {
        public string _achternaam { get; set; }
        public string _voornaam { get; set; }
        public string _persnummer { get; set; }
        public string _adres { get; set; }
        public string _postcode { get; set; }
        public string _woonplaats { get; set; }
        public string _telthuis { get; set; }
        public string _tel06prive { get; set; }
        public string _telwerk { get; set; }
        public string _emailwerk { get; set; }
        public string _emailthuis { get; set; }
        public string _adrescodewerk { get; set; }
        public string _funtie { get; set; }
        public string _kleur { get; set; }
        public string _nieuwkleur { get; set; }
        public DateTime _verhuisdatum { get; set; }
        public string _tel06werk { get; set; }
        public string _werkplek { get; set; }
        public string _vuilwerk { get; set; }
        public string _passwoord { get; set; }
    }
}
