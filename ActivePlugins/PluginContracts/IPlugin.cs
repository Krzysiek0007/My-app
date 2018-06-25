using System;

namespace PluginContracts
{
    public interface IPlugin
    {
        string Name { get; }
        string Descripton { get; }
        string Execute(string input);
    }
}
