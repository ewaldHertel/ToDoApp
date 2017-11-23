using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalender
{
    class Termine
    {
        public int Id { get; set; }
        public string Bezeichnung { get; set; }
        public DateTime Von { get; set; }
        public DateTime Bis { get; set; }

        public Termine(int id, string bez, DateTime von, DateTime bis)
        {
            this.Id = id;
            this.Bezeichnung = bez;
            this.Von = von;
            this.Bis = bis;
        }
    }
}
