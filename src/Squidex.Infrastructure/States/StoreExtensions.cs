﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschränkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System;
using System.Threading.Tasks;
using Squidex.Infrastructure.EventSourcing;
using Squidex.Infrastructure.Tasks;

namespace Squidex.Infrastructure.States
{
    public static class StoreExtensions
    {
        public static IPersistence WithEventSourcing<TOwner, TKey>(this IStore<TKey> store, TKey key, Func<Envelope<IEvent>, Task> applyEvent)
        {
            return store.WithEventSourcing(typeof(TOwner), key, applyEvent);
        }

        public static IPersistence<TState> WithSnapshots<TOwner, TState, TKey>(this IStore<TKey> store, TKey key, Func<TState, Task> applySnapshot)
        {
            return store.WithSnapshots(typeof(TOwner), key, applySnapshot);
        }

        public static IPersistence<TState> WithSnapshotsAndEventSourcing<TOwner, TState, TKey>(this IStore<TKey> store, TKey key, Func<TState, Task> applySnapshot, Func<Envelope<IEvent>, Task> applyEvent)
        {
            return store.WithSnapshotsAndEventSourcing(typeof(TOwner), key, applySnapshot, applyEvent);
        }

        public static IPersistence WithEventSourcing<TKey>(this IStore<TKey> store, Type owner, TKey key, Action<Envelope<IEvent>> applyEvent)
        {
            return store.WithEventSourcing(owner, key, applyEvent.ToAsync());
        }

        public static IPersistence<TState> WithSnapshots<TState, TKey>(this IStore<TKey> store, Type owner, TKey key, Action<TState> applySnapshot)
        {
            return store.WithSnapshots(owner, key, applySnapshot.ToAsync());
        }

        public static IPersistence<TState> WithSnapshotsAndEventSourcing<TState, TKey>(this IStore<TKey> store, Type owner, TKey key, Action<TState> applySnapshot, Action<Envelope<IEvent>> applyEvent)
        {
            return store.WithSnapshotsAndEventSourcing(owner, key, applySnapshot.ToAsync(), applyEvent.ToAsync());
        }

        public static IPersistence WithEventSourcing<TOwner, TKey>(this IStore<TKey> store, TKey key, Action<Envelope<IEvent>> applyEvent)
        {
            return store.WithEventSourcing(typeof(TOwner), key, applyEvent.ToAsync());
        }

        public static IPersistence<TState> WithSnapshots<TOwner, TState, TKey>(this IStore<TKey> store, TKey key, Action<TState> applySnapshot)
        {
            return store.WithSnapshots(typeof(TOwner), key, applySnapshot.ToAsync());
        }

        public static IPersistence<TState> WithSnapshotsAndEventSourcing<TOwner, TState, TKey>(this IStore<TKey> store, TKey key, Action<TState> applySnapshot, Action<Envelope<IEvent>> applyEvent)
        {
            return store.WithSnapshotsAndEventSourcing(typeof(TOwner), key, applySnapshot.ToAsync(), applyEvent.ToAsync());
        }

        public static Task WriteEventAsync<T>(IPersistence<T> persistence, Envelope<IEvent> @event)
        {
            return persistence.WriteEventsAsync(new[] { @event });
        }

        public static Task WriteEventAsync<T>(IPersistence<T> persistence, IEvent @event)
        {
            return persistence.WriteEventsAsync(new[] { Envelope.Create(@event) });
        }

        public static Task ClearSnapshotsAsync<TKey, TState>(this IStore<TKey> store)
        {
            return store.GetSnapshotStore<TState>().ClearAsync();
        }

        public static Task RemoveSnapshotAsync<TKey, TState>(this IStore<TKey> store, TKey key)
        {
            return store.GetSnapshotStore<TState>().RemoveAsync(key);
        }

        public static async Task<TState> GetSnapshotAsync<TKey, TState>(this IStore<TKey> store, TKey key)
        {
            var result = await store.GetSnapshotStore<TState>().ReadAsync(key);

            return result.Value;
        }
    }
}
