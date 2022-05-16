using Conecta4.Wpf.Core;
using Conecta4.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Conecta4.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game game { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            
            this.Left = SystemParameters.VirtualScreenLeft;
            this.Top = SystemParameters.VirtualScreenTop + 350;

            CreateGame();

            this.Loaded += StartGame_Loaded;
        }

        private void CreateGame()
        {
            game = new Game();

            CreateBoardUI(game.BoardRows, game.BoardColumns);
        }

        private void StartGame_Loaded(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            // Randomnly determine starter player
            Random random = new Random();
            int number = random.Next(0, 2);

            game.IsPlayerTurn = (number == 0) ? true : false;

            if (game.IsPlayerTurn)
            {
                MessageBox.Show("Player starts!");
            }
            else
            {
                MessageBox.Show("Computer starts!");

                PlayComputerTurn();
            }
        }

        private void PlayComputerTurn()
        {
            if (game.IsPlayerTurn == false) { 

                (int row, int col) = game.RunComputerTurn();
                PlaceChipUI(row, col, CellType.ComputerChip);

                (bool isWinner, List<CellPositionModel>? winner4) = game.CheckWinner();

                if (isWinner)
                {
                    EndGame(winner4, false);
                }
                else
                {
                    game.IsPlayerTurn = true;
                }

            }
        }

        private void BtnDrop_Click(object sender, RoutedEventArgs e)
        {
            if (game.IsPlayerTurn == false)
            {
                return;
            }

            var buttonClicked = (Button)sender;
            int column = Grid.GetColumn(buttonClicked);
            txtInfoGame.Text = $"Click {column}";

            int rowPlaced = game.RunPlayerTurn(column);

            if (rowPlaced < 0)
            {
                MessageBox.Show("This column is full! Please select another one");
            } 
            else
            {
                PlaceChipUI(rowPlaced, column, CellType.PlayerChip);

                (bool isWinner, List<CellPositionModel>? winner4) = game.CheckWinner();

                if (isWinner)
                {
                    EndGame(winner4, true);
                }
                else
                {
                    game.IsPlayerTurn = false;
                    PlayComputerTurn();
                }

            }

        }

        private void EndGame(List<CellPositionModel> cells, bool humanWinner)
        {
            string winnerMessage = humanWinner ? "Congratulations!! You win!!" : "Oooooh, you lost...";

            MessageBox.Show(winnerMessage);

            foreach (var cell in cells)
            {
                PlaceChipUI(cell.Row, cell.Column, CellType.Winner);
            }


        }

        private void PlaceChipUI(int row, int column, CellType cellType)
        {
            SolidColorBrush color = new SolidColorBrush();
            
            switch (cellType)
            {
                case CellType.PlayerChip:
                    color.Color = Colors.Red;
                    break;

                case CellType.ComputerChip:
                    color.Color = Colors.Yellow;
                    break;

                case CellType.Winner:
                    color.Color = Colors.Green;
                    break;

                default:
                    color.Color = Colors.White;
                    break;

            }

            Ellipse cellContent = new Ellipse();

            foreach (UIElement child in boardGrid.Children)
            {
                if (Grid.GetRow(child) == (game.BoardRows -1 - row) && Grid.GetColumn(child) == (column))
                {
                    cellContent = (Ellipse)child;
                }
            }

            cellContent.Fill = color;
        }

        private void CreateBoardUI(int rows, int columns)
        {

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    CreateBoardCellUI(boardGrid, row, col);
                }
            }
        }


        private void CreateBoardCellUI(Grid board, int row, int col)
        {
            Ellipse cell = new Ellipse();

            board.Children.Add(cell);
            Grid.SetRow(cell, row);
            Grid.SetColumn(cell, col);
        }

        private void MenuNewGame_Click(object sender, RoutedEventArgs e)
        {
            CreateGame();
            StartGame();
        }
    }
}
