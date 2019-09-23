using NCalc;
using System.Collections.Generic;

namespace Simple_Calculator
{
    public class CalculatorProcessor
    {
        private Problem currentProblem;
        public List<Problem> history { get; private set; }

        private bool Halt { get; set; }
        private bool NoDelete { get; set; }

        public CalculatorProcessor() : base()
        {
            this.history = new List<Problem>();
            this.currentProblem = new Problem(0);
            this.Halt = false;
            this.NoDelete = false;
            return;
        }

        public class Problem
        {
            private Expression solver;

            public string Raw { get; set; }
            private string Result { get; set; }

            public Problem() : this("")
            {
                return;
            }

            public Problem(double raw) : this(raw + "")
            {
                return;
            }

            public Problem(string raw) : base()
            {
                this.Raw = raw;
                this.Result = null;
                this.solver = null;
            }

            public bool NegativeExists()
            {
                return this.Raw.Contains("n");
            }

            public void SetRawWithNumber(double number)
            {
                this.Raw = number + "";
                if (number < 0)
                {
                    this.Raw = "n" + this.Raw;
                }
                return;
            }

            public string Solve()
            {
                this.solver = new Expression(this.Raw.Replace("n", "-1*"));
                this.Result = this.solver.Evaluate() + "";
                return this.Result;
            }
        }

        public void AppendDecimal()
        {
            this.currentProblem.Raw += ".";
            return;
        }

        public void AppendNumberToOperation(int number)
        {
            if (this.currentProblem.Raw.Equals("0"))
            {
                this.currentProblem.SetRawWithNumber(number);
            }
            else
            {
                this.currentProblem.Raw += number;
            }
            this.Halt = false;
            return;
        }

        public void Add()
        {
            this.currentProblem.Raw += " + ";
            this.Halt = true;
            return;
        }

        public void C()
        {
            this.currentProblem.SetRawWithNumber(0);
            return;
        }

        public void CE()
        {
            var components = this.currentProblem.Raw.Split();
            var endOf = this.currentProblem.Raw.LastIndexOf(components[components.Length - 1]);
            var canRemove = double.TryParse(this.currentProblem.Raw.Substring(endOf), out double result);
            if (canRemove)
            {
                this.currentProblem.Raw = this.currentProblem.Raw.Substring(0, endOf);
            }

            if (components.Length == 1)
            {
                this.currentProblem.SetRawWithNumber(0);
            }
            return;
        }

        public void ClearHistory()
        {
            this.history.Clear();
            return;
        }

        public void Delete()
        {
            if (!this.NoDelete)
            {
                if (this.currentProblem.NegativeExists() && this.currentProblem.Raw.Length > 2)
                {
                    this.currentProblem.Raw = this.currentProblem.Raw.Substring(0, this.currentProblem.Raw.Length - 1);
                }
                else if (this.currentProblem.NegativeExists() && this.currentProblem.Raw.Length == 2)
                {
                    this.CE();
                }
                else if (this.currentProblem.Raw.Length > 1)
                {
                    this.currentProblem.Raw = this.currentProblem.Raw.Substring(0, this.currentProblem.Raw.Length - 1);
                }
                else
                {
                    this.CE();
                }
            }
            return;
        }

        public void Div()
        {
            this.currentProblem.Raw += " / ";
            this.Halt = true;
            return;
        }

        public void OneOverX()
        {
            this.currentProblem.Raw = "1/(" + this.currentProblem.Raw + ")";
            this.Halt = true;
            return;
        }

        public void Mul()
        {
            this.currentProblem.Raw += " x ";
            this.Halt = true;
            return;
        }

        public void Percent()
        {
            this.currentProblem.Raw = this.currentProblem.Raw + "%";
            return;
        }

        public void ReverseOperationNumber()
        {
            if (!this.currentProblem.Raw.Equals("0"))
            {
                if (this.currentProblem.NegativeExists())
                {
                    this.currentProblem.Raw = this.currentProblem.Raw.Substring(1);
                }
                else
                {
                    this.currentProblem.Raw = "n" + this.currentProblem.Raw;
                }
            }
            return;
        }

        public object Solve()
        {
            this.NoDelete = false;
            this.history.Add(this.currentProblem);
            var result = this.currentProblem.Solve();
            this.currentProblem.Raw = result;
            return result;
        }

        public void Sqr()
        {
            this.currentProblem.Raw = "(" + this.currentProblem.Raw + "*" + this.currentProblem.Raw + ")";
            this.NoDelete = true;
            return;
        }

        public void Sqrt()
        {
            this.currentProblem.Raw = "Sqrt(" + this.currentProblem.Raw + ")";
            this.NoDelete = true;
            return;
        }

        public void Sub()
        {
            this.currentProblem.Raw += " - ";
            this.Halt = true;
            return;
        }

        public override string ToString()
        {
            return this.currentProblem.Raw.Replace("n", "-");
        }
    }
}
