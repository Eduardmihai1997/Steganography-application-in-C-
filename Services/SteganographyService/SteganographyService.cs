using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageSecret.Services.SteganographyService {
    public class SteganographyService : ISteganographyService
    {


        // ENCODE IMAGE WITH MESSAGE
        public BitmapImage Encode(BitmapImage image, string message)
        {
            var pixels = GetPixelsFromImage(image);
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var messageLength = messageBytes.Length;

            if (pixels.Length < messageBytes.Length * 8 + 32)
            {
                throw new InvalidOperationException("Image is too small to hide the message.");
            }

            var messageLengthBits = ConvertToBitsArray(BitConverter.GetBytes(messageLength));

            pixels = EncodeBitsArray(pixels, messageLengthBits, 0);
            pixels = EncodeBitsArray(pixels, ConvertToBitsArray(messageBytes), 32);

            return CreateImageFromPixels(pixels, image.PixelWidth, image.PixelHeight);
        }

        // DECODE IMAGE FROM MESSAGE
        public string Decode(BitmapImage image)
        {
            var pixels = GetPixelsFromImage(image);

            var messageLengthBits = DecodeBitsArray(pixels, 32, 0);
            var messageLength = BitConverter.ToInt32(messageLengthBits, 0);

            var messageBytes = DecodeBitsArray(pixels, messageLength * 8, 32);

            return Encoding.UTF8.GetString(messageBytes);
        }


        // CONVERT IMAGE TO PIXELS
        private byte[] GetPixelsFromImage(BitmapImage image)
        {
            int width = image.PixelWidth;
            int height = image.PixelHeight;
            int bytesPerPixel = (image.Format.BitsPerPixel + 7) / 8;
            int stride = (width * bytesPerPixel + 3) & ~3;

            byte[] pixels = new byte[height * stride];
            image.CopyPixels(new Int32Rect(0, 0, width, height), pixels, stride, 0);

            return pixels;
        }

        // CONVERT PIXELS TO BIT ARRAY
        private byte[] ConvertToBitsArray(byte[] bytes)
        {
            var bits = new byte[bytes.Length * 8];

            for (int i = 0; i < bytes.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bits[i * 8 + j] = (byte)((bytes[i] >> j) & 1);
                }
            }

            return bits;
        }


        // ENCODE MESSAGE INTO PIXELS
        private byte[] EncodeBitsArray(byte[] pixels, byte[] bits, int offset)
        {
            for (int i = 0; i < bits.Length; i++)
            {
                pixels[offset + i] = (byte)((pixels[offset + i] & 0xFE) | bits[i]);
            }

            return pixels;
        }


        // DECODE MESSAGE FROM PIXELS
        private byte[] DecodeBitsArray(byte[] pixels, int length, int offset)
        {
            byte[] bits = new byte[length];

            for (int i = 0; i < length; i++)
            {
                bits[i] = (byte)(pixels[offset + i] & 1);
            }

            int bytesLength = (int)Math.Ceiling(length / 8.0);
            byte[] bytes = new byte[bytesLength];

            for (int i = 0; i < bytesLength; i++)
            {
                for (int j = 0; j < 8 && i * 8 + j < length; j++)
                {
                    bytes[i] |= (byte)(bits[i * 8 + j] << j);
                }
            }

            return bytes;
        }

        //Create image from pixels
        private BitmapImage CreateImageFromPixels(byte[] pixels, int width, int height)
        {
            BitmapImage resultImage = new BitmapImage();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32, null, pixels, width * 4)));
                encoder.Save(memoryStream);

                resultImage.BeginInit();
                resultImage.StreamSource = new MemoryStream(memoryStream.ToArray());
                resultImage.EndInit();
                resultImage.Freeze();
            }

            return resultImage;
        }
    }
}
