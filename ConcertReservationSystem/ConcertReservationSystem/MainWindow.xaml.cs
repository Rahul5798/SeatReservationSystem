using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConcertReservationSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VM vm = new VM();
        private int totalReservedSeats; //to check if all seats are reserved or not
        private Button[] buttons; //Array of buttons
        private bool reserveButtonClicked = false;
        public MainWindow()
        {
            InitializeComponent();

            buttons = new Button[]{btnSeat1,btnSeat2,btnSeat3,btnSeat4,btnSeat5,btnSeat6,btnSeat7,btnSeat8,btnSeat9,btnSeat10,btnSeat11
                    ,btnSeat12,btnSeat13,btnSeat14,btnSeat15,btnSeat16};
        }


        private void seatSelector(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int index = Array.IndexOf(buttons, button);
            tbSeatNumber.Text = (index + 1).ToString();
        }


        private void reserveSeat(object sender, RoutedEventArgs e)
        {
            Seat[] seats = new Seat[16];
            for (int i = 0; i < 16; i++)
            {
                seats[i] = new Seat();
                seats[i].seatNumber = i + 1;
                seats[i].seatCustomerName = "";
                seats[i].isReserved = false;

            }
            if (reserveButtonClicked)
            {
                ArrayOfSeat seat = vm.ReadFromXml();
                seats = seat.seats.ToArray();
            }
            reserveButtonClicked = true;

            int seatNumber;
            //if all seats are reserved messagebox will show message
            if (totalReservedSeats != 16)
            {
                //checking if seat number and customer name is provided for reservation
                if (tbSeatNumber.Text != "" && tbCustomerName.Text != "")
                {
                    //if value of seat number is not number than messagebox will show message
                    try
                    {
                        seatNumber = int.Parse(tbSeatNumber.Text);
                        seats[seatNumber - 1].seatNumber = seatNumber;
                        seats[seatNumber - 1].seatCustomerName = tbCustomerName.Text;
                        //if seat is already reserved messagebox will show message
                        if (seats[seatNumber - 1].isReserved)
                        {
                            MessageBox.Show($"Seat {seats[seatNumber - 1].seatNumber} is already reserved");
                        }
                        else
                        {
                            //buttons[seatNumber - 1].Content = seats[seatNumber - 1].seatCustomerName;
                            //buttons[seatNumber - 1].Background = new SolidColorBrush(Color.FromRgb(66, 135, 245));
                            seats[seatNumber - 1].isReserved = true;
                            totalReservedSeats++;
                        }
                    }
                    catch (System.Exception)
                    {
                        MessageBox.Show("Enter seat number in correct format");
                    }

                }
                else
                {
                    MessageBox.Show("Enter seat number and Customer name for reservation");
                }
            }
            else
            {
                MessageBox.Show("All seats are reserved");
            }
            vm.WriteXmlSerializer(seats);
            readSeatArrangement();
        }
        public void readSeatArrangement()
        {
            ArrayOfSeat seat = vm.ReadFromXml();
            Seat[] seats = seat.seats.ToArray();
            for (int i = 0; i < seats.Length; i++)
            {
                if (seats[i].isReserved)
                {
                    buttons[seats[i].seatNumber - 1].Content = seats[seats[i].seatNumber - 1].seatCustomerName;
                    buttons[seats[i].seatNumber - 1].Background = new SolidColorBrush(Color.FromRgb(66, 135, 245));
                }
                else
                {
                    buttons[seats[i].seatNumber - 1].Content = "unreserved";
                    buttons[seats[i].seatNumber - 1].Background = new SolidColorBrush(Color.FromRgb(220, 220, 220));

                }
            }

        }
        private void cancelReservation(object sender, RoutedEventArgs e)
        {
            ArrayOfSeat seat = vm.ReadFromXml();
            Seat[] seats = seat.seats.ToArray();
            //if there is no seats reserved messagebox will show message
            if (totalReservedSeats > 0)
            {
                //if seat number is provided for canceling reservation
                if (tbSeatNumber.Text != "")
                {
                    //if value of seat number is not number than messagebox will show message
                    try
                    {
                        int seatNumber = int.Parse(tbSeatNumber.Text);

                        for (int i = 0; i < seats.Length; i++)
                        {
                            if (seats[i].seatNumber == seatNumber)
                            {
                                //if the seat is already empty messagebox will show message
                                if (seats[i].isReserved)
                                {
                                    //buttons[i].Content = "unreserved";
                                    //buttons[i].Background = new SolidColorBrush(Color.FromRgb(220, 220, 220));
                                    seats[i].isReserved = false;
                                    seats[i].seatCustomerName = "";
                                    totalReservedSeats--;
                                    tbCustomerName.Text = "";
                                    tbSeatNumber.Text = "";
                                }
                                else
                                {
                                    MessageBox.Show("Seat is already empty");
                                }
                            }

                        }
                    }
                    catch (System.Exception)
                    {
                        MessageBox.Show("Enter seat number in correct format");
                    }

                }
                else
                {
                    string name = tbCustomerName.Text;
                    bool multipleSeats = false; //to check if two seats are reserved with the same name
                    bool isSeatExistWithName = false; //to check if there is any seat is reserved with provided name
                                                      //checking for multiple seats with same name
                    for (int i = 0; i < seats.Length; i++)
                    {
                        for (int j = i + 1; j < seats.Length; j++)
                        {
                            if (seats[i].seatCustomerName == seats[j].seatCustomerName)
                            {
                                if (seats[i].seatCustomerName.ToLower() == name.ToLower())
                                {
                                    multipleSeats = true;

                                }
                            }
                        }
                    }
                    //Canceling the reservation using name
                    for (int i = 0; i < seats.Length; i++)
                    {
                        if (seats[i].seatCustomerName.ToLower() == name.ToLower())
                        {
                            if (multipleSeats)
                            {
                                MessageBox.Show("There are two seats with same name. Enter seat number");
                                isSeatExistWithName = true;
                                multipleSeats = false;
                                break;
                            }
                            else
                            {
                                buttons[i].Content = "unreserved";
                                buttons[i].Background = new SolidColorBrush(Color.FromRgb(220, 220, 220));
                                seats[i].isReserved = false;
                                seats[i].seatCustomerName = "";
                                totalReservedSeats--;
                                isSeatExistWithName = true;
                                tbCustomerName.Text = "";
                                tbSeatNumber.Text = "";
                            }
                        }
                    }
                    if (!isSeatExistWithName)
                    {
                        MessageBox.Show($"No seats are reserved with the name {name}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Every seat is empty");

            }
            vm.WriteXmlSerializer(seats);
            readSeatArrangement();
        }

        private void cancelAllReservations(object sender, RoutedEventArgs e)
        {
            ArrayOfSeat seat = vm.ReadFromXml();
            Seat[] seats = seat.seats.ToArray();
            if (totalReservedSeats > 0)
            {
                for (int i = 0; i < seats.Length; i++)
                {
                    if (seats[i].isReserved)
                    {
                        buttons[i].Content = "unreserved";
                        buttons[i].Background = new SolidColorBrush(Color.FromRgb(220, 220, 220));
                        seats[i].isReserved = false;
                        seats[i].seatCustomerName = "";
                        totalReservedSeats--;
                        tbCustomerName.Text = "";
                        tbSeatNumber.Text = "";
                    }
                }
            }
            else
            {
                MessageBox.Show("Every seat is empty");

            }
            vm.WriteXmlSerializer(seats);
            readSeatArrangement();
        }

        private void linqAction(object sender, RoutedEventArgs e)
        {
            ArrayOfSeat seat = vm.ReadFromXml();
            Seat[] seats = seat.seats.ToArray();
           
            //linqButtonNames 
            Button button = sender as Button;
            string[] buttons = { "btnLINQ1", "btnLINQ2", "btnLINQ3" };
            if(listBox.Items.Count != 0)
            {
                listBox.Items.Clear();
            }
            if (button.Name == buttons[0])
            {

                var SeatNames = seats.Where(x => x.isReserved == true).Select(x => x.seatCustomerName).OrderByDescending(x => x).ToList();//from seat in seats where seat.isReserved=true select seat.seatCustomerName;
                for (int i = 0; i < SeatNames.Count; i++)
                {
                    listBox.Items.Add(SeatNames[i]);
                }
            }
            else if (button.Name== buttons[1])
            {
                var SeatNames = seats.Where(x => x.isReserved == true).Select(x => x.seatCustomerName).OrderBy(x => x.Length).ToList();
                for (int i = 0; i < SeatNames.Count; i++)
                {
                    listBox.Items.Add(SeatNames[i]);
                }
            }
            else
            {
                var SeatNames = seats.Where(x => x.isReserved == false).Select(x => x.seatNumber).OrderBy(x => x).ToList();
                for (int i = 0; i < SeatNames.Count; i++)
                {
                    listBox.Items.Add(SeatNames[i]);
                }
            }
            
        }
    }
}

