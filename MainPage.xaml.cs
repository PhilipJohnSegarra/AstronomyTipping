using Microsoft.Maui.Controls;
using System.Globalization;
namespace AstronomyTipping
{
    public partial class MainPage : ContentPage
    {
        int CurrentSliderValue, splitBy = 1;
        double Bill, TipPercentage;
        Receipt rcp;
        CultureInfo CultureInfo;
        public MainPage()
        {
            InitializeComponent();

            CurrentSliderValue = (int)tipSlider.Value;
            splitEntry.Text = splitBy.ToString();
            Bill = Convert.ToDouble(BillEntry.Text);
            TipPercentage = 0;
            CultureInfo = new CultureInfo("en-PH");

            rcp = new Receipt(Bill, TipPercentage, splitBy);
        }

        private async void tipSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            double targetX = ((tipSlider.Value / 50) * sliderContainer.Width);
            if((int)tipSlider.Value > CurrentSliderValue)
            {
                await Astronaut.ScaleXTo(1, 500);
                await Astronaut.TranslateTo(targetX - (Astronaut.Width), 0, 3000);
                CurrentSliderValue = (int)(tipSlider.Value);
            }
            else if((int)tipSlider.Value < CurrentSliderValue)
            {
                await Astronaut.ScaleXTo(-1, 500);
                await Astronaut.TranslateTo(targetX, 0, 3000);
                CurrentSliderValue = (int)(tipSlider.Value);
            }

            TipPercentage = (double)tipSlider.Value;
            rcp.TipPercentage = TipPercentage;
            UpdateObject();
            UpdateUI();

            lblCustomTip.Text = "Customize Tip:\n" + tipSlider.Value.ToString("F2") + "%";
        }

        private async void decrementBtn_Clicked(object sender, EventArgs e)
        {
            if(splitBy > 1)
            {
                splitBy--;
                splitEntry.Text = splitBy.ToString();
            }
            rcp.SplitBy = splitBy;
            UpdateObject();
            UpdateUI();

            await Moon.TranslateTo(130, 300, 500);
            await Moon.TranslateTo(0, 0, 0);
        }

        private async void tenPercentBtn_Clicked(object sender, EventArgs e)
        {
            TipPercentage = 10;
            rcp.TipPercentage = TipPercentage;
            tipSlider.Value = TipPercentage;
            UpdateObject();
            UpdateUI();

            await Sun.TranslateTo(-160, 120, 500);
            await Sun.TranslateTo(0, 0, 0);
            twentyPercentBtn.BackgroundColor = Color.FromArgb("#201F1F");
            tenPercentBtn.BackgroundColor = Color.FromArgb("#EEB685");
            fifteenPercentBtn.BackgroundColor = Color.FromArgb("#201F1F");
        }

        private async void fifteenPercentBtn_Clicked(object sender, EventArgs e)
        {
            TipPercentage = 15;
            rcp.TipPercentage = TipPercentage;
            tipSlider.Value = TipPercentage;
            UpdateObject();
            UpdateUI();

            await Sun.TranslateTo(-80, 120, 500);
            await Sun.TranslateTo(0, 0, 0);
            twentyPercentBtn.BackgroundColor = Color.FromArgb("#201F1F");
            tenPercentBtn.BackgroundColor = Color.FromArgb("#201F1F");
            fifteenPercentBtn.BackgroundColor = Color.FromArgb("#EEB685");
        }
        private async void twentyPercentBtn_Clicked(object sender, EventArgs e)
        {
            TipPercentage = 20;
            rcp.TipPercentage = TipPercentage;
            tipSlider.Value = TipPercentage;
            UpdateObject();
            UpdateUI();

            await Sun.TranslateTo(0, 120, 500);
            await Sun.TranslateTo(0, 0, 0);
            twentyPercentBtn.BackgroundColor = Color.FromArgb("#EEB685");
            tenPercentBtn.BackgroundColor = Color.FromArgb("#201F1F");
            fifteenPercentBtn.BackgroundColor = Color.FromArgb("#201F1F");
        }

        private void BillEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                BillEntry.Text = "0";
                return;
            }
            Bill = Convert.ToDouble(BillEntry.Text);
            rcp.Bill = Bill;
            UpdateObject();
            UpdateUI();
        }

        private async void incrementBtn_Clicked(object sender, EventArgs e)
        {
            splitBy++;
            splitEntry.Text = splitBy.ToString();
            rcp.SplitBy = splitBy;
            UpdateObject();
            UpdateUI();

            await Moon.TranslateTo(270, 300, 500);
            await Moon.TranslateTo(0, 0, 0);
        }

        private void UpdateUI()
        {
            
            IndividualAmount.Text = string.Format(CultureInfo, "₱{0:F2}", rcp.AmountPerPerson);
            TotalAmount.Text = string.Format(CultureInfo, "₱{0:F2}", rcp.Bill);
            TipAmount.Text = string.Format(CultureInfo, "₱{0:F2}", rcp.TipAmount);
        }
        private void UpdateObject()
        {
            rcp.TipCalculator(rcp.TipPercentage, rcp.Bill);
            rcp.AmountPerPersonCalcu(rcp.Bill, rcp.SplitBy);
        }
    }

    class Receipt
    {
        public double Bill { get; set; }
        public double TipPercentage { get; set; }
        public double TipAmount { get; set; }
        public int SplitBy { get; set; }
        public double AmountPerPerson { get; set; }
        public Receipt(double bill, double tipPercentage, int splitBy)
        {
            Bill = bill;
            TipPercentage = tipPercentage;
            SplitBy = splitBy;
        }

        public void TipCalculator(double tipPercentage, double bill)
        {
            double decimalTip = tipPercentage / 100;
            this.TipAmount = bill * decimalTip;
        }

        public void AmountPerPersonCalcu(double bill, int splitBy)
        {
            double TotalAmount = this.Bill + this.TipAmount;
            this.AmountPerPerson = (double)(TotalAmount / splitBy);
        }
    }
}
