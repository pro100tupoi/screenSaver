using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace screenSaver.Classes
{
    /// <summary>
    /// Представляет одну снежинку в анимации: хранит позицию, размер, скорость и тип изображения.
    /// </summary>
    public class Snowflake
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Scale { get; set; }
        public float Speed { get; set; }
        public int ImageIndex { get; set; }
    }
}
