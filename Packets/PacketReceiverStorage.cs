using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Minecraft.Packets
{
    public class PacketReceiverStorage : IDictionary<byte, IPacketReceiver>
    {
        private readonly Dictionary<byte, IPacketReceiver> Receivers;

        internal PacketReceiverStorage() => Receivers = new Dictionary<byte, IPacketReceiver>();

        public IPacketReceiver this[byte key] { get => Receivers[key]; set => throw new NotImplementedException(); }

        public ICollection<byte> Keys => Receivers.Keys;

        public ICollection<IPacketReceiver> Values => Receivers.Values;

        public int Count => Receivers.Count;

        public bool IsReadOnly => false;

        public void Add(byte key, IPacketReceiver value)
        {
            if (Receivers.ContainsKey(key))
                if (Receivers[key].AllowOverride)
                    Receivers[key] = value;
                else
                    throw new ArgumentException(nameof(IPacketReceiver) + '(' + key + ") does not allow overriding!", nameof(key));

            else Receivers.Add(key, value);
        }

        public void Add(KeyValuePair<byte, IPacketReceiver> item) =>Add(item.Key, item.Value);

        public void Clear()
        {
            throw new InvalidOperationException(nameof(PacketReceiverStorage) + " does not allow clearing!");
        }

        public bool Contains(KeyValuePair<byte, IPacketReceiver> item) => Receivers.ContainsKey(item.Key) && Receivers[item.Key] == item.Value;

        public bool ContainsKey(byte key) => Receivers.ContainsKey(key);

        public void CopyTo(KeyValuePair<byte, IPacketReceiver>[] array, int arrayIndex)
        {
            throw new NotImplementedException(nameof(PacketReceiverStorage) + " does not allow inserting by index!");
        }

        public IEnumerator<KeyValuePair<byte, IPacketReceiver>> GetEnumerator() => Receivers.GetEnumerator();

        public bool Remove(byte key)
        {
            if (Receivers.TryGetValue(key, out IPacketReceiver? value) && !value.AllowOverride)
                throw new InvalidOperationException(nameof(IPacketReceiver) + '(' + key + ") does not allow overriding!");

            return Receivers.Remove(key);
        }

        public bool Remove(KeyValuePair<byte, IPacketReceiver> item) => Contains(item) && Remove(item.Key);

        public bool TryGetValue(byte key, [MaybeNullWhen(false)] out IPacketReceiver value) => Receivers.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => Receivers.GetEnumerator();
    }
}
