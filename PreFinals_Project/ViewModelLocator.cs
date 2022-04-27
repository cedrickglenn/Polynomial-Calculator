using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreFinals_Project.ViewModels;

namespace PreFinals_Project
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel { get; set; }

        public ViewModelLocator()
        {
            MainViewModel = new MainViewModel();
        }
    }
}
