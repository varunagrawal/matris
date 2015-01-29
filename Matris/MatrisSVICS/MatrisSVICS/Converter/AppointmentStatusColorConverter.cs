using MatrisSVICS.Calendar.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MatrisSVICS.Calendar.Converter
{
        public class AppointmentStatusColorConverter : System.Windows.Data.IValueConverter
        {
            public object Convert(
                object o,
                Type type,
                object parameter,
                System.Globalization.CultureInfo culture)
            {
                if (o == null)
                    return null;
                EventModel _event = (EventModel)o;

                return _event.IsAllottedAppointment ? new SolidColorBrush(Colors.Orange) : new SolidColorBrush(Colors.Red);
            }

            public object ConvertBack(
                object appointmentStatusBool,
                Type type,
                object parameter,
                System.Globalization.CultureInfo culture)
            {
                return null;
            }
        }
    }

