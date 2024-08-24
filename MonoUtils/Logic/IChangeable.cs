using System;

namespace MonoUtils.Logic;

public interface IChangeable
{
    public event EventHandler HasChanged;
}