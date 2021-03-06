﻿using System;
using System.Collections;
using System.Linq;
using BusinessLight.Domain;

namespace BusinessLight.Data.InMemory
{
    internal class InMemoryRepository : IRepository
    {
        private bool _disposed;
        private IList _source;

        public InMemoryRepository(IList source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            _source = source;
        }

        public void Add<T>(T entity) where T : UniqueEntity
        {
            Remove(entity);
            _source.Add(entity);
        }

        public void Update<T>(T entity) where T : UniqueEntity
        {
            Remove(entity);
            Add(entity);
        }

        public void Remove<T>(T entity) where T : UniqueEntity
        {
             _source.Remove(entity);
        }

        public IQueryable<T> Query<T>() where T : UniqueEntity
        {
            return _source.OfType<T>().AsQueryable();
        }

        public IQueryable<T> ApplyQuery<T>(IQuery<T> query) where T : UniqueEntity
        {
            return Query<T>().Where(query.GetFilterExpression());
        }

        public IOrderedQueryable<T> ApplyQuery<T>(ISortedQuery<T> query) where T : UniqueEntity
        {
            return query.GetSortingExpression()(Query<T>().Where(query.GetFilterExpression()));
        }

        public T GetById<T>(Guid id) where T : UniqueEntity
        {
            return _source.OfType<T>().Single(x => x.Id == id);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                 _source = null;
            }

            _disposed = true;
        }
    }
}
