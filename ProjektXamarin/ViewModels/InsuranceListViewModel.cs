﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using ProjektXamarin.Models;
using ProjektXamarin.Views;
using Xamarin.Forms;

namespace ProjektXamarin.ViewModels
{
    public class InsuranceListViewModel : ViewModelBase
    {
        public ObservableCollection<Insurance> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command DeleteItemCommand { get; set; }
        public Customer customer;
        public InsuranceListViewModel()
        {
            Title = "Your insurance list";
            Items = new ObservableCollection<Insurance>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            MessagingCenter.Subscribe<NewInsurancePage, Insurance>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Insurance;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
            MessagingCenter.Subscribe<ProfilePageModel, Customer>(this, "ProfileUpdated", (obj, _cus)=>
            {
                customer = _cus;
            });
        
        }
        public bool IsCustomerAdult()
        {
            if (customer.Age < 18)
                return false;
            else return true;
        }
        public bool IsCustomerreplete()
        {
            if (
                customer.Adress != null &&
                customer.Education != null &&
                customer.FirstName != null &&
                customer.LastName != null &&
                customer.MarialStatus != null
                
                )
                return true;
            else return false;
                    
        }
        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
