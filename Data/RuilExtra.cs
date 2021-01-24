using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bezetting2.Data
{
    [Serializable]
    public class RuilExtra
    {
        public string _naamAanvraagDoor { get; set; }
        public string _naamAanvraagVoor { get; set; }
        public string _ploeg { get; set; }
        public DateTime _datum { get; set; }
        public string _dienst { get; set; }
        public string _werkplek { get; set; }
        public string _persoonLoopt { get; set; }
        public  bool _extra { get; set; }  // false dan ruil
        public int _vanploeg { get; set; }

    }
}
