﻿using OpenCvSharp.Extensions;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.Core.GameSystem.GameHelper
{
    public class FuseBitmap
    {
        //背景图
        //写入底图
        //PointBitmap dstPb = null;
        private BitmapDataBitmap dstPb = null;

        public Bitmap dstBitmap = null;
        public bool[][] isHand;
        public int awidth = 0;
        public int aheight = 0;
        public int minWidth = 0;
        public int maxWidth = 0;
        public int minHeigh = 0;
        public int maxHeigh = 0;

        public static BitmapDataBitmap BackGroundPb = null;
        public static Bitmap BackGroundBmp = null;
        public static bool BackGroundBmplock = false;
        public static int tolerance = 30;

        public int unsamePointSum = 0;

        public static void setBackGround(Bitmap backgroup, int tol)
        {
            if (BackGroundBmplock)
            {
                BackGroundPb.UnlockBits();
                BackGroundPb = null;
            }
            tolerance = tol;
            BackGroundBmp = DeepCopyBitmap(backgroup);
            BackGroundPb = new BitmapDataBitmap(BackGroundBmp);
            BackGroundPb.LockBits();
            BackGroundBmplock = true;
        }

        public FuseBitmap(Bitmap back)
        {
            awidth = back.Width;
            aheight = back.Height;
            isHand = new bool[awidth][];
            for (int i = 0; i < awidth; i++)
            {
                isHand[i] = new bool[aheight];
            }
            dstBitmap = DeepCopyBitmap(back);
            dstPb = new BitmapDataBitmap(dstBitmap);
            dstPb.LockBits();
        }

        public void SetRect(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            minWidth = p1.X;
            maxWidth = p2.X;
            minHeigh = p1.Y;
            maxHeigh = p2.Y;
        }

        public void Dispose()
        {
            dstPb.UnlockBits();
        }

        /* public void FuseColorImg(bool flag = false)
         {
             Parallel.For(minWidth, maxWidth, new ParallelOptions { MaxDegreeOfParallelism = 3 }, (i) =>
             //Parallel.For(0, awidth,(j) =>
             {
                 //R>95 && G>40 && B>20 && R>G && R>B && Max(R,G,B)-Min(R,G,B)>15 && Abs(R-G)>15
                 //R [p + 2]
                 //G [p + 1]
                 //B [p]
                 for (int j = minHeigh; j < maxHeigh; j++)
                 {
                     //定位像素点位置
                     int p = j * awidth * 3 + i * 3;
                     if (dstPb.srcArray[p + 2] > 95 &&
                         dstPb.srcArray[p + 1] > 40 &&
                         dstPb.srcArray[p] > 20 &&
                         dstPb.srcArray[p + 2] > dstPb.srcArray[p + 1] &&
                         dstPb.srcArray[p + 2] > dstPb.srcArray[p] &&
                         Max(dstPb.srcArray[p + 2], dstPb.srcArray[p + 1], dstPb.srcArray[p]) - Min(dstPb.srcArray[p + 2], dstPb.srcArray[p + 1], dstPb.srcArray[p]) > 15 &&
                         Math.Abs(dstPb.srcArray[p + 2] - dstPb.srcArray[p + 1]) > 15)
                     {
                         if (flag)
                         {
                             dstPb.srcArray[p] = 0;
                             dstPb.srcArray[p + 1] = 0;
                             dstPb.srcArray[p + 2] = 255;
                         }
                         isHand[i][j] = true;
                     }
                 }
             });
         }*/

        public void FuseColorImg(int tol, bool flag = false)
        {
            unsamePointSum = 0;
            tolerance = tol;
            int[] unsame = new int[maxWidth - minWidth];
            Parallel.For(minWidth, maxWidth, new ParallelOptions { MaxDegreeOfParallelism = 3 }, (i) =>
            //Parallel.For(0, awidth,(j) =>
            {
                //R>95 && G>40 && B>20 && R>G && R>B && Max(R,G,B)-Min(R,G,B)>15 && Abs(R-G)>15
                //R [p + 2]
                //G [p + 1]
                //B [p]
                for (int j = minHeigh; j < maxHeigh; j++)
                {
                    //定位像素点位置
                    int p = j * awidth * 3 + i * 3;
                    int[] list = new int[3];
                    list[0] = (dstPb.srcArray[p + 2] - BackGroundPb.srcArray[p + 2]);
                    list[1] = (dstPb.srcArray[p + 1] - BackGroundPb.srcArray[p + 1]);
                    list[2] = (dstPb.srcArray[p] - BackGroundPb.srcArray[p]);
                    Array.Sort(list);
                    if (list[list.Length - 1] - list[0] > tolerance)
                    {
                        if (flag)
                        {
                            dstPb.srcArray[p] = 0;
                            dstPb.srcArray[p + 1] = 0;
                            dstPb.srcArray[p + 2] = 255;
                        }
                        isHand[i][j] = true;
                        unsame[i - minWidth]++;
                    }
                }
            });

            for (int i = 0; i < unsame.Length; i++)
            {
                unsamePointSum += unsame[i];
            }
        }

        private int Max(int R, int G, int B)
        {
            int Max = R > G ? R : G;
            if (Max < B)
            {
                Max = B;
            }
            return Max;
        }

        private int Min(int R, int G, int B)
        {
            int Min = R < G ? R : G;
            if (Min > B)
            {
                Min = B;
            }
            return Min;
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

        private static Bitmap ResizeImage(Image image, int Width, int Height)
        {
            int fillWidth = 0, fillHeight = 0;
            int fillWidth2 = 0, fillHeight2 = 0;

            if (image.Width < Width && image.Height < Height)
            {
                fillWidth = (Width - image.Width) / 2;
                fillWidth2 = Width - image.Width - fillWidth;

                fillHeight = (Height - image.Height) / 2;
                fillHeight2 = Height - image.Height - fillHeight;
                using (Mat source = BitmapConverter.ToMat((Bitmap)image))
                using (Mat dest = new Mat(new OpenCvSharp.Size(Width, Height), source.Type()))
                {
                    Cv2.CopyMakeBorder(source, dest, fillHeight, fillHeight2, fillWidth, fillWidth2, BorderTypes.Constant, null);
                    return dest.ToBitmap(image.PixelFormat);
                }
            }

            bool horizontalFill = Height > Width;
            int resizedWidth, resizedHeight;
            // ┌┬┬┐
            // ││││
            // └┴┴┘
            if (horizontalFill)
            {
                resizedWidth = image.Width * Height / image.Height;
                resizedHeight = Height;
                fillWidth = (Width - resizedWidth) / 2;
                fillWidth2 = Width - resizedWidth - fillWidth;
            }
            // ┌─┐
            // ├─┤
            // ├─┤
            // └─┘
            else
            {
                resizedWidth = Width;
                resizedHeight = image.Height * Width / image.Width;
                fillHeight = (Height - resizedHeight) / 2;
                fillHeight2 = Height - resizedHeight - fillHeight;
            }
            using (Mat resizedMat = new Mat())
            using (Mat source = BitmapConverter.ToMat((Bitmap)image))
            using (Mat dest = new Mat(new OpenCvSharp.Size(Width, Height), source.Type()))
            {
                Cv2.Resize(source, resizedMat, new OpenCvSharp.Size(resizedWidth, resizedHeight));
                Cv2.CopyMakeBorder(resizedMat, dest, fillHeight, fillHeight2, fillWidth, fillWidth2, BorderTypes.Constant, null);
                return dest.ToBitmap(image.PixelFormat);
            }
        }
    }
}