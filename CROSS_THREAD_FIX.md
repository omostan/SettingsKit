# Cross-Thread Access Fix for SettingsService

## Problem Description

The application was throwing a `System.InvalidOperationException` with the message:
```
Der aufrufende Thread kann nicht auf dieses Objekt zugreifen, da sich das Objekt im Besitz eines anderen Threads befindet.
(The calling thread cannot access this object because a different thread owns it.)
```

### Root Cause

The error occurred when `SettingsService<T>` attempted to save settings from a background thread. The issue happened during JSON serialization when accessing properties that trigger WPF UI operations (like `DataGridColumn.Visibility` through the `IsAllColumnVisible` property).

**Call Stack Analysis:**
1. `ScheduleSave()` creates a background task using `Task.Run`
2. `SaveAsync()` runs on a thread pool thread (due to `ConfigureAwait(false)`)
3. `SaveCore()` serializes settings using `JsonSerializer.Serialize()`
4. JSON serialization accesses the `IsAllColumnVisible` property
5. This property getter triggers UI-related code that accesses WPF dependency properties
6. WPF dependency properties can only be accessed from the UI thread → **Exception thrown**

## Solution

Added **SynchronizationContext support** to automatically marshal save operations to the UI thread when necessary.

### Changes Made

1. **Added SynchronizationContext field**
   - Captures the thread context when `SettingsService<T>` is initialized
   - If initialized on the UI thread, the context will be the WPF `DispatcherSynchronizationContext`

2. **Capture context in constructor**
   ```csharp
   _synchronizationContext = SynchronizationContext.Current;
   ```

3. **Modified SaveAsync method**
   - Checks if a `SynchronizationContext` was captured
   - If yes: marshals the `SaveCore()` operation to the original thread using `Post()`
   - If no: executes `SaveCore()` directly on the background thread (console apps, etc.)

### How It Works

```
UI Thread                          Background Thread
---------                          -----------------
PropertyChanged event
    ↓
ScheduleSave() ----------------------→ Task.Run
    ↓                                      ↓
    ↓                                  SaveAsync()
    ↓                                      ↓
    ↓                                  Check _synchronizationContext
    ↓                                      ↓
    ↓ ←-------------------------------- Post() callback
    ↓
SaveCore() (on UI thread)
    ↓
JSON Serialization (safe access to UI properties)
```

## Benefits

✅ **Automatic thread safety** - No manual dispatcher calls required  
✅ **Backward compatible** - Works with console apps (no sync context)  
✅ **WPF compatible** - Handles UI thread requirements automatically  
✅ **Cross-platform** - Works with any `SynchronizationContext` (WinForms, WPF, etc.)  

## Testing Recommendations

1. Test settings save in WPF applications with UI-bound properties
2. Verify console applications still work (no sync context)
3. Test rapid property changes to ensure debouncing still works
4. Verify thread-safety with concurrent access scenarios

## Version Impact

This fix should be included in:
- **Version 1.0.2** - As a bug fix for the cross-thread access issue
- Update release notes to mention "Fixed cross-thread access violations in WPF applications"

---

**Date**: February 10, 2026  
**Issue**: Cross-thread access violation in WPF applications  
**Resolution**: Added SynchronizationContext support for automatic UI thread marshalling

