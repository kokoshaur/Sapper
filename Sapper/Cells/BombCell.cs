namespace Sapper.Cells
{
    class BombCell : ICell
    {
        public int count { get; set; } = 0;
        public int Press()
        {
            return 1;
        }
    }
}
