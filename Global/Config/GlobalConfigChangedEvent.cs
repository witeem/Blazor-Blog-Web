﻿namespace BlazorServerApp.Global;
public partial class GlobalConfigChangedEvent
{
    public delegate void GlobalConfigChanged();
    public event GlobalConfigChanged OnGlobalConfigChanged;
    public event GlobalConfigChanged NavigationStateHasChanged;

    public void Invoke()
    {
        OnGlobalConfigChanged?.Invoke();
    }

    public void InvokeNavigationStateHasChanged()
    {
        NavigationStateHasChanged?.Invoke();
    }
}
