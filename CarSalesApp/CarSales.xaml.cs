using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


//Assignment CarSales3 ITWorks Jinghua Zhong  30/11/2020

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CarSalesApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CarSales : Page
    {
        private double vehicleWarranty;
        private double optionalExtras;
        private double accidentInsurance;

        public CarSales()
        {
            this.InitializeComponent();
        }


        //Arrays

        String[] names = new string[10];
        int[] phoneNums = new int[10];
        String[] vehicleMakes = new string[8];



        private async void SaveBottonTextBlock_Click(object sender, RoutedEventArgs e)
        {
            //input the customerName and cannot be left blank
            //if left blank, display error message and let enter the name again

            if (customerNameTextBox.Text == "")
            {
                var dialogMessage = new MessageDialog("Please enter a CustomerName.");//create an output message
                //error message
                await dialogMessage.ShowAsync(); //show the error message
                customerNameTextBox.Focus(FocusState.Programmatic);//set the foucs on the textbox
                customerNameTextBox.SelectAll();//Select all of the text
                return;//statement to return back to the xaml page
            }
            //input the customerPhone and cannot be left blank
            //if left blank, display error message and let enter the name again
            if (customerPhoneTextBox.Text == "")
            {
                var dialogMessage = new MessageDialog("Please enter a customerPhone Number.");//create an output message
                //error message
                await dialogMessage.ShowAsync(); //show the error message
                customerPhoneTextBox.Focus(FocusState.Programmatic);//set the foucs on the textbox
                customerPhoneTextBox.SelectAll();//select all of the text
                return;//statement to return back to the xaml page
            }

            customerNameTextBox.IsEnabled = false; //display the name TextBox 
            customerPhoneTextBox.IsEnabled = false;//display the phone TextBox
            vehiclePriceTextBox.Focus(FocusState.Programmatic);

        }

        private void ResetBottonTextBlock_Click(object sender, RoutedEventArgs e)
        //clear all fields and return focus to the Customer Name field
        {
            customerNameTextBox.Text = "";
            customerPhoneTextBox.Text = "";
            vehiclePriceTextBox.Text = "";
            lessTradeInTextBox.Text = "";
            subAmountTextBox.Text = "";
            gstAmountTextBox.Text = "";
            finalAmountTextBox.Text = "";

            warrantyComboBox.SelectedIndex = -1;
            tintingCheckBox.IsChecked = false;
            protectionCheckBox.IsChecked = false;
            gpsCheckBox.IsChecked = false;
            soundCheckBox.IsChecked = false;
            // toggle switch is turned off
            yesnoToggleSwitch.IsOn = false;
            //accident insurance is cleared
            under25RadioButton.IsChecked = false;
            over25RadioButton.IsChecked = false;

            calendarDatePicker.Date = null;
            pickupTimePicker.SelectedTime = null;

            customerPhoneTextBox.IsEnabled = true;
            customerNameTextBox.IsEnabled = true;
            customerNameTextBox.Focus(FocusState.Programmatic);

            summaryTextBox.Text = "";

        }

        private void SetDateTime(DateTimeOffset dateTimeOffset)
        {
            throw new NotImplementedException();
        }

        private async void SummaryBottonTextBlock_Click(object sender, RoutedEventArgs e)
        {
            //declaration constants
            const double GST_RATE = 0.1;
            //declaration variable
            double vehiclePrice;
            double lessTradeIn;
            double subAmount;
            double gstAmount;
            double finalAmount;



            //try test code for inputing vehicle price
            try
            {
                vehiclePrice = double.Parse(vehiclePriceTextBox.Text);
            }
            //catch statement will run after try blcok finds any error
            catch (Exception theException)
            {
                var dialogMessage = new MessageDialog("Error! Please enter a numeric value for vehicle price." + theException.Message);
                await dialogMessage.ShowAsync(); //show the error message
                vehiclePriceTextBox.Focus(FocusState.Programmatic);//set the foucs on the textbox
                vehiclePriceTextBox.SelectAll();//select all of the text
                return;
            }


            //vehicle Price must greater than 0, if not, get error message and back to textbox, need to enter again 
            if (vehiclePrice < 0)
            {
                var dialogMessage = new MessageDialog("Error! Please enter Vehicle price greater than 0");
                await dialogMessage.ShowAsync();//show the error message
                vehiclePriceTextBox.Focus(FocusState.Programmatic);//set the focus on the textbox
                vehiclePriceTextBox.SelectAll();//select all of the text
                return;
            }
            string lti = lessTradeInTextBox.Text; //if the lessTradeInTextBox is empty then set it to zero
            int a = 0;
            if (lti == "")
            {
                lti = lessTradeInTextBox.Text = a.ToString();
            }
            //try test for inputing lessTradeIn
            try
            {
                lessTradeIn = double.Parse(lessTradeInTextBox.Text);
            }
            //catch statement will run after try blcok finds any error
            catch (Exception theException)
            {
                var dialogMessage = new MessageDialog("Error! Please enter a numeric value for lessTradeIn price." + theException.Message);

                await dialogMessage.ShowAsync();//show the error message
                lessTradeInTextBox.Focus(FocusState.Programmatic);//set the focucs on the textbox
                lessTradeInTextBox.SelectAll();//select all of the text
                return;
            }


            //lessTradeIn must greater than or equal to 0, if not, get error message and back to textbox, need to enter again
            if (lessTradeIn < 0)
            {
                var dialogMessage = new MessageDialog("Error! Please enter trade in price greater than or equal to 0");
                await dialogMessage.ShowAsync();//show the error message
                vehiclePriceTextBox.Focus(FocusState.Programmatic);//set the focus on the textbox
                vehiclePriceTextBox.SelectAll();//select all of the text
                return;
            }


            //vehicle price must greater than tradeIn price, if not, get error message and back to textbox, need to enter again
            if (lessTradeIn >= vehiclePrice)
            {
                var dialogMessage = new MessageDialog("Error! Please enter tradeIn price less than vehiclePrice");
                await dialogMessage.ShowAsync();//show the error message
                lessTradeInTextBox.Focus(FocusState.Programmatic);//set the focus on the textbox
                lessTradeInTextBox.SelectAll();//select all of the text
                return;
            }


            //calculation
            vehicleWarranty = calcVehicleWarranty(vehiclePrice);
            optionalExtras = calcOptionalExtras();
            accidentInsurance = calcAccidentInsurance(vehiclePrice, optionalExtras);

            subAmount = vehiclePrice + vehicleWarranty + optionalExtras + accidentInsurance - lessTradeIn;//sumAmount calculation method
            subAmountTextBox.Text = subAmount.ToString("C");//output the subAmount to the subAmountTextBlock as a string(text)


            gstAmount = subAmount * GST_RATE;// gstAmount calculation method
            gstAmountTextBox.Text = gstAmount.ToString("C");//output the gstAmount to the gstAmountTextBlock as a string(text)

            finalAmount = subAmount + gstAmount;//finalAmont calculation method
            finalAmountTextBox.Text = finalAmount.ToString("C");//output the finalAmount to the finalAmountTextBlock as a string(text)

            //when the summary button is pressed, display full details of a purchase
            summaryTextBox.Text = ("Summary details " + "\r\n" + "\r\n" +
                                   "CustomerName: " + customerNameTextBox.Text + "\r\n" +
                                   "CustomerPhone: " + customerPhoneTextBox.Text + "\r\n" + "\r\n" +
                                   "VehiclePrice: " + vehiclePriceTextBox.Text + "\r\n" +
                                   "TradeInPrice: " + lessTradeInTextBox.Text + "\r\n" + "\r\n" +
                                   "VehicleWarranty: " + vehicleWarranty + "\r\n" +
                                   "OptionalExtras: " + optionalExtras + "\r\n" +
                                   "AccidentInsurance: " + accidentInsurance + "\r\n" + "\r\n" +
                                   "SubAmount: " + subAmount + "\r\n" +
                                   "GSTAmount: " + gstAmount + "\r\n" +
                                   "FinalAmount: " + finalAmount);
        }



        //calc vehicleWarranty
        private double calcVehicleWarranty(double vehiclePrice)
        {
            //declaration constants
            //const double WARRANTY_RATE_1 = 0;
            const double WARRANTY_RATE_2 = 0.05;
            const double WARRANTY_RATE_3 = 0.1;
            const double WARRANTY_RATE_4 = 0.2;
            double warranty = 0.0;

            //if (warrantyComboBox.SelectedIndex == 0)
            //{
            //    warranty = vehiclePrice * WARRANTY_RATE_1;
            //}
            if (warrantyComboBox.SelectedIndex == 1)
            {
                warranty = vehiclePrice * WARRANTY_RATE_2;
            }
            else if (warrantyComboBox.SelectedIndex == 2)
            {
                warranty = vehiclePrice * WARRANTY_RATE_3;
            }
            else if (warrantyComboBox.SelectedIndex == 3)
            {
                warranty = vehiclePrice * WARRANTY_RATE_4;
            }
            else
            {
                warranty = 0.0;
            }


            return warranty;
        }
        //ToggleSwitch
        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (yesnoToggleSwitch.IsOn == true)
            {
                under25RadioButton.Visibility = Visibility.Visible;
                under25RadioButton.IsChecked = true;
                over25RadioButton.IsEnabled = true;
                under25RadioButton.IsChecked = true;
            }
            else
            {
                under25RadioButton.Visibility = Visibility.Collapsed;
                under25RadioButton.IsChecked = false;
                over25RadioButton.IsEnabled = false;
                under25RadioButton.IsChecked = false;
            }

        }



        //calc OptionalExtras

        private double calcOptionalExtras()
        {

            //Declaring constants
            const double WINDOW_TINTING = 150;
            const double DUCO_PROTECTION = 180;
            const double GPS_NAVIGATION = 320;
            const double DELUXE_SOUND = 350;

            double total = 0;

            if (tintingCheckBox.IsChecked == true)
            {
                total = total + WINDOW_TINTING;
            }
            if (protectionCheckBox.IsChecked == true)
            {
                total = total + DUCO_PROTECTION;
            }
            if (gpsCheckBox.IsChecked == true)
            {
                total = total + GPS_NAVIGATION;
            }
            if (soundCheckBox.IsChecked == true)
            {
                total = total + DELUXE_SOUND;
            }
            return total;
        }

        //calc AccidentInsurance

        private double calcAccidentInsurance(double vehiclePrice, double optionalExtras)
        {
            const double INSURANCE_RATE_1 = 0.2;
            const double INSURANCE_RATE_2 = 0.1;
            double insurance = 0.0;
            if (under25RadioButton.IsChecked == true)
            {
                insurance = (vehiclePrice + optionalExtras) * INSURANCE_RATE_1;
            }

            else if (over25RadioButton.IsChecked == true)
            {
                insurance = (vehiclePrice + optionalExtras) * INSURANCE_RATE_2;
            }

            return insurance;
        }

        //Part3

        private void page_loaded(object sender, RoutedEventArgs e)
        {
            names = new string[] { "Ann", "Jane", "Liam", "Noah", "William", "James", "Logan", "Benjamin", "Oliver", "Elijah" };
            phoneNums = new int[] { 0420000111, 0420333111, 0420000222, 0420000333, 0410111000, 0410222333, 0411000111, 0412000222, 0415000555, 0421000012 };
            vehicleMakes = new string[] { "Toyota", "Holden", "Mitsubishi", "Ford", "BMW", "Mazda", "Volkswagen", "Mini" };

        }

        //display all customer
        private void displayAllCustomers_Click(object sender, RoutedEventArgs e)
        {

            string output = "";
            for (int index = 0; index < names.Length; index++)//loop through the array
            {
                output = output + names[index] + ",  " + phoneNums[index].ToString() + "\n";
            }
            summaryTextBox.Text = "Display All customers Array elements: \n" + output;  //output the list

        }
        // search name
        //the search method requires a name to search for and will return the index at which
        //the name is found, -1 if not found.

        private async void searchNameArrayButton_Click(object sender, RoutedEventArgs e)
        {
            int counter = 0;  // to track name in array 
            bool found = false; //found will be true or false depending if name found
            string criteria = customerNameTextBox.Text.ToUpper();//input search criteria from use and 
                                                                 // convert to uppercase to make checking a string easier
            if (customerNameTextBox.Text == "") //check if customerName textbox empty
            {
                var dialogMessage = new MessageDialog("Please enter a name into the customerName box.");
                await dialogMessage.ShowAsync();
                customerNameTextBox.Focus(FocusState.Programmatic);
                return;
            }
            if (customerNameTextBox.Text == "") // check if search textbox empty
            {
                var dialogMessage = new MessageDialog("Please enter a name into the customerNameTextBox.");
                await dialogMessage.ShowAsync();
                customerNameTextBox.Focus(FocusState.Programmatic);
                return;
            }

            // do sequential search of string array until match found or end of array reached
            while (!found && counter < names.Length)
            { // while not found and not end of array
                if (criteria == names[counter].ToUpper())// check if the name is found
                    found = true;
                else
                    counter++; // if no match move to next element in array
            } // end while loop

            if (counter < names.Length)  // if a name has been found
            {
                customerPhoneTextBox.Text = phoneNums[counter].ToString();

            }
            else // not found
            {
                summaryTextBox.Text = criteria + ":  " + "Not found";
                
            }


        }
        // delete name
        private async void deleteName_Click(object sender, RoutedEventArgs e)
        {
            int counter = 0; // used to keep track of name in array
            bool found = false; // boolean to keep track if name found
            if (customerNameTextBox.Text == "") //check if search textbox empty
            {
                var dialogMessage = new MessageDialog("Please enter a Name");
                await dialogMessage.ShowAsync();
                customerNameTextBox.Focus(FocusState.Programmatic);
                return;
            }
            string criteria = customerNameTextBox.Text.ToUpper(); //input search criteria from user
            // do sequential search of string array until match found or end of array reached
            while (!found && counter < names.Length)
            {
                if (criteria == names[counter].ToUpper())
                    found = true;
                else
                    counter++;
            }
            if (counter < names.Length) // if found the name exists, delete from the array by replacing the one to delete with the next name etc
            {
                for (int i = counter; i < names.Length - 1; i++)
                {
                    names[i] = names[i + 1]; // copy the next item in the array to the previous position
                }
                Array.Resize(ref names, names.Length - 1);  //RESIZE name array by -1
                Array.Resize(ref phoneNums, phoneNums.Length - 1);
                displayAllCustomers_Click(sender, e);
                var dialogMessage = new MessageDialog("Customer: " + criteria + ", Phone number: " + phoneNums[counter] + " has been deleted.\n" + "Updated to length." + names.Length);
                await dialogMessage.ShowAsync();
            }
            else
            {
                summaryTextBox.Text = criteria + ":  " + " DOES NOT EXIST to delete";
            }
        }
        private void displayAllMakes_Click(object sender, RoutedEventArgs e)
        {
            Array.Sort(vehicleMakes);
            string output = "";
            for (int index = 0; index < vehicleMakes.Length; index++)//loop through the array
            {
                output = output + vehicleMakes[index] + "\n";
            }
            summaryTextBox.Text = "All makes:" + "\n" + output; // output the list

        }


        //binary search is half-interval search, logarithmic search, or binary chop
        //binary search compares the target value to the middle element of the array.
        //Firstly, rewrite the binary array search method "binaryArraySearch",
        //then code the binarySearch_Click.
        private static int binaryArraySearch(string[] vehicleMakes, string item)
        {
            int min = 0; 
            int max = vehicleMakes.Length - 1;
            int mid;
            item = item.ToUpper();
            do
            {
                mid = (min + max) / 2;
                if (vehicleMakes[mid].ToUpper() == item)    //if the item is found return the index mid
                    //or use if (item.CompareTo(data[mid].ToUpper()) == 0)
                return mid;

                if (item.CompareTo(vehicleMakes[mid].ToUpper()) > 0)  //check if the item wanted is in the top half of the search

                    min = mid + 1;    //set the minpart of the search to the mid +1

                else

                    max = mid - 1;    //otherwise the item must be in the lower half of the search, set max to the mid -1
            }
            while (min <= max);
            {
                return -1;          // -1 is returned when not found
            }
        }

        private async void binarySearch_Click_1(object sender, RoutedEventArgs e)
        {
            string criteria;
            criteria = binarySearchTextBox.Text.ToUpper();
            Array.Sort(vehicleMakes);    //make sure the data is sorted
            displayAllMakes_Click(sender, e);    //display the allMakes in ascending order
            int foundPos = binaryArraySearch(vehicleMakes, criteria);    //call the binaryArraySearch method
           
            if (binarySearchTextBox.Text == "") // check if search textbox empty
            {
                var dialogMessage = new MessageDialog("Please enter a make into the binarySearchTextBox");
                await dialogMessage.ShowAsync();
                binarySearchTextBox.Focus(FocusState.Programmatic);
                return;
            }

            if (foundPos == -1)
            {
                var dialogMessage = new MessageDialog(criteria + " not found");
                await dialogMessage.ShowAsync();
            }
            else
            {
                var dialogMessage = new MessageDialog(criteria + "  found at index " + foundPos + " VehicleMakes array");
                await dialogMessage.ShowAsync();
            }
      
            //    int counter = 0;  // to track name in array 
            //    bool found = false; //found will be true or false depending if make found
            //    string criteria = binarySearchTextBox.Text.ToUpper();//input search criteria from use and 
            //                                                         // convert to uppercase to make checking a string easier
            //    if (binarySearchTextBox.Text == "") // check if search textbox empty
            //    {
            //        var dialogMessage = new MessageDialog("Please enter a make into the searchTextBox");
            //        await dialogMessage.ShowAsync();
            //        customerNameTextBox.Focus(FocusState.Programmatic);
            //        return;
            //    }


            //    while (!found && counter < vehicleMakes.Length)
            //    { // while not found and not end of array
            //        if (criteria == vehicleMakes[counter].ToUpper())// check if the make is found
            //            found = true;
            //        else
            //            counter++; // if no match move to next element in array
            //    } // end while loop

            //    if (counter < vehicleMakes.Length)  // if a make has been found
            //    {
            //        summaryTextBox.Text = vehicleMakes[counter] + ":  " + "Found";
            //    }
            //    else // not found
            //    {
            //        summaryTextBox.Text = criteria + ":  " +  "Not found";
            //    }
            //}
        }

        //inser make
        private async void insertMake_Click_1(object sender, RoutedEventArgs e)
        {

            int counter = 0; // used to keep track of position in array
            bool found = false; // boolean to keep track if vichleMakes found
            if (insertTextBox.Text == "") //check if search textbox empty
            {
                var dialogMessage = new MessageDialog("Please enter a vehicle into the search box");
                await dialogMessage.ShowAsync();
                insertTextBox.Focus(FocusState.Programmatic);
                return;
            }

            string criteria = insertTextBox.Text.ToUpper(); // input search criteria from user
                                                            // do sequential search of string array until match found or end of array reached
            while (!found && counter < vehicleMakes.Length) // while not found and not end of array
            {
                if (criteria == vehicleMakes[counter].ToUpper())
                    found = true;
                else
                    counter++; // if no match move to next element in array
            }
            if (counter < vehicleMakes.Length)    // if found the vehicle make already exists, do not add to array
            {
                summaryTextBox.Text = criteria + " ALREADY EXISTS";
            }
            else
            {
                Array.Resize(ref vehicleMakes, vehicleMakes.Length + 1);            // RESIZE city Array by 1
                var dialogMessage = new MessageDialog(criteria + " added, vehicleMakes array now updated to length " + names.Length + ".");
                await dialogMessage.ShowAsync();
                vehicleMakes[vehicleMakes.Length - 1] = insertTextBox.Text;  // assign the city entered to the last element in the array
                displayAllMakes_Click(sender, e);
            }
        }
    }
}

    



      
    






