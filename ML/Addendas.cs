using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ML
{
    public class Addendas
    {
        public Guid IdAddenda { get; set; }

        public string NombreAddenda { get; set; }

        public string XML { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string Usuario { get; set; }

        public bool? Estado { get; set; }

        public List<object> ListAddendas { get; set; }

    }
}
