using System;
using System.Drawing;
using System.IO;
using QRCoder;

namespace GloboWeather.WeatherManagement.Infrastructure.Helpers
{
    public class QRCodeHelper
    {
        public static Bitmap CreateQRCode(string input, bool drawQuietZones = false)
        {
            //https://github.com/codebude/QRCoder
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(input, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(5, Color.Black, Color.White, drawQuietZones: drawQuietZones);
            
            return qrCodeImage;
        }

        public static (Stream FileStream, string FilePath) CreateQRCodeStream(string input)
        {
            var tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}_tmp.jpg");
            var image = CreateQRCode(input);
            Stream msStream = new MemoryStream();

            image.Save(tempPath, System.Drawing.Imaging.ImageFormat.Jpeg);

            var stream = File.OpenRead(tempPath);

            return (stream, tempPath);

        }
    }
}
