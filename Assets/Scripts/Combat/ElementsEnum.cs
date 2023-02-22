using System;

namespace MVC.Model.Elements
{
    [Flags]
    public enum ElementsEnum
    {
        None = 0,
        Geo = 1,
        Pyro = 2,
        Hydro = 4,
        Electro = 5
    }
}


