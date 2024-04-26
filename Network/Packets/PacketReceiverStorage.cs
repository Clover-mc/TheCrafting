using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Minecraft.Network.Packets;

public sealed class PacketReceiverStorage : IDictionary<byte, IPacketReceiver>
{
    readonly Dictionary<byte, IPacketReceiver> _receivers = new();

    public IPacketReceiver this[byte key]
    {
        get => _receivers[key];
        set => Add(key, value);
    }

    public ICollection<byte> Keys => _receivers.Keys;

    public ICollection<IPacketReceiver> Values => _receivers.Values;

    public int Count => _receivers.Count;

    public bool IsReadOnly => false;

    public void Add(byte key, IPacketReceiver value)
    {
        if (_receivers.TryGetValue(key, out var receiver))
        {
            if (receiver.AllowOverride)
            {
                _receivers[key] = value;
            }
            else
            {
                throw new ArgumentException($"Packet with id {key} do not allow overriding!", nameof(key));
            }
        }
        else
        {
            _receivers.Add(key, value);
        }
    }

    public void Add(KeyValuePair<byte, IPacketReceiver> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        throw new NotSupportedException("Clearing storage is not allowed!");
    }

    public bool Contains(KeyValuePair<byte, IPacketReceiver> item)
    {
        return _receivers.TryGetValue(item.Key, out var receiver) && receiver == item.Value;
    }

    public bool ContainsKey(byte key)
    {
        return _receivers.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<byte, IPacketReceiver>[] array, int arrayIndex)
    {
        _receivers.ToArray().CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<byte, IPacketReceiver>> GetEnumerator()
    {
        return _receivers.GetEnumerator();
    }

    public bool Remove(byte key)
    {
        return _receivers.Remove(key);
    }

    public bool Remove(KeyValuePair<byte, IPacketReceiver> item)
    {
        return Remove(item.Key);
    }

    public bool TryGetValue(byte key, [MaybeNullWhen(false)] out IPacketReceiver value)
    {
        return _receivers.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _receivers.GetEnumerator();
    }
}
