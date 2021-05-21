namespace Avalonia.Extensions.Controls
{
    public sealed class Options
    {
        public Options(ShowPosition position)
        {
            this.Position = position;
            this.Size = new Size(160, 60);
        }
        public Options(ShowPosition position, Size size)
        {
            this.Size = size;
            this.Position = position;
        }
        public Options(ShowPosition position, Size size, ScollOrientation scollOrientation) : this(position, size)
        {
            this.ScollOrientation = scollOrientation;
        }
        public Options(ShowPosition position, Size size, ScollOrientation scollOrientation, int movePixel) : this(position, size, scollOrientation)
        {
            this.MovePixel = movePixel;
        }
        public Options(ShowPosition position, Size size, ScollOrientation scollOrientation, int movePixel, int moveDelay) : this(position, size, scollOrientation, movePixel)
        {
            this.MoveDelay = moveDelay;
        }
        public Size Size { get; set; }
        public ShowPosition Position { get; set; }
        public int MovePixel { get; set; } = 1;
        public int MoveDelay { get; set; } = 20;
        public ScollOrientation ScollOrientation { get; set; }
        public bool IsVaidate
        {
            get
            {
                return !((Position == ShowPosition.TopLeft || Position == ShowPosition.TopRight)
                    && ScollOrientation == ScollOrientation.Vertical);
            }
        }
        public void Update(Options options)
        {
            Size = options.Size;
            Position = options.Position;
            MovePixel = options.MovePixel;
            MoveDelay = options.MoveDelay;
            ScollOrientation = options.ScollOrientation;
        }
    }
}