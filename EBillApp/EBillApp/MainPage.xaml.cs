using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EBillApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //Get all records
            var recordsList = await App.SQLiteDb.DisplayAll();
            if (recordsList != null)
            {
                Records.ItemsSource = recordsList;
            }
        }

        private async void buttonAdd_Clicked(object sender, EventArgs e)
        {
            //int meterNum = Int32.Parse(MeterNum.Text);
            //double presentRead = Double.Parse(PresentRead.Text);
            //double previousRead = Double.Parse(PreviousRead.Text);
            //int.TryParse(MeterNum.Text, out int meterNum);
            double.TryParse(PresentRead.Text, out double presentRead);
            double.TryParse(PreviousRead.Text, out double previousRead);

            string typeOfRegis;
            double consumptionRead, electricityCharge, demandCharge, serviceCharge, principalAmount, vat, amountPayable;

            if (!string.IsNullOrEmpty(PresentRead.Text) && !string.IsNullOrEmpty(PreviousRead.Text))
            {
                consumptionRead = presentRead - previousRead;
                if (consumptionRead < 72)
                {
                    electricityCharge = 6.50;
                }
                else if (consumptionRead <= 150)
                {
                    electricityCharge = 9.50;
                }
                else if (consumptionRead <= 300)
                {
                    electricityCharge = 10.50;
                }
                else if (consumptionRead <= 400)
                {
                    electricityCharge = 12.50;
                }
                else if (consumptionRead <= 500)
                {
                    electricityCharge = 14.00;
                }
                else
                {
                    electricityCharge = 16.50;
                }

                if (RbtnH.IsChecked)
                {
                    typeOfRegis = "H";
                    demandCharge = 200;
                    serviceCharge = 50;
                }
                else if (RbtnB.IsChecked)
                {
                    typeOfRegis = "B";
                    demandCharge = 400;
                    serviceCharge = 100;
                }
                else
                {
                    await DisplayAlert("Required", "Invalid Type of Registration", "OK");
                    return;
                }

                principalAmount = electricityCharge + demandCharge + serviceCharge;
                vat = principalAmount * 0.05; // 5% of principalAmount
                amountPayable = principalAmount + vat;

                RECORDS records = new RECORDS()
                {
                    //METER_NUM = meterNum,
                    PRESENT_READ = presentRead,
                    PREVIOUS_READ = previousRead,
                    CONSUMPTION_READ = consumptionRead,
                    ELECTRICITY_CHARGE = electricityCharge,
                    TYPE_OF_REGIS = typeOfRegis,
                    DEMAND_CHARGE = demandCharge,
                    SERVICE_CHARGE = serviceCharge,
                    PRINCIPAL_AMOUNT = principalAmount,
                    AMOUNT_PAYABLE = amountPayable
                };

                // Adding
                await App.SQLiteDb.Save(records);
                MeterNum.Text = string.Empty;
                PresentRead.Text = string.Empty;
                PreviousRead.Text = string.Empty;
                RbtnH.IsChecked = false;
                RbtnB.IsChecked = false;
                await DisplayAlert("Success", "Added Successfully", "Ok");

                // Get all
                var recordsList = await App.SQLiteDb.DisplayAll();
                if (recordsList != null)
                {
                    Records.ItemsSource = recordsList;
                }
            }
            else
            {
                await DisplayAlert("Required", "Please Fill Present and Previous Reading", "OK");
            }
        }

        private async void buttonSearch_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MeterNum.Text))
            {
                if (int.TryParse(MeterNum.Text, out int meter_num))
                {
                    var meterRecord = await App.SQLiteDb.Search(meter_num);
                    if (meterRecord != null)
                    {
                        // Display the record details
                        MeterNum.Text = meterRecord.METER_NUM.ToString();
                        PresentRead.Text = meterRecord.PRESENT_READ.ToString();
                        PreviousRead.Text = meterRecord.PREVIOUS_READ.ToString();
                        //if (meterRecord.TYPE_OF_REGIS == "H")
                        //{
                        //    RbtnH = RbtnH.che;
                        //}

                        await DisplayAlert("Success",
                            "Meter No.: " + meterRecord.METER_NUM + "\n" +
                            "Principal Amount: " + meterRecord.PRINCIPAL_AMOUNT + "\n" +
                            "Amount Payable: " + meterRecord.AMOUNT_PAYABLE + "\n" +
                            "Type of Registration: " + meterRecord.TYPE_OF_REGIS,
                            "OK");
                    }
                    else
                    {
                        await DisplayAlert("Not Found", "Meter No. not found.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Invalid Input", "Meter No. must be a valid integer.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Required", "Please Enter Meter NO.", "OK");
            }
        }

        private async void buttonUpdate_Clicked(object sender, EventArgs e)
        {
            string typeOfRegis;
            double demandCharge, serviceCharge, principalAmount, vat, amountPayable;

            if (!string.IsNullOrEmpty(MeterNum.Text))
            {
                // meterNumValue;
                if (int.TryParse(MeterNum.Text, out int meterNumValue))
                {
                    // Retrieve existing record
                    var existingRecord = await App.SQLiteDb.Search(meterNumValue);
                    if (existingRecord != null)
                    {
                        // Update properties
                        double presentReadValue, previousReadValue;
                        if (double.TryParse(PresentRead.Text, out presentReadValue) &&
                            double.TryParse(PreviousRead.Text, out previousReadValue))
                        {
                            // Calculate other properties based on updated readings
                            var consumptionRead = presentReadValue - previousReadValue;
                            double electricityCharge;
                            if (consumptionRead < 72)
                            {
                                electricityCharge = 6.50;
                            }
                            else if (consumptionRead <= 150)
                            {
                                electricityCharge = 9.50;
                            }
                            else if (consumptionRead <= 300)
                            {
                                electricityCharge = 10.50;
                            }
                            else if (consumptionRead <= 400)
                            {
                                electricityCharge = 12.50;
                            }
                            else if (consumptionRead <= 500)
                            {
                                electricityCharge = 14.00;
                            }
                            else
                            {
                                electricityCharge = 16.50;
                            }

                            if (RbtnH.IsChecked)
                            {
                                typeOfRegis = "H";
                                demandCharge = 200;
                                serviceCharge = 50;
                            }
                            else if (RbtnB.IsChecked)
                            {
                                typeOfRegis = "B";
                                demandCharge = 400;
                                serviceCharge = 100;
                            }
                            else
                            {
                                await DisplayAlert("Required", "Invalid Type of Registration", "OK");
                                return;
                            }

                            principalAmount = electricityCharge + demandCharge + serviceCharge;
                            vat = principalAmount * 0.05; // 5% of principalAmount
                            amountPayable = principalAmount + vat;

                            // Update existing record
                            existingRecord.PRESENT_READ = presentReadValue;
                            existingRecord.PREVIOUS_READ = previousReadValue;
                            existingRecord.CONSUMPTION_READ = consumptionRead;
                            existingRecord.ELECTRICITY_CHARGE = electricityCharge;
                            existingRecord.TYPE_OF_REGIS = typeOfRegis;
                            existingRecord.DEMAND_CHARGE = demandCharge;
                            existingRecord.SERVICE_CHARGE = serviceCharge;
                            existingRecord.PRINCIPAL_AMOUNT = principalAmount;
                            existingRecord.AMOUNT_PAYABLE = amountPayable;

                            // Save changes
                            await App.SQLiteDb.Save(existingRecord);
                            await DisplayAlert("Success", "Record Updated Successfully", "OK");

                            // Refresh records list
                            var recordsList = await App.SQLiteDb.DisplayAll();
                            if (recordsList != null)
                            {
                                Records.ItemsSource = recordsList;
                            }

                            // Clear input fields
                            MeterNum.Text = string.Empty;
                            PresentRead.Text = string.Empty;
                            PreviousRead.Text = string.Empty;
                            RbtnH.IsChecked = false;
                            RbtnB.IsChecked = false;
                        }
                        else
                        {
                            await DisplayAlert("Invalid Input", "Present and Previous readings must be numeric.", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Not Found", "Record with Meter No. not found.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Invalid Input", "Meter No. must be numeric.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Required", "Please Enter Meter NO.", "OK");
            }
        }

        private async void buttonDelete_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MeterNum.Text))
            {
                //Get Person  
                var meterNumValue = await App.SQLiteDb.Search(Convert.ToInt32(MeterNum.Text));
                if (meterNumValue != null)
                {
                    //Delete Record  
                    await App.SQLiteDb.Delete(meterNumValue);
                    MeterNum.Text = string.Empty;
                    await DisplayAlert("Success", "Record Deleted", "OK");

                    //Get All Record  
                    var personList = await App.SQLiteDb.DisplayAll();
                    if (personList != null)
                    {
                        Records.ItemsSource = personList;
                    }
                }
            }
            else
            {
                await DisplayAlert("Required", "Please Enter PersonID", "OK");
            }
        }
    }
}
