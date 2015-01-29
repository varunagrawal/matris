using MatrisSVICS.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.UserData;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MatrisSVICS.ViewModels
{
    public class ContactsViewModel
    {
        public List<CustomContact> contact { get; set; }
        public List<string> phonenums;
     //   public ToDOMainViewModel todomain;

        public MobileServiceCollection<Installation, Installation> items;

        public IMobileServiceTable<Installation> todoTable = App.MobileService.GetTable<Installation>();
        public CustomContact customcont;

        public ContactsViewModel()
        {
            contact = new List<CustomContact>();
            customcont = new CustomContact();

        }

        public async Task<Int32> RefreshTodoItems()
        {
            phonenums = new List<string>();
            try
            {
                items = await todoTable.ToCollectionAsync();

                foreach (var item in items)
                {
                    phonenums.Add(item.PhoneNumber.ToString());
                }

            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e.Message);
                //   MessageBox.Show(e.Message, "Error loading items", MessageBoxButton.OK);
            }

            return 1;
        }
        public void waitForRefreshItems()
        {
            //Task<Int32> result = RefreshTodoItems();
            //int flag = await result;
            //if (flag == 1)
            //{
            //    contactMethod();
            //}
        }

        public void contactMethod()
        {
            Contacts cons = new Contacts();

            cons.SearchCompleted += new EventHandler<ContactsSearchEventArgs>(Contacts_SearchCompleted);
            cons.SearchAsync(String.Empty, FilterKind.None, "Contacts Test #1");
        }

        //conttacts

        void Contacts_SearchCompleted(object sender, ContactsSearchEventArgs e)
        {


            try
            {
                //Bind the results to the user interface.
                //ContactResultsData.DataContext = e.Results;
                contact = new List<CustomContact>();

                foreach (var con in e.Results)
                {
                    var num = con.PhoneNumbers.FirstOrDefault();
                    customcont.Number = num.ToString();
                    customcont.Name = con.DisplayName;
                }

                CompareWitCloudData(customcont, phonenums);

                PhoneApplicationService.Current.State["contactsModel"] = this;
                (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/Contacts.xaml", UriKind.Relative));


            }
            catch (System.Exception)
            {
                //No results
            }


        }
        public void CompareWitCloudData(CustomContact con, List<string> phonenums)
        {
            //    var result = Regex.Match(con.Number.ToLower(), @"\d+").Value;
            if (con.Number != null)
            {
                for (int k = 0; k < phonenums.Count; k++)
                {

                    var getNumbers = (from t in con.Number.ToLower()
                                      where char.IsDigit(t)
                                      select t).ToArray();
                    string st = new string(getNumbers);
                    //  string st = (con.Number.ToLower()).Substring(0, (con.Number.ToLower()).IndexOf('(') - 1);
                    if (st == (phonenums[k].ToLower()))
                    {
                        contact.Add(con);
                    }
                }
            }
        }

    }
}
