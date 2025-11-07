using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using screenSaver.Classes;

namespace screenSaver
{
    public partial class MainForm : Form
    {
        private List<Snowflake> snowflakes;
        private Image[] snowflakeImages;
        private readonly Random rand = new Random();

        // Буфер для двойной буферизации
        private Bitmap backBuffer;
        private Graphics bufferGraphics;

        public MainForm()
        {
            InitializeComponent();

            snowflakeImages = new Image[]
            {
                Properties.Resources.snowflake1,
                Properties.Resources.snowflake2,
                Properties.Resources.snowflake3,
                Properties.Resources.snowflake4,
                Properties.Resources.snowflake5,
                Properties.Resources.snowflake6
            };

            timer1.Interval = 16;
            timer1.Tick += Timer1_Tick;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Создаём буфер при загрузке
            backBuffer = new Bitmap(ClientSize.Width, ClientSize.Height);
            bufferGraphics = Graphics.FromImage(backBuffer);
            bufferGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            CreateSnowflakes();
            timer1.Start();
        }

        private void CreateSnowflakes()
        {
            snowflakes = new List<Snowflake>();
            var count = rand.Next(500, 801);

            for (var i = 0; i < count; i++)
            {
                var scale = (float)(0.01 + rand.NextDouble() * 0.02);
                var speed = (float)(0.15 + rand.NextDouble() * 7.7);
                speed *= (0.95f + 0.1f * scale);

                snowflakes.Add(new Snowflake
                {
                    X = (float)rand.NextDouble() * ClientSize.Width,
                    Y = (float)rand.NextDouble() * (-ClientSize.Height * 2),
                    Scale = scale,
                    Speed = speed,
                    ImageIndex = rand.Next(snowflakeImages.Length)
                });
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            // Обновляем позиции
            foreach (var flake in snowflakes)
            {
                flake.Y += flake.Speed;

                if (flake.Y > ClientSize.Height + 50)
                {
                    flake.Y = (float)rand.NextDouble() * (-100);
                    flake.X = (float)rand.NextDouble() * ClientSize.Width;
                }
            }

            // Если у вас фоновое изображение (BackgroundImage), рисуем его
            if (BackgroundImage != null)
            {
                bufferGraphics.DrawImage(BackgroundImage, Point.Empty);
            }

            // Рисуем снежинки
            foreach (var flake in snowflakes)
            {
                Image img = snowflakeImages[flake.ImageIndex];
                var size = img.Width * flake.Scale;
                var w = Math.Max(1, (int)size);
                var h = Math.Max(1, (int)size);
                var x = (int)(flake.X - w / 2f);
                var y = (int)(flake.Y - h / 2f);

                bufferGraphics.DrawImage(img, new Rectangle(x, y, w, h));
            }

            // Копируем готовый кадр на экран
            using (var screenGraphics = CreateGraphics())
            {
                screenGraphics.DrawImage(backBuffer, Point.Empty);
            }
        }

        private void MainForm_Click(object sender, EventArgs e) => Application.Exit();
        private void MainForm_KeyDown(object sender, KeyEventArgs e) => Application.Exit();

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (ClientSize.Width > 0 && ClientSize.Height > 0)
            {
                backBuffer?.Dispose();
                bufferGraphics?.Dispose();

                backBuffer = new Bitmap(ClientSize.Width, ClientSize.Height);
                bufferGraphics = Graphics.FromImage(backBuffer);
                bufferGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            }
        }
    }
}
