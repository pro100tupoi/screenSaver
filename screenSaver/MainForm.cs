using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using screenSaver.Classes;

namespace screenSaver
{
    public partial class MainForm : Form
    {
        private List<Snowflake> snowflakes;
        private Image[] snowflakeImages;

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
            CreateSnowflakes();
            timer1.Start();
        }

        private void CreateSnowflakes()
        {
            snowflakes = [];
            var rand = new Random();

            var count = rand.Next(500, 801);

            for (int i = 0; i < count; i++)
            {
                // от 1% до 2% размера оригинала
                var scale = (float)(0.01 + rand.NextDouble() * 0.02);

                // Скорость
                var speed = (float)(0.15 + rand.NextDouble() * 7.7);

                speed *= 0.95f + 0.1f * scale;

                var idx = rand.Next(snowflakeImages.Length);

                snowflakes.Add(new Snowflake
                {
                    X = (float)rand.NextDouble() * ClientSize.Width,
                    Y = (float)rand.NextDouble() * (-ClientSize.Height * 2), // распределяем выше
                    Scale = scale,
                    Speed = speed,
                    ImageIndex = idx
                });
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            var rand = new Random();
            foreach (var flake in snowflakes)
            {
                flake.Y += flake.Speed;

                if (flake.Y > ClientSize.Height + 50)
                {
                    flake.Y = (float)rand.NextDouble() * (-100);
                    flake.X = (float)rand.NextDouble() * ClientSize.Width;
                }
            }
            this.Invalidate();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            // Рисуем снежинки
            foreach (var flake in snowflakes)
            {
                Image img = snowflakeImages[flake.ImageIndex];
                var size = img.Width * flake.Scale;
                var w = Math.Max(1, (int)size);
                var h = Math.Max(1, (int)size);
                var x = (int)(flake.X - w / 2f);
                var y = (int)(flake.Y - h / 2f);

                e.Graphics.DrawImage(img, new Rectangle(x, y, w, h));
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e) => Application.Exit();
        private void MainForm_Click(object sender, EventArgs e) => Application.Exit();
    }
}
