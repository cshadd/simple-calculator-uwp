using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Simple_Calculator
{
    /// <summary>
    /// The calculator page.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private bool ShowingHistory { get; set; }
        private CalculatorProcessor processor;

        public MainPage() : base()
        {
            this.InitializeComponent();
            this.ShowingHistory = false;
            this.processor = new CalculatorProcessor();
            var calculatorHistoryStack = this.FindName("CalculatorHistoryStack") as StackPanel;
            var newPlaceHolder = new TextBlock();
            newPlaceHolder.Text = "There's no history yet";
            newPlaceHolder.FontSize = 20;
            calculatorHistoryStack.Children.Add(newPlaceHolder);
            return;
        }

        /// <summary>
        /// Invokes a calculator operation based on the sender's name. In this
        /// case a button that is pressed will determine the action to take.
        /// </summary>
        /// <param name="sender">The sender object that called the method</param>
        /// <param name="e">Details about the tapped route</param>
        private void Operation(object sender, TappedRoutedEventArgs e)
        {
            var clickedElementName = ((FrameworkElement)sender).Name;
            Debug.WriteLine("Noga did an operation with " + clickedElementName);

            var calculatorHistoryStack = this.FindName("CalculatorHistoryStack") as StackPanel;
            var operatorTrashHistory = this.FindName("Operator_TrashHistory") as Button;
            
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
                calculatorHistoryStack.Children.Clear();
                var newPlaceHolder = new TextBlock();
                newPlaceHolder.Text = "There's no history yet";
                newPlaceHolder.FontSize = 20;
                calculatorHistoryStack.Children.Add(newPlaceHolder);
                operatorTrashHistory.Visibility = Visibility.Collapsed;
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
                this.processor.AppendNumberToOperation(0);
            }
            else if (clickedElementName == "Operator_1")
            {
                this.processor.AppendNumberToOperation(1);
            }
            else if (clickedElementName == "Operator_2")
            {
                this.processor.AppendNumberToOperation(2);
            }
            else if (clickedElementName == "Operator_3")
            {
                this.processor.AppendNumberToOperation(3);
            }
            else if (clickedElementName == "Operator_4")
            {
                this.processor.AppendNumberToOperation(4);
            }
            else if (clickedElementName == "Operator_5")
            {
                this.processor.AppendNumberToOperation(5);
            }
            else if (clickedElementName == "Operator_6")
            {
                this.processor.AppendNumberToOperation(6);
            }
            else if (clickedElementName == "Operator_7")
            {
                this.processor.AppendNumberToOperation(7);
            }
            else if (clickedElementName == "Operator_8")
            {
                this.processor.AppendNumberToOperation(8);
            }
            else if (clickedElementName == "Operator_9")
            {
                this.processor.AppendNumberToOperation(9);
            }

            // Refresh Result
            var result = this.FindName("CalculatorResult") as TextBlock;
            result.Text = this.processor.ToString();

            if (clickedElementName == "Operator_Equal")
            {
                try
                {
                    result.Text = "" + this.processor.Solve();
                    if (this.processor.history.Count == 1)
                    {
                        calculatorHistoryStack.Children.Clear();
                    }

                    var newPlaceHolder = new TextBlock();
                    newPlaceHolder.TextAlignment = TextAlignment.Right;
                    newPlaceHolder.Margin = new Thickness(0, 0, 10, 0);
                    newPlaceHolder.FontSize = 20;
                    newPlaceHolder.Text = (this.processor.history[this.processor.history.Count - 1].Raw + " = ").Replace("n", "-");
                    calculatorHistoryStack.Children.Add(newPlaceHolder);

                    newPlaceHolder = new TextBlock();
                    newPlaceHolder.TextAlignment = TextAlignment.Right;
                    newPlaceHolder.Margin = new Thickness(0, 0, 10, 0);
                    newPlaceHolder.FontSize = 36;
                    newPlaceHolder.Text = result.Text;
                    calculatorHistoryStack.Children.Add(newPlaceHolder);
                }
                catch (NCalc.EvaluationException ex)
                {
                    result.Text = "Malformed expression!";
                    Debug.WriteLine(ex);
                }
                catch (Exception ex)
                {
                    result.Text = "Unknown error!";
                    Debug.WriteLine(ex);
                }
                finally { }
            }

            // Recalculate History
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
