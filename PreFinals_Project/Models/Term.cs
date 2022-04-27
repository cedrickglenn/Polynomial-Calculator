using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PreFinals_Project.DoublyLinkedList_Class;

namespace PreFinals_Project.Models
{
    public class Term : IComparable<Term>
    {
        public double Coefficient { get; internal set; }
        public List<Variable> Variables { get; private set; }
        public string VariablesInString { get; private set; }
        public string VariablesInLatexForm { get; private set; }
        public string VariableCharactersInString { get; private set; }
        public int TotalDegree { get; private set; }
        public bool NotAdded { get; internal set; }
        public ILinkedList<Term> DenominatorTerms { get; internal set; }
        public Term(double coefficient, List<Variable> variables)
        {
            Coefficient = coefficient;
            Variables = ProcessVariables(variables);
            VariablesInString = ProcessVariableInString();
            VariablesInLatexForm = ProcessVariableInLatexForm();
            VariableCharactersInString = ProcessVariableCharactersInString();
            TotalDegree = GetTotalDegree();
            NotAdded = true;
        }
        public Term(double coefficient, List<Variable> variables, ILinkedList<Term> denominatorTerms)
        {
            Coefficient = coefficient;
            DenominatorTerms = denominatorTerms;
            Variables = ProcessVariables(variables);
            VariablesInString = ProcessVariableInString();
            VariablesInLatexForm = ProcessVariableInLatexForm();
            VariableCharactersInString = ProcessVariableCharactersInString();
            TotalDegree = GetTotalDegree();
            NotAdded = true;
        }
        public Term(double coefficient, List<Variable> variablesA,List<Variable> variablesB)
        {
            var newVariablesA = new List<Variable>();
            var newVariablesB = new List<Variable>();
            foreach (var variable in variablesA) newVariablesA.Add(new Variable(variable.Character, variable.Exponent));
            foreach (var variable in variablesB) newVariablesB.Add(new Variable(variable.Character, variable.Exponent));
            Coefficient = coefficient;
            Variables = ProcessVariablesForDivision(newVariablesA,newVariablesB);
            VariablesInString = ProcessVariableInString();
            VariablesInLatexForm = ProcessVariableInLatexForm();
            VariableCharactersInString = ProcessVariableCharactersInString();
            TotalDegree = GetTotalDegree();
            NotAdded = true;
        }


        private List<Variable> ProcessVariables(List<Variable> variables)
        {
            var newVariables = new List<Variable>();
            foreach (var variable in variables)
            {
                var newExponent = 0;
                var firstOccurrence = true;
                foreach (var checkingVariable in variables)
                {
                    if (variable == checkingVariable) continue;
                    if ((variable.Character == checkingVariable.Character) && checkingVariable.NotAdded)
                    {
                        if (firstOccurrence) newExponent += variable.Exponent + checkingVariable.Exponent;
                        else newExponent += checkingVariable.Exponent;
                        variable.NotAdded = false;
                        checkingVariable.NotAdded = false;
                        firstOccurrence = false;
                    }
                }
                if (!variable.NotAdded && (newExponent != 0)) newVariables.Add(new Variable(variable.Character, newExponent));
            }
            foreach (var variable in variables)
                if (variable.NotAdded && (variable.Exponent != 0))
                    newVariables.Add(variable);
            newVariables.Sort();
            return newVariables;
        }

        private List<Variable> ProcessVariablesForDivision(List<Variable> dividendVariables, List<Variable> divisorVariables)
        {
            var newVariables = new List<Variable>();
            foreach (var variable in dividendVariables)
            {
                var newExponent = 0;
                var isFirstOccurrence = true;
                foreach (var checkingVariable in divisorVariables)
                {
                    if (variable == checkingVariable) continue;
                    if ((variable.Character == checkingVariable.Character) && checkingVariable.NotAdded)
                    {
                        if (isFirstOccurrence) newExponent = variable.Exponent - checkingVariable.Exponent;
                        else newExponent -= checkingVariable.Exponent;
                        variable.NotAdded = false;
                        checkingVariable.NotAdded = false;
                        isFirstOccurrence = false;
                    }
                }
                if (!variable.NotAdded && (newExponent != 0)) newVariables.Add(new Variable(variable.Character, newExponent));
            }
            foreach (var variable in dividendVariables)
                if (variable.NotAdded && (variable.Exponent != 0))
                    newVariables.Add(variable);
            newVariables.Sort();
            return newVariables;
        }

        private string ProcessVariableInString()
        {
            var sb = new StringBuilder();
            foreach (var variable in Variables)
            {
                sb.Append(variable);
            }
            if (sb.Length == 0)
                return string.Empty;
            sb.Replace("{", string.Empty);
            sb.Replace("}", string.Empty);
            return sb.ToString();
        }
        private string ProcessVariableInLatexForm()
        {
            var sb = new StringBuilder();
            foreach (var variable in Variables)
            {
                sb.Append(variable);
            }
            if (sb.Length == 0)
                return string.Empty;
            return sb.ToString();
        }
        private string ProcessVariableCharactersInString()
        {
            var sb = new StringBuilder();
            foreach (var variable in Variables)
            {
                sb.Append(variable.Character);
            }
            if (sb.Length == 0)
                return string.Empty;
            return sb.ToString();
        }
        public int CompareTo(Term other)
        {
            if (other == null) return 1;
            IComparer<int> comparerA = Comparer<int>.Default;
            IComparer<string> comparerB = Comparer<string>.Default;
            if (TotalDegree == other.TotalDegree) return comparerB.Compare(VariableCharactersInString, other.VariableCharactersInString);
            return comparerA.Compare(other.TotalDegree, TotalDegree);
        }

        private int GetTotalDegree()
        {
            var allDegrees = new List<int>();
            foreach (var variable in Variables) allDegrees.Add(variable.Exponent);
            return allDegrees.Count == 0 ? 0 : allDegrees.Sum();
        }


        public override string ToString()
        {
            var sb = new StringBuilder();
            if (DenominatorTerms != null)
            {
                sb.Append(@"\frac{");
                if (Coefficient == 1 && VariablesInLatexForm != string.Empty) sb.Append(VariablesInLatexForm);
                else sb.Append($"{Math.Abs(Coefficient)}{VariablesInLatexForm}");
                sb.Append("}{");
                foreach (var denominatorTerm in DenominatorTerms)
                {
                    sb.Append($"{denominatorTerm} + ");
                }
                sb.Replace("+ -", "- ");
                if (sb.Length > 0) sb.Remove(sb.Length - 3, 3);
                sb.Append("}");
                return sb.ToString();
            }
            if (Coefficient == 1 && VariablesInLatexForm != string.Empty) sb.Append(VariablesInLatexForm);
            else sb.Append($"{Math.Abs(Coefficient)}{VariablesInLatexForm}");
            return sb.ToString();
        }
    }
}
