﻿using System;
using System.Threading.Tasks;

namespace NStore.Core.Persistence
{
    public class SubscriptionWrapper : ISubscription
    {
        private readonly ISubscription _wrapped;

        public SubscriptionWrapper(ISubscription wrapped)
        {
            _wrapped = wrapped;
            ChunkFilter = c => true;
        }

        public Action<IChunk> BeforeOnNext { get; set; }
        public Func<IChunk, bool> ChunkFilter { get; set; }

        public async Task<bool> OnNextAsync(IChunk chunk)
        {
            if (ChunkFilter(chunk))
            {
                BeforeOnNext?.Invoke(chunk);
                return await _wrapped.OnNextAsync(chunk).ConfigureAwait(false);
            }

            return true;
        }

        public async Task CompletedAsync(long indexOrPosition)
        {
            await _wrapped.CompletedAsync(indexOrPosition).ConfigureAwait(false);
        }

        public async Task StoppedAsync(long indexOrPosition)
        {
            await _wrapped.StoppedAsync(indexOrPosition).ConfigureAwait(false);
        }

        public async Task OnStartAsync(long indexOrPosition)
        {
            await _wrapped.OnStartAsync(indexOrPosition).ConfigureAwait(false);
        }

        public async Task OnErrorAsync(long indexOrPosition, Exception ex)
        {
            await _wrapped.OnErrorAsync(indexOrPosition, ex).ConfigureAwait(false);
        }
    }
}