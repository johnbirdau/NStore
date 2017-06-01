﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NStore.Persistence
{
    public class PartitionRecorder : IPartitionConsumer
    {
        private sealed class Element
        {
            public Element(long index, object payload)
            {
                Index = index;
                Payload = payload;
            }

            public long Index { get; }
            public object Payload { get; }
        }

        private readonly IList<Element> _data = new List<Element>();
        private readonly IDictionary<long, object> _map = new Dictionary<long, object>();
        public IEnumerable<object> Data => _data;
        public int Length => _data.Count;
        public bool ReadCompleted { get; private set; }

        public Task<bool> OnNext(IPartitionData data)
        {
            _data.Add(new Element(data.Index, data.Payload));
            _map[data.Index] = data.Payload;
            return Task.FromResult(true);
        }

        public Task Completed()
        {
            ReadCompleted = true;
            return Task.CompletedTask;
        }

        public Task OnError(Exception ex)
        {
            throw ex;
        }

        public void Replay(Action<object> action)
        {
            Replay(action, 0);
        }

        public void Replay(Action<object> action, int startAt)
        {
            for (var i = startAt; i < _data.Count; i++)
            {
                action(_data[i].Payload);
            }
        }

        public bool IsEmpty => _data.Count == 0;
        public object this[int position] => _data[position].Payload;

        public long GetIndex(int position) => _data[position].Index;
        public object ByIndex(int index) => _map[index];
    }
}