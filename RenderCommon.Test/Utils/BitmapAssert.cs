using NUnit.Framework;
using System.Drawing;

namespace RenderCommon.Test.Utils;

internal abstract class BitmapAssert
{
    public static void AreEqual(Bitmap generatedImage, string baselineName)
    {
        var baseline = LoadBaseline(baselineName);
        Assert.That(baseline.Width, Is.EqualTo(generatedImage.Width));
        Assert.That(baseline.Height, Is.EqualTo(generatedImage.Height));

        // Horrible slow per-pixel baseline comparison, but ok for now
        // TODO: improve speed/channel tolerances
        for (var x = 0; x < generatedImage.Width; x++)
        {
            for (var y = 0; y < generatedImage.Height; y++)
            {
                var baselinePixel = baseline.GetPixel(x, y);
                var aPixel = generatedImage.GetPixel(x, y);

                Assert.That(aPixel.ToArgb(), Is.EqualTo(baselinePixel.ToArgb()),
                    $"Pixel {x}, {y} not identical");
            }
        }
    }

    public static void AreEqual(Bitmap image, SparseMatrix<Color> matrix)
    {
        Assert.That(matrix.Columns, Is.EqualTo(image.Width));
        Assert.That(matrix.Rows, Is.EqualTo(image.Height));

        // Horrible slow per-pixel baseline comparison, but ok for now
        // TODO: improve speed/channel tolerances
        for (var x = 0; x < image.Width; x++)
        {
            for (var y = 0; y < image.Height; y++)
            {
                var baselinePixel = matrix[x, y];
                var aPixel = image.GetPixel(x, y);

                Assert.That(aPixel.ToArgb(), Is.EqualTo(baselinePixel.ToArgb()),
                    $"Pixel {x}, {y} not identical");
            }
        }

    }

    private static Bitmap LoadBaseline(string baseline)
    {
        return new Bitmap(Image.FromFile($"Baselines/{baseline}"));
    }

    public static void SaveBitmap(SparseMatrix<Color> matrix, string filename)
    {
        var bitmap = new Bitmap(matrix.Columns, matrix.Rows);
        for (var x = 0; x < matrix.Columns; x++)
        {
            for (var y = 0; y < matrix.Rows; y++)
            {
                bitmap.SetPixel(x, y, matrix[x, y]);
            }
        }
        bitmap.Save(filename);
    }
}
