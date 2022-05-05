using Microsoft.AspNetCore.Components;
using QRCoder;
using RocketServe.Data;
using RocketServe.Data.Repositories.RestaurantRepository;
using RocketServe.Data.Repositories.TableRepository;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Pages.BussinessAdmin
{
    public class TablesListBase : ComponentBase
    {
        [Parameter]
        public string RestaurantId { get; set; }

        [Inject]
        IRestaurantRepository restaurantRepo { get; set; }

        [Inject]
        ITableRepository tableRepo { get; set; }


        //selected restaurant
        protected Restaurant restaurant;
        protected string qrUri;
        protected bool showQrPopup = false;
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            restaurant = await restaurantRepo.GetByIdAsync(RestaurantId);
            restaurant.Tables.Sort((t1,t2)=> String.Compare(t1.Name, t2.Name));
        }

        protected async Task AddTableAsync()
        {
            var lastTable = restaurant.Tables.LastOrDefault();
            int lastTableNumber = 0;
            if (lastTable != null)
                int.TryParse(lastTable.Name.Split(' ')[1], out lastTableNumber);

            await tableRepo.InsertAsync(new Table
            {
                IsWorking = true,
                Name = $"Table {++lastTableNumber}",
                Restaurant = restaurant
            });
        }
        protected async Task RemoveLastTableAsync()
        {
            var lastTable = restaurant.Tables.LastOrDefault();
            await tableRepo.RemoveAsync(lastTable);
        }
        protected async Task MarkTableAsWorking(Table table)
        {
            table.IsWorking = true;
            await tableRepo.SaveAsync(table);
        }
        protected async Task MarkTableAsUnWorking(Table table)
        {
            table.IsWorking = false;
            await tableRepo.SaveAsync(table);
        }
        protected void CloseQrPopup()
        {
            showQrPopup = false;
        }
        protected void GetTableQR(Table table)
        {
            showQrPopup = true;
            QRCodeGenerator QrGenerator = new QRCodeGenerator();
            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode($"http://e0be-78-109-68-216.ngrok.io/UserMenu/{table.Id}/", QRCodeGenerator.ECCLevel.Q);
            QRCode QrCode = new QRCode(QrCodeInfo);
            Bitmap QrBitmap = QrCode.GetGraphic(60);
            byte[] BitmapArray = BitmapToByteArray(QrBitmap);
            string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
            qrUri = QrUri;
        }


        private byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
