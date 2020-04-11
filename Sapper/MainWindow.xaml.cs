using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Sapper.Cells;

namespace Sapper
{
    public partial class MainWindow : Window
    {
        private ICell[,] Field;

        private int FieldSize;
        private int CellSize;
        private int BombCount;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeField()
        {
            Field = new ICell[FieldSize, FieldSize];
            InitializeButton();
            InitializeBomb();
        }
        private void InitializeButton()
        {
            Button button;

            for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
            {
                button = new Button
                {
                    Name = "x" + x + "y" + y,
                    Width = CellSize,
                    Height = CellSize,
                };
                button.Click += CellClick;
                button.MouseRightButtonDown += HypothesisClick;
                PanelField.Children.Add(button);
                Field[x, y] = new EmptyCell();
            }
        }
        private void InitializeBomb()
        {
            Random rand = new Random();
            int randX, randY;
            for (int i = 0; i < BombCount; i++)
            {
                randX = rand.Next(FieldSize - 1);
                randY = rand.Next(FieldSize - 1);
                while (Field[randX, randY] is BombCell)
                {
                    randX = rand.Next(FieldSize - 1);
                    randY = rand.Next(FieldSize - 1);
                }
                Field[randX, randY] = new BombCell();
                InitializeNeigh(randX,randY);
            }
        }
        private void InitializeNeigh(int randX, int randY)
        {
            for (int shiftX = -1; shiftX < 2; shiftX++)
                for (int shiftY = -1; shiftY < 2; shiftY++)
                    if ((randX + shiftX > -1) && (randX + shiftX < FieldSize) && (randY + shiftY > -1) && (randY + shiftY < FieldSize))
                        if (Field[randX + shiftX, randY + shiftY] is EmptyCell)
                            Field[randX + shiftX, randY + shiftY].count++;
        }
        private void EzModButton_Click(object sender, RoutedEventArgs e)
        {
            PanelField.Children.Clear();
            FieldSize = 10;
            CellSize = 80;
            BombCount = 10;
            InitializeField();
        }
        private void MidModButton_Click(object sender, RoutedEventArgs e)
        {
            PanelField.Children.Clear();
            FieldSize = 20;
            CellSize = 40;
            BombCount = 40;
            InitializeField();
        }
        private void HardModButton_Click(object sender, RoutedEventArgs e)
        {
            PanelField.Children.Clear();
            FieldSize = 40;
            CellSize = 20;
            BombCount = 140;
            InitializeField();
        }
        private void CellClick(object sender, RoutedEventArgs e)
        {
            string[] xy = (sender as Button).Name.Split(new char[] {'x', 'y'});
            int x = Convert.ToInt32(xy[2]);
            int y = Convert.ToInt32(xy[1]);
            if (Field[x, y] is BombCell)
                GameOver();
            else
                ShowNumber(x,y);
        }
        private void HypothesisClick(object sender, RoutedEventArgs e)
        {
            (sender as Button).Content = "?";
        }
        private void GameOver()
        {
            for (int i = 0; i < FieldSize; i++)
                for (int j = 0; j < FieldSize; j++)
                    if (Field[i, j] is BombCell)
                        ((Button)PanelField.Children[i + j * FieldSize]).Background = Brushes.Red;

            MessageBox.Show("Проиграл");
            PanelField.Children.Clear();
            InitializeField();
        }
        private void ShowNumber(int x, int y)
        {
            if ((x < 0) && (x > FieldSize) && (y < 0) && (y > FieldSize))
                return;;
            if ((Field[x, y].count == 0) && ((Button)PanelField.Children[x + y * FieldSize]).IsEnabled)
            {
                ((Button)PanelField.Children[x + y * FieldSize]).IsEnabled = false;
                for (int shiftX = -1; shiftX < 2; shiftX++)
                    for (int shiftY = -1; shiftY < 2; shiftY++)
                        if ((x + shiftX > -1) && (x + shiftX < FieldSize) && (y + shiftY > -1) && (y + shiftY < FieldSize))
                            if (Field[x + shiftX, y + shiftY] is EmptyCell)
                                if (!((shiftX==0) && (shiftY == 0)))
                                    ShowNumber(x + shiftX, y + shiftY);
            }
            ((Button) PanelField.Children[x + y * FieldSize]).Content = Field[x, y].count;
        }
    }
}
