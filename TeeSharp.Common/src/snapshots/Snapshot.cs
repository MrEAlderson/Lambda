﻿using TeeSharp.Common.Enums;
using TeeSharp.Core;

namespace TeeSharp.Common.Snapshots
{
    public class Snapshot
    {
        public const int MaxParts = 64;
        public const int MaxSize = MaxParts * 1024;
        public const int MaxPacketSize = 900;

        public SnapshotItem this[int index] => _items[index];

        public readonly int Size;
        public int ItemsCount => _items.Length;

        private SnapshotItem[] _items;

        public Snapshot(SnapshotItem[] items, int size)
        {
            Size = size;
            _items = items;
        }

        public static SnapshotItems Type(int key)
        {
            return (SnapshotItems) (key >> 16);
        }

        public static int Id(int key)
        {
            return key & 0xffff;
        }

        public static int Key(int id, SnapshotItems type)
        {
            return (int) type << 16 | id;
        }

        public SnapshotItem FindItem(int id, SnapshotItems type)
        {
            return FindItem(Key(id, type));
        }

        public SnapshotItem FindItem(int key)
        {
            for (var i = 0; i < _items.Length; i++)
            {
                if (_items[i].Key == key)
                    return _items[i];
            }

            return null;
        }

        public int Crc()
        {
            int crc = 0;

            for (var i = 0; i < _items.Length; i++)
            {
                var data = _items[i].Item.Serialize();
                for (var field = 0; field < data.Length; field++)
                {
                    crc += data[field];
                }
            }

            return crc;
        }

        public void DebugDump()
        {
            Debug.Log("snapshot", $"data_size={Size} num_items={ItemsCount}");
            for (var i = 0; i < _items.Length; i++)
            {
                Debug.Log("snapshot", $"type={_items[i].Type} id={_items[i].Id}");
                var data = _items[i].Item.Serialize();
                for (var field = 0; field < data.Length; field++)
                    Debug.Log("snapshot", $"field={field} value={data[field]}");
            }
        }
    }
}