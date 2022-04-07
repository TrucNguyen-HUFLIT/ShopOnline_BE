using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShopOnline.Core.Helpers
{
    public static class ConvertCurrencyHelper
    {
        private static DateTime LastUpdated { get; set; }
        private static double ExchangeRate { get; set; }
        private static int MaxTimeToReload { get; set; }

        public static async Task<int> ConvertVNDToUSD(double priceVND)
        {
            await ConvertVNDToUSD();
            return (int)Math.Round(priceVND / ExchangeRate);
        }

        private static async Task ConvertVNDToUSD()
        {
            if (LastUpdated.Date != DateTime.Now.Date)
            {
                if (MaxTimeToReload <= 10 && MaxTimeToReload != 0)
                {
                    MaxTimeToReload++;
                    return;
                }
                try
                {
                    //way 1:
                    //const string API_KEY = "9Ntk9X3TsMEeF2jxW6zJrEj5Tk6xNh";
                    //string URL = $"https://www.amdoren.com/api/currency.php?api_key={API_KEY}&from=USD&to=VND";
                    //way 2:
                    //const string API_KEY = "af8cf5a0-5b29-11ec-a3b6-e13601d8bd47";
                    //string URL = $"https://freecurrencyapi.net/api/v2/latest?apikey={API_KEY}";
                    //way 3:
                    const string API_KEY = "90a73b2366918861daa931fa";
                    string URL = $"https://v6.exchangerate-api.com/v6/{API_KEY}/latest/USD";

                    HttpClient httpClient = new();
                    HttpResponseMessage httpResponse = await httpClient.GetAsync(URL);
                    var content = await httpResponse.Content.ReadAsStringAsync();

                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        MaxTimeToReload = MaxTimeToReload == 10 ? 0 : MaxTimeToReload++;
                        Console.WriteLine(content);
                        if (ExchangeRate == default)
                        {
                            ExchangeRate = 22500;
                        }
                    }
                    else
                    {
                        MaxTimeToReload = 0;
                        var result = JsonConvert.DeserializeObject<ResultResponse>(content);
                        //way 1:
                        //ExchangeRate = result.Amount;
                        //way 2:
                        //ExchangeRate = result.Data.VND;
                        //way 3:
                        ExchangeRate = result.Conversion_Rates.VND;

                        LastUpdated = DateTime.Now;
                    }
                }
                catch (Exception e)
                {
                    MaxTimeToReload = MaxTimeToReload == 10 ? 0 : MaxTimeToReload++;
                    Console.WriteLine(e);
                    if (ExchangeRate == default)
                    {
                        ExchangeRate = 22500;
                    }
                }
            }
        }
    }

    class ResultResponse
    {
        //way 1
        //public double Amount { get; set; }
        //way 2
        //public ConvertCurrencyResponse Data { get; set; }
        //way 3
        public ConvertCurrencyResponse Conversion_Rates { get; set; }
    }

    class ConvertCurrencyResponse
    {
        public double VND { get; set; }
    }
}
