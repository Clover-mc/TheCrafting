using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minecraft.Tools;

public class IntervalStorage
{
    ulong _index = 0;
    readonly Dictionary<ulong, bool> _storage = new();

    public ulong Create(Action<ulong> func, TimeSpan interval, bool instantDelay = false)
    {
        ulong given = _index++;
        _storage.Add(given, true);
        
        Task.Run(async () =>
        {
            if (instantDelay)
            {
                await Task.Delay(interval);
            }

            while (IsWorking(given))
            {
                try
                {
                    func?.Invoke(given);
                }
                catch (Exception ex)
                {
                    ConsoleWrapper.ConsoleWriter.WriteError(ex);
                }

                await Task.Delay(interval);
            }

            _storage.Remove(given);
        });
        
        return given;
    }

    public bool IsWorking(ulong index)
    {
        return _storage.ContainsKey(index) && _storage[index];
    }

    public void Cancel(ulong index)
    {
        if (_storage.ContainsKey(index))
        {
            _storage[index] = false;
        }
    }
}
