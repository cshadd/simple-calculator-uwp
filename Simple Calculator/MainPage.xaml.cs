// ***********************************************************************
// Assembly         : Simple Calculator
// Author           : Christian
// Created          : 09-23-2019
//
// Last Modified By : Christian
// Last Modified On : 09-23-2019
// ***********************************************************************
// <copyright file="MainPage.xaml.cs" company="">
//     Copyright © 2019 Christian Shadd
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Simple_Calculator
{
    /// <summary>
    /// The main page.
    /// </summary>
    sealed partial class MainPage : Page
    {
        /// <summary>
        /// Gets or sets a value indicating whether history is shown.
        /// </summary>
        /// <value><c>true</c> if history is shown; otherwise,
        /// <c>false</c>.</value>
        private bool ShowingHistory { get; set; }
        /// <summary>
        /// The processor that is used to calculate our expressions.
        /// </summary>
        /// <seealso cref="CalculatorProcessor"/>
        private CalculatorProcessor processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage() : base()
        {
            this.InitializeComponent();
            this.ShowingHistory = false;
            this.processor = new CalculatorProcessor();
            this.CreateHistoryPlaceHolder();
            Debug.WriteLine("I love calculators!", "NOGA");
            return;
        }

        /// <summary>
        /// Creates the history place holder.
        /// </summary>
        private void CreateHistoryPlaceHolder()
        {
            var calculatorHistoryStack = this.FindName("CalculatorHistoryStack") as StackPanel;
            calculatorHistoryStack.Children.Clear();
            var newPlaceHolder = new TextBlock();
            newPlaceHolder.Text = "There's no history yet";
            newPlaceHolder.FontSize = 20;
            calculatorHistoryStack.Children.Add(newPlaceHolder);
            return;
        }

        /// <summary>
        /// Invokes a calculator operation based on the sender's name. In this
        /// case a button that is tapped will determine the action to take.
        /// </summary>
        /// <param name="sender">The sender object that called the method.</param>
        /// <param name="e">Details about the tapped route.</param>
        private void Operation(object sender, TappedRoutedEventArgs e)
        {
            var clickedElementName = ((FrameworkElement)sender).Name;
            Debug.WriteLine("Operation " + clickedElementName + " executed!", "NOGA");

            var calculatorHistoryStack = this.FindName("CalculatorHistoryStack") as StackPanel;

            
            if (clickedElementName == "Operator_Exit")
            {
                Application.Current.Exit();
            }
            else if (clickedElementName == "Operator_History")
            {
                var calculatorButtons = this.FindName("CalculatorButtons") as Grid;
                var calculatorHistory = this.FindName("CalculatorHistory") as Grid;

                if (this.ShowingHistory)
                {
                    calculatorButtons.Visibility = Visibility.Visible;
                    calculatorHistory.Visibility = Visibility.Collapsed;
                }
                else
                {
                    calculatorButtons.Visibility = Visibility.Collapsed;
                    calculatorHistory.Visibility = Visibility.Visible;
                }
                this.ShowingHistory = !this.ShowingHistory;
            }
            else if (clickedElementName == "Operator_TrashHistory")
            {
                this.processor.ClearHistory();
                this.CreateHistoryPlaceHolder();
            }
            else if (clickedElementName == "Operator_Percent")
            {
                this.processor.Percent();
            }
            else if (clickedElementName == "Operator_Sqrt")
            {
                this.processor.Sqrt();
            }
            else if (clickedElementName == "Operator_Sqr")
            {
                this.processor.Sqr();
            }
            else if (clickedElementName == "Operator_1Overx")
            {
                this.processor.OneOverX();
            }
            else if (clickedElementName == "Operator_CE")
            {
                this.processor.CE();
            }
            else if (clickedElementName == "Operator_C")
            {
                this.processor.C();
            }
            else if (clickedElementName == "Operator_Del")
            {
                this.processor.Delete();
            }
            else if (clickedElementName == "Operator_Div")
            {
                this.processor.Div();
            }
            else if (clickedElementName == "Operator_Mul")
            {
                this.processor.Mul();
            }
            else if (clickedElementName == "Operator_Sub")
            {
                this.processor.Sub();
            }
            else if (clickedElementName == "Operator_Add")
            {
                this.processor.Add();
            }
            else if (clickedElementName == "Operator_Dot")
            {
                this.processor.AppendDecimal();
            }
            else if (clickedElementName == "Operator_Reverse")
            {
                this.processor.ReverseOperationNumber();
            }
            else if (clickedElementName == "Operator_0")
            {
                this.processor.AppendNumber(0);
            }
            else if (clickedElementName == "Operator_1")
            {
                this.processor.AppendNumber(1);
            }
            else if (clickedElementName == "Operator_2")
            {
                this.processor.AppendNumber(2);
            }
            else if (clickedElementName == "Operator_3")
            {
                this.processor.AppendNumber(3);
            }
            else if (clickedElementName == "Operator_4")
            {
                this.processor.AppendNumber(4);
            }
            else if (clickedElementName == "Operator_5")
            {
                this.processor.AppendNumber(5);
            }
            else if (clickedElementName == "Operator_6")
            {
                this.processor.AppendNumber(6);
            }
            else if (clickedElementName == "Operator_7")
            {
                this.processor.AppendNumber(7);
            }
            else if (clickedElementName == "Operator_8")
            {
                this.processor.AppendNumber(8);
            }
            else if (clickedElementName == "Operator_9")
            {
                this.processor.AppendNumber(9);
            }

            // Refresh Result after each button tap.
            var calculatorResult = this.FindName("CalculatorResult") as TextBlock;
            calculatorResult.Text = this.processor.ToString();

            if (clickedElementName == "Operator_Equal")
            {
                try
                {
                    var result = this.processor.Solve();
                    calculatorResult.Text = result;

                    Debug.WriteLine("Result is " + result + "!", "NOGA");

                    // If history only has one value, which is most likely the
                    // no history placeholder...
                    if (this.processor.history.Count == 1)
                    {
                        // Clear history.
                        calculatorHistoryStack.Children.Clear();
                    }

                    // Top entry of the history stack.
                    var top = this.processor.history.Peek();

                    // Add solved problem to history view.
                    var newPlaceHolder = new TextBlock();
                    newPlaceHolder.TextAlignment = TextAlignment.Right;
                    newPlaceHolder.Margin = new Thickness(0, 0, 10, 0);
                    newPlaceHolder.FontSize = 20;
                    newPlaceHolder.Text = (top.ProperExpression() + " = ");
                    calculatorHistoryStack.Children.Add(newPlaceHolder);

                    newPlaceHolder = new TextBlock();
                    newPlaceHolder.TextAlignment = TextAlignment.Right;
                    newPlaceHolder.Margin = new Thickness(0, 0, 10, 0);
                    newPlaceHolder.FontSize = 36;
                    newPlaceHolder.Text = result;
                    calculatorHistoryStack.Children.Add(newPlaceHolder);
                }
                catch (NCalc.EvaluationException ex)
                {
                    calculatorResult.Text = "Malformed expression!";
                    Debug.WriteLine(ex, "ERROR");
                }
                catch (Exception ex)
                {
                    calculatorResult.Text = "Unknown error!";
                    Debug.WriteLine(ex, "ERROR");
                }
                finally { }
            }

            var operatorTrashHistory = this.FindName("Operator_TrashHistory") as Button;

            // Recalculate the need to show the history trashcan after each
            // button tap.
            if (this.processor.history.Count > 0)
            {
                operatorTrashHistory.Visibility = Visibility.Visible;
            }
            else
            {
                operatorTrashHistory.Visibility = Visibility.Collapsed;
            }

            return;
        }
    }
}
