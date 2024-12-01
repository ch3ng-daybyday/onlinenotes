using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateCaptcha
{
    public class CaptchaResult
    {
        public byte[] ImageBytes { get; }
        public string VerificationCode { get; }

        public CaptchaResult(byte[] imageBytes, string verificationCode)
        {
            ImageBytes = imageBytes;
            VerificationCode = verificationCode;
        }
    }
    public class VerificationCode
    {
        private readonly static Random random = new Random();
        public CaptchaResult getVerificationCodeByte(int codeBit, bool isAddLine)
        {
            //var random = new Random();
            codeBit = codeBit > 8 || codeBit <= 0 ? 4 : codeBit;
            var code = random.Next((int)Math.Pow(10.0, codeBit - 1), (int)(Math.Pow(10.0, codeBit) - 1)).ToString();
            var bitmap = new Bitmap(100, 40);

            using (MemoryStream ms = new MemoryStream())
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    using (var font = new Font("Arial", 20, FontStyle.Bold))
                    {
                        g.Clear(Color.White);
                        int xPosition = 0;
                        int xMove = 100 / (codeBit + 1);
                        for (int i = 0; i < codeBit; i++)
                        {
                            int yPosition = random.Next(5, 20);
                            PointF point = new PointF(xPosition + font.Size / 2, yPosition + font.Size / 2);
                            g.DrawString(code[i].ToString(), font, Brushes.Black, new PointF(xPosition, yPosition));
                            xPosition += xMove;
                        }
                    }
                    //draw Line
                    if (isAddLine)
                    {
                        var pen = new Pen(Color.Black);
                        for (int i = 0; i < 10; i++)
                        {
                            g.DrawLine(pen, random.Next(0, 100), random.Next(0, 40), random.Next(0, 100), random.Next(0, 40));
                        }
                    }
                }
                bitmap.Save(ms, ImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);
                var imgData = ms.ToArray();
                return new CaptchaResult(imgData, code);
            }
        }
    }
}
