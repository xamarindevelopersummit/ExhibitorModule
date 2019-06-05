using System;
using System.Collections.Generic;
using SQLite;

namespace ExhibitorModule.Data.Abstractions
{
    public interface IDatabase
    {
		int CreateTable<T>();
        int CreateTable<T>(CreateFlags flags);
        List<T> All<T>() where T : new();
        T FindWithQuery<T>(string query, params object[] args) where T : new();
        List<T> Query<T>(string query, params object[] args) where T : new();
        int DropTable<T>();
        int Insert(object obj, Type objType);
        int Delete(object objectToDelete);
        int Update(object obj, Type objType);
    }
}
