namespace Sapper.Cells
{
    class EmptyCell : ICell
    {
        public int count { get; set; } = 0;
        public int Press()
        {
            return 0; 
        }
    }
}
