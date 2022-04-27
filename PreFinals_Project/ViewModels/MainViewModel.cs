using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Helpers;
using PreFinals_Project.Models;

namespace PreFinals_Project.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private string _firstPolynomialString;
        private string _secondPolynomialString;
        private string _result;
        private string _solutionText;
        private bool _isCalculateCommandEnabled;
        private string _operationToUse;

        public string FirstPolynomialString
        {
            get => _firstPolynomialString;
            set
            {
                if (value != null) if (Regex.IsMatch(value,"[^A-Za-z0-9.,+-/*^ ]")) return;
                _firstPolynomialString = value;
                RaisePropertyChanged(nameof(FirstPolynomialString));
                CalculateCommandEnabler();
            }
        }
        public string SecondPolynomialString
        {
            get => _secondPolynomialString;
            set
            {
                if (value != null) if (Regex.IsMatch(value, "[^A-Za-z0-9.,+-/*^ ]")) return;
                _secondPolynomialString = value;
                RaisePropertyChanged(nameof(SecondPolynomialString));
                CalculateCommandEnabler();
            }
        }
        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                RaisePropertyChanged(nameof(Result));
            }
        }
        public string SolutionText
        {
            get { return _solutionText; }
            set
            {
                _solutionText = value;
                RaisePropertyChanged(nameof(SolutionText));
            }
        }
        public string OperationToUse
        {
            get { return _operationToUse; }
            set
            {
                _operationToUse = value;
                RaisePropertyChanged(nameof(OperationToUse));
                CalculateCommandEnabler();
            }
        }

        public ObservableCollection<string> PolynomialOperations { get; set; }
        public ICommand CalculateCommand => new RelayCommand(Calculate);
        public ICommand ResetCommand => new RelayCommand(Reset);
        public bool IsCalculateCommandEnabled
        {
            get { return _isCalculateCommandEnabled; }
            set
            {
                _isCalculateCommandEnabled = value;
                RaisePropertyChanged(nameof(IsCalculateCommandEnabled));
            }
        }

        public MainViewModel()
        {
            PolynomialOperations = InitializePolynomialOperations();
            Result = "";
            SolutionText = "";
        }

        private ObservableCollection<string> InitializePolynomialOperations()
        {
            var operations = new ObservableCollection<string>();
            operations.Add("Add");
            operations.Add("Subtract");
            operations.Add("Multiply");
            operations.Add("Divide");
            return operations;
        }

        private void Calculate()
        {
            try
            {
                var firstPolynomial = new Polynomial(FirstPolynomialString);
                var secondPolynomial = new Polynomial(SecondPolynomialString);
                Polynomial resultPolynomial;
                switch (OperationToUse)
                {
                    case "Add":
                        resultPolynomial = firstPolynomial.Add(secondPolynomial);
                        Result = resultPolynomial.ToString();
                        SolutionText = resultPolynomial.SolutionText.ToString();
                        break;
                    case "Subtract":
                        resultPolynomial = firstPolynomial.Subtract(secondPolynomial);
                        Result = resultPolynomial.ToString();
                        SolutionText = resultPolynomial.SolutionText.ToString();
                        break;
                    case "Multiply":
                        resultPolynomial = firstPolynomial.Multiply(secondPolynomial);
                        Result = resultPolynomial.ToString();
                        SolutionText = resultPolynomial.SolutionText.ToString();
                        break;
                    default:
                        resultPolynomial = firstPolynomial.Divide(secondPolynomial);
                        Result = resultPolynomial.ToString();
                        SolutionText = resultPolynomial.SolutionText.ToString();
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Polynomials to {OperationToUse} are not valid. Please try again.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                Reset();
            }
        }

        private void Reset()
        {
            FirstPolynomialString = null;
            SecondPolynomialString = null;
            OperationToUse = null;
            Result = "";
            SolutionText = "";
        }
        private void CalculateCommandEnabler()
        {
            if (_firstPolynomialString == null)
            {
                IsCalculateCommandEnabled = false;
                return;
            }
            if (_secondPolynomialString == null)
            {
                IsCalculateCommandEnabled = false;
                return;
            }
            if (_operationToUse == null)
            {
                IsCalculateCommandEnabled = false;
                return;
            }
            if (_firstPolynomialString.Trim() == string.Empty || _firstPolynomialString.Trim() == "")
            {
                IsCalculateCommandEnabled = false;
                return;
            }
            if (_secondPolynomialString.Trim() == string.Empty || _secondPolynomialString.Trim() == "")
            {
                IsCalculateCommandEnabled = false;
                return;
            }
            if (_operationToUse.Trim() == string.Empty || _operationToUse.Trim() == "")
            {
                IsCalculateCommandEnabled = false;
                return;
            }

            IsCalculateCommandEnabled = true;
        }
    }
}
