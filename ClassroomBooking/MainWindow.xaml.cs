using ClassroomBooking.Models;
using ClassroomBooking.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ClassroomBooking
{
    public partial class MainWindow : Window
    {
        private readonly UserService userService = new UserService();
        private readonly RoomService roomService = new RoomService();
        private readonly BookingService bookingService = new BookingService();

        public MainWindow()
        {
            InitializeComponent();

            UserComboBox.ItemsSource = userService.GetUsers();
            RoomComboBox.ItemsSource = roomService.GetRooms();
            BookingDatePicker.SelectedDate = DateTime.Today;

            RefreshBookings();
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(UserComboBox.SelectedItem is User user))
            {
                StatusTextBlock.Text = "Выберите пользователя.";
                return;
            }

            if (!(RoomComboBox.SelectedItem is Room room))
            {
                StatusTextBlock.Text = "Выберите аудиторию.";
                return;
            }

            if (!(BookingDatePicker.SelectedDate is DateTime date))
            {
                StatusTextBlock.Text = "Выберите дату.";
                return;
            }

            if (!TimeSpan.TryParse(StartTimeTextBox.Text, out var startTime) ||
                !TimeSpan.TryParse(EndTimeTextBox.Text, out var endTime))
            {
                StatusTextBlock.Text = "Введите время в формате ЧЧ:ММ.";
                return;
            }

            if (startTime >= endTime)
            {
                StatusTextBlock.Text = "Время начала должно быть раньше времени окончания.";
                return;
            }

            var booking = new Booking
            {
                UserId = user.Id,
                RoomId = room.Id,
                Date = date,
                StartTime = startTime,
                EndTime = endTime
            };

            if (!bookingService.AddBooking(booking))
            {
                StatusTextBlock.Text = "На это время аудитория уже забронирована.";
                return;
            }

            StatusTextBlock.Text = "Бронирование создано.";
            RefreshBookings();
        }

        private void RefreshBookings()
        {
            var users = userService.GetUsers();
            var rooms = roomService.GetRooms();

            BookingsListBox.ItemsSource = bookingService.GetBookings()
                .Select(b =>
                {
                    var u = users.FirstOrDefault(x => x.Id == b.UserId);
                    var r = rooms.FirstOrDefault(x => x.Id == b.RoomId);
                    return $"{b.Date:dd.MM.yyyy} {b.StartTime:hh\\:mm}-{b.EndTime:hh\\:mm} | Ауд. {r?.Number} | {u?.Name}";
                })
                .ToList();
        }
    }
}