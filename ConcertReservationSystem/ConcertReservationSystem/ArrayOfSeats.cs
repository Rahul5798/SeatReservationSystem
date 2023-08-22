using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConcertReservationSystem
{
    [XmlRoot("ArrayOfSeat")]
    public class ArrayOfSeat
    {
        [XmlElement("Seat")]
        public List<Seat> seats = new List<Seat>();
    }
}
