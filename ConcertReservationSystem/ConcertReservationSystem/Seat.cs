using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConcertReservationSystem 
{
    public class Seat : INotifyPropertyChanged
    {
        public int seatNumber { get; set; }
        public string seatCustomerName { get; set; }
        public bool isReserved { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
