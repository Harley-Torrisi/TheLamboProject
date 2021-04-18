using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TheLamboProject.Data.DataBases.DataWharehouse;
using Newtonsoft.Json.Converters;
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace TheLamboProject.Data.Services
{
    public class CoinspotService
    {
        readonly DbContextOptionsBuilder<DataWharehouseCXT> dataWharehouseBuilder;

        public CoinspotService()
        {
            dataWharehouseBuilder = new DbContextOptionsBuilder<DataWharehouseCXT>();
            string dir = $@"Data Source={AppDomain.CurrentDomain.BaseDirectory}\Data\DataBases\DataWharehouse\DataWharehouse.db";
            dataWharehouseBuilder.UseSqlite(dir);
        }

        public void CacheCurrentPrices()
        {
            try
            {
                using var client = new System.Net.WebClient();
                string response = client.DownloadString("https://www.coinspot.com.au/pubapi/latest");
                JObject results = JsonConvert.DeserializeObject<JObject>(response);

                if (results["status"].ToString() == "ok")
                {
                    var dateNow = DateTime.Now;
                    long currentDate = long.Parse(dateNow.ToString("yyyyMMdd"));
                    long currentTime = dateNow.Second + (dateNow.Minute * 60) + (dateNow.Hour * 60 * 60);

                    var coins = results["prices"];

                    using (DataWharehouseCXT db = new(dataWharehouseBuilder.Options))
                    {
                        foreach (var coin in coins)
                        {
                            db.CoinspotPrices.Add(new DataBases.DataWharehouse.Models.CoinspotPrice
                            {
                                CurrencyCode = ((JProperty)coin).Name,
                                PriceBid = double.TryParse(coin.ElementAt(0).ElementAt(0).First().Value<string>(), out double bid) ? bid : 0,
                                PriceAsk = double.TryParse(coin.ElementAt(0).ElementAt(1).First().Value<string>(), out double ask) ? ask : 0,
                                PriceLatest = double.TryParse(coin.ElementAt(0).ElementAt(2).First().Value<string>(), out double lastest) ? lastest : 0,
                                CurrentDate = currentDate,
                                CurrentTime = currentTime,
                            });
                        }

                        db.SaveChanges();
                    }
                }
            }
            catch(Exception e) { 
            
            }
        }
    }
}
