using MatrisSVICS.Calendar.Model;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrisSVICS.Calendar.ViewModel
{
    public class EventListDatabaseContext : DataContext
    {
        public static string dbConnectionString = "Data Source=isostore:/MATRISEvents.sdf";

        public EventListDatabaseContext(string connectionString) : base(connectionString) { }

        public Table<EventModel> Events;
    }
}
