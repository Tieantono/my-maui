using MyMaui.Models;
using MyMaui.Services;
using Z.Expressions;

namespace MyMaui;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        //EvaluateRequest();
    }

    string? translatedNumber;

    private void OnTranslate(object sender, EventArgs e)
    {
        var enteredNumber = PhoneNumberText.Text;
        translatedNumber = PhonewordTranslator.ToNumber(enteredNumber);

        if (!string.IsNullOrEmpty(translatedNumber))
        {
            CallButton.IsEnabled = true;
            CallButton.Text = $"Call {translatedNumber}";
        }
        else
        {
            CallButton.IsEnabled = false;
            CallButton.Text = "Call";
        }
    }

    async void OnCall(object sender, EventArgs e)
    {
        if (await this.DisplayAlert(
            "Dial a Number",
            $"Would you like to call {translatedNumber}?",
            "Yes",
            "No"))
        {
            try
            {
                if (PhoneDialer.Default.IsSupported && !string.IsNullOrWhiteSpace(translatedNumber))
                    PhoneDialer.Default.Open(translatedNumber);
            }
            catch (ArgumentNullException)
            {
                await DisplayAlert("Unable to make the call", "Phone number was not valid.", "OK");
            }
            catch (Exception)
            {
                await DisplayAlert("Unable to make the call", "Phone call failed.", "OK");
            }
        }
    }

    private void EvaluateRequest()
    {
        var list = new List<int>() { 1, 2, 3, 4 };
        var greaterThan = 2;

        list = Eval.Execute<List<int>>("list.Where(x => x > greaterThan)",
           new { list, greaterThan });

        var exp = @"(Convert.ToDateTime(TransDate) >= Convert.ToDateTime(""2024-10-21 07:26:00"")) 
        && (Convert.ToDateTime(""2025-10-21 07:24:00"") >= Convert.ToDateTime(TransDate)) 
        && (Convert.ToDecimal(ItemProduct
            .Sum(y => Convert.ToDecimal(y.Price) * Convert.ToDecimal(y.Qty))) >= Convert.ToDecimal(""100000"")) 
            && (ZoneCode == ""Zone1"" ) 
            && (SiteCode == ""Site1"" ) 
        && (PromoAppCode == ""Promo1"") 
        && ((Mop.FirstOrDefault(q => q.MopCode == ""Mop1"").Amount) >= 100000) ";

        var req = new PromoRequest
        {
            TransDate = "2025-01-02 12:00:00",
            ZoneCode = "Zone1",
            SiteCode = "Site1",
            PromoAppCode = "Promo1",
            ItemProduct = [
                new ItemProduct{
                    Price = 50_000,
                    Qty = 1
                },
                new ItemProduct{
                    Price = 50_000,
                    Qty = 2
                }
            ],
            Mop = [
                new Mop {
                   Amount = 100_000,
                   MopCode = "Mop1"
                }
            ]
        };

        EvalContext context = new();
        var isValid = context.Execute<bool>(exp, req);

        var itemList = list.Select(Q => Q.ToString()).ToList();

        if (isValid)
        {
            //TestLabel.Text = "Eval is true";
            //itemList.Add(TestLabel.Text);
        }
        else
        {
            //TestLabel.Text = "Eval is false";
            //itemList.Add(TestLabel.Text);
        }
    }

}
