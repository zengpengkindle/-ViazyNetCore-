using System.Diagnostics.CodeAnalysis;

namespace ViazyNetCore.Core;

public class DisposeAction : IDisposable
{
    private readonly Action _action;

    /// <summary>
    /// Creates a new <see cref="DisposeAction"/> object.
    /// </summary>
    /// <param name="action">Action to be executed when this object is disposed.</param>
    public DisposeAction([NotNull] Action action)
    {
        Check.NotNull(action, nameof(action));

        _action = action;
    }

    public void Dispose()
    {
        _action();
    }
}
