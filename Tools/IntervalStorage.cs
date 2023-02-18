using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minecraft.Tools
{
    public class IntervalStorage
    {
        private ulong Index;
        private Dictionary<ulong, bool> Storage;

        public IntervalStorage()
        {
            Index = 0;
            Storage = new Dictionary<ulong, bool>();
        }

        public ulong Create(Action<ulong> func, TickSpan interval, bool instantDelay = false) => Create(func, interval.TimeSpan, instantDelay);
        public ulong Create(Action<ulong> func, TimeSpan interval, bool instantDelay = false)
        {
            ulong given = Index;
            Storage.Add(given, true);
            Task.Run(async () =>
            {
                if (instantDelay) await Task.Delay(interval);
                while (IsWorking(given))
                {
                    func?.Invoke(given);
                    await Task.Delay(interval);
                }
            });
            Index++;
            return given;
        }

        public bool IsWorking(ulong index)
        {
            return Storage.ContainsKey(index) && Storage[index];
        }

        public void Cancel(ulong index)
        {
            if (Storage.ContainsKey(index))
            {
                Storage[index] = false;
            }
        }
    }
}
