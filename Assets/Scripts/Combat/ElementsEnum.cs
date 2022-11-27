using System;

[Flags]
public enum ElementsEnum 
{
    None = 0,
    Geo = 0 >> 1,
    Pyro = 0 >> 2,
    Hydro = 0 >> 3
}
