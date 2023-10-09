using OpenCvSharp.XFeatures2D;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp.Extensions;

namespace Volleyball.Core.GameSystem.GameHelper
{
    public class ImageHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static OpenCvSharp.Mat Bitmap2Mat(Bitmap bmp)
        {
            OpenCvSharp.Mat mat = null;
            try
            {
                mat = OpenCvSharp.Extensions.BitmapConverter.ToMat(bmp);//用bitmap转换为mat
            }
            catch (Exception ex)
            {
                mat = null;
                Console.WriteLine(ex.Message);
            }

            return mat;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Bitmap Mat2Bitmap(OpenCvSharp.Mat mat)
        {
            Bitmap bmp = null;
            try
            {
                bmp = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(mat);//用bitmap转换为mat
            }
            catch (Exception ex)
            {
                bmp = null;
                Console.WriteLine(ex.Message);
            }

            return bmp;
        }

        /// <summary>
        /// 深度复制bitmap
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap DeepCopyBitmap(Bitmap bitmap)
        {
            try
            {
                if (bitmap == null) return null;
                Bitmap dstBitmap = bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), bitmap.PixelFormat);
                return dstBitmap;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : {0}", ex.Message);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="imgSrc"></param>
        /// <param name="imgSub"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static System.Drawing.Point FindPicFromImage(Bitmap imgSrc, Bitmap imgSub, double threshold = 0.9)
        {
            OpenCvSharp.Mat srcMat = null;
            OpenCvSharp.Mat dstMat = null;
            OpenCvSharp.OutputArray outArray = null;
            try
            {
                srcMat = imgSrc.ToMat();
                dstMat = imgSub.ToMat();
                outArray = OpenCvSharp.OutputArray.Create(srcMat);
                OpenCvSharp.Cv2.MatchTemplate(srcMat, dstMat, outArray, TemplateMatchModes.SqDiffNormed);
                double minValue, maxValue;
                OpenCvSharp.Point location, point;
                OpenCvSharp.Cv2.MinMaxLoc(OpenCvSharp.InputArray.Create(outArray.GetMat()), out minValue, out maxValue, out location, out point);
                Console.WriteLine(maxValue);
                if (maxValue >= threshold)
                    return new System.Drawing.Point(point.X, point.Y);
                return System.Drawing.Point.Empty;
            }
            catch (Exception ex)
            {
                return System.Drawing.Point.Empty;
            }
            finally
            {
                if (srcMat != null)
                    srcMat.Dispose();
                if (dstMat != null)
                    dstMat.Dispose();
                if (outArray != null)
                    outArray.Dispose();
            }
        }

        /// <summary>
        /// FindGoodMatchs
        /// </summary>
        /// <param name="src"></param>
        /// <param name="matches"></param>
        /// <returns></returns>
        private static DMatch[] FindGoodMatchs(Mat src, DMatch[] matches)
        {
            double max = 0, min = 1000;
            for (int i = 0; i < src.Rows; i++)
            {
                double dist = matches[i].Distance;
                if (dist > max)
                {
                    max = dist;
                }
                if (dist < min)
                {
                    min = dist;
                }
            }
            var good = new List<DMatch>();
            for (int i = 0; i < src.Rows; i++)
            {
                double dist = matches[i].Distance;
                if (dist < Math.Max(3 * min, 0.02))
                {
                    good.Add(matches[i]);
                }
            }
            return good.ToArray();
        }

        /// <summary>
        /// SURF 特征匹配
        /// </summary>
        /// <returns></returns>
        private static Mat MatchBySURF2(Mat src2, Mat src1)
        {
            var gray1 = new Mat();
            var gray2 = new Mat();

            Cv2.CvtColor(src1, gray1, ColorConversionCodes.BGR2GRAY);
            Cv2.CvtColor(src2, gray2, ColorConversionCodes.BGR2GRAY);

            var surf = SURF.Create(1000, 4, 2, true);

            var descriptors1 = new Mat();
            var descriptors2 = new Mat();
            surf.DetectAndCompute(gray1, null, out KeyPoint[] keypoints1, descriptors1);
            surf.DetectAndCompute(gray2, null, out KeyPoint[] keypoints2, descriptors2);

            var matcher = new FlannBasedMatcher();
            DMatch[] matches = matcher.Match(descriptors1, descriptors2);

            var img = new Mat();
            Cv2.DrawMatches(gray1, keypoints1, gray2, keypoints2, matches, img, new Scalar(0, 255, 0), new Scalar(0, 0, 255), null, DrawMatchesFlags.NotDrawSinglePoints);

            return img;
        }
    }
}