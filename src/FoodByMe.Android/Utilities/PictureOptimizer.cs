using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using FoodByMe.Core.Contracts;

namespace FoodByMe.Android.Utilities
{
    public class PictureOptimizer : IPictureOptimizer
    {
        private const int Width = 640;

        private const int Height = 400;

        public async Task OptimizeAsync(Stream input, Stream output)
        {
            var options = await GetDimensionsAsync(input).ConfigureAwait(false);
            options.InSampleSize = CalculateInSampleSize(options, Width, Height);
            options.InJustDecodeBounds = false;

            input.Position = 0;
            var bitmap = await BitmapFactory
                .DecodeStreamAsync(input, new Rect(), options)
                .ConfigureAwait(false);
            await bitmap.CompressAsync(Bitmap.CompressFormat.Jpeg, 100, output)
                .ConfigureAwait(false);
        }

        private static async Task<BitmapFactory.Options> GetDimensionsAsync(Stream input)
        {
            var options = new BitmapFactory.Options {InJustDecodeBounds = true};
            await BitmapFactory.DecodeStreamAsync(input, new Rect(), options).ConfigureAwait(false);
            return options;
        }

        private static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Raw height and width of image
            float height = options.OutHeight;
            float width = options.OutWidth;
            var inSampleSize = 1D;

            if (!(height > reqHeight) && !(width > reqWidth))
            {
                return (int) inSampleSize;
            }
            var halfHeight = (int) (height/2);
            var halfWidth = (int) (width/2);

            // Calculate a inSampleSize that is a power of 2 - the decoder will use a value that is a power of two anyway.
            while (halfHeight/inSampleSize > reqHeight && halfWidth/inSampleSize > reqWidth)
            {
                inSampleSize *= 2;
            }

            return (int) inSampleSize;
        }
    }
}